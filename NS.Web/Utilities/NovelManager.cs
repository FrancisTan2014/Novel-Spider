using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using NS.Models;
using NS.Utilities;
using WebGrease.Css.Extensions;

namespace NS.Web.Utilities
{
    /// <summary>
    /// 小说管理类（下载、保存）
    /// </summary>
    public class NovelManager
    {
        #region Properties
        private readonly Spider _novelSpider = new Spider(new QuanshuAnalyzer());

        private readonly Queue<KeyValuePair<int, Novel>> _novelDownloadQueue = new Queue<KeyValuePair<int, Novel>>();

        private readonly Queue<Chapter> _chapterDownloadQueue = new Queue<Chapter>();

        private int _maxDownloadTaskCount;
        /// <summary>
        /// 下载小说时的最大线程数
        /// </summary>
        public int MaxDownloadTaskCount
        {
            get { return _maxDownloadTaskCount; }
            set
            {
                if (value <= 0)
                {
                    _maxDownloadTaskCount = 10;

                }
                else if (value > 10)
                {
                    _maxDownloadTaskCount = 10;
                }
                else
                {
                    _maxDownloadTaskCount = value;
                }
            }
        }

        private int _chapterDownloadTaskLimit;
        /// <summary>
        /// 下载章节内容时允许最大线程数
        /// </summary>
        public int ChapterDownloadTaskLimit
        {
            get { return _chapterDownloadTaskLimit; }
            set
            {
                if (value <= 0)
                {
                    _chapterDownloadTaskLimit = 50;
                }
                else if (value > 50)
                {
                    _chapterDownloadTaskLimit = 50;
                }
                else
                {
                    _chapterDownloadTaskLimit = value;
                }
            }
        }

        public List<NovelType> TypeList { get; set; }
        #endregion

        #region Methods
        public NovelManager()
        {
            MaxDownloadTaskCount = 10;
            ChapterDownloadTaskLimit = 50;
        }

        public void Start()
        {
            ProcessNovelTypes();
        }

        private void ProcessNovelTypes()
        {
            var typeDic = _novelSpider.GetNovelTypes();
            UpdateNovelTypes(typeDic.Keys.Select(key => key.RemoveSpace()).ToList());

            ProcessNovelUrls(typeDic);
        }

        private void ProcessNovelUrls(Dictionary<string, string> typeDic)
        {
            typeDic.ForEach(couple =>
            {
                Task.Factory.StartNew(() =>
                {
                    var typeName = couple.Key.RemoveSpace();
                    var typeId = TypeList.Find(type => type.TypeName.Equals(typeName)).Id;
                    var typeUrl = HttpHelper.RelateToAbsolute(_novelSpider.Analyzer.NovelSiteIndexUrl, couple.Value);

                    var totalPageCount = _novelSpider.GetTotalPageCount(typeUrl);
                    for (var index = 0; index < totalPageCount; index++)
                    {
                        var novelUrlDic = _novelSpider.GetNovelUrls(typeUrl, index);
                        novelUrlDic.ForEach(novelUrl =>
                        {
                            try
                            {
                                var tempUrl = HttpHelper.RelateToAbsolute(typeUrl, novelUrl.Value);
                                var novel = _novelSpider.GetNovelInfo(tempUrl);
                                if (novel != null)
                                {
                                    novel.Name = novelUrl.Key.RemoveSpace().RemoveHtmlTags();
                                    novel.CoverUrl = HttpHelper.RelateToAbsolute(tempUrl, novel.CoverUrl);

                                    _novelDownloadQueue.Enqueue(new KeyValuePair<int, Novel>(typeId, novel));
                                    StartDownloadNovel();
                                    StartDownloadChapter();
                                }
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        });
                    }
                });
            });
        }

        private void StartDownloadNovel()
        {
            var exit = -1; // 当它变为1时，退出无限循环
            while (exit == -1)
            {
                try
                {
                    var data = _novelDownloadQueue.Dequeue();
                    var novel = UpdateDatabase(data.Key, data.Value);

                    ProcessChapters(novel);

                    Thread.Sleep(50);
                }
                catch (InvalidOperationException ex)
                {
                    exit++;
                }
            }
        }

        private void StartDownloadChapter()
        {
            Thread.Sleep(300);

            var tasks = new List<Task>();
            for (var i = 0; i < ChapterDownloadTaskLimit; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    using (var _db = new NsContext())
                    {
                        var exit = -1; // 当它变为2时，退出无限循环
                        while (exit < 1)
                        {
                            try
                            {
                                var chapter = _chapterDownloadQueue.Dequeue();

                                var chapterInDb =
                                _db.Chapters.SingleOrDefault(
                                    c => c.NovelId == chapter.NovelId && chapter.TitleWithNoSpace == c.TitleWithNoSpace);
                                if (chapterInDb == null)
                                {
                                    var content = _novelSpider.GetChapterContent(chapter.Url);
                                    var length = content.ComputeWordCount();

                                    chapter.Content = content;
                                    chapter.WordCount = length;
                                    chapter.UpdateTime = DateTime.Now;

                                    _db.Chapters.Add(chapter);
                                    _db.SaveChanges();
                                }

                                Thread.Sleep(5);
                            }
                            catch (InvalidOperationException ex)
                            {
                                exit++;
                                Thread.Sleep(3000);
                            }
                            catch (Exception ex)
                            {
                                Thread.Sleep(50);
                            }
                        }
                    }
                });

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }

        private Novel UpdateDatabase(int typeId, Novel novel)
        {
            var lockObj = new object();
            using (var _db = new NsContext())
            {
                lock (lockObj)
                {
                    // 保存作者信息
                    var authorName = novel.Author.Name.RemoveSpace().RemoveHtmlTags().RemovePunctuations();
                    var author = _db.Authors.SingleOrDefault(a => a.Name.Equals(authorName));
                    if (author == null)
                    {
                        novel.Author = _db.Authors.Add(novel.Author);
                        _db.SaveChanges();
                    }
                    else
                    {
                        novel.Author = author;
                    }

                    novel.AuthorId = novel.Author.Id;

                    // 若为新小说则插入数据库
                    var novelInDb = _db.Novels.SingleOrDefault(n => n.AuthorId == novel.AuthorId && n.Name == novel.Name);
                    if (novelInDb == null)
                    {
                        novel.UpdateTime = DateTime.Now;
                        novel = _db.Novels.Add(novel);
                        _db.SaveChanges();
                    }
                    else
                    {
                        novel = novelInDb;
                    }

                    // 建立小说与分类的关联关系
                    var relate = _db.NovelTypeRelate.SingleOrDefault(r => r.NovelTypeId == typeId && r.NovelId == novel.Id);
                    if (relate == null)
                    {
                        _db.NovelTypeRelate.Add(new NovelTypeRelate
                        {
                            NovelTypeId = typeId,
                            NovelId = novel.Id,
                            UpdateTime = DateTime.Now
                        });
                        _db.SaveChanges();
                    }
                }
            }

            return novel;
        }

        private void ProcessChapters(Novel novel)
        {
            var chapterUrls = _novelSpider.GetChapterUrls(novel.ChapterListUrl);

            var sort = 0;
            foreach (var item in chapterUrls)
            {
                var chapterName = item.Key;
                var titleWithNoSpace = chapterName.RemoveSpace();
                var chapterDownloadUrl = HttpHelper.RelateToAbsolute(novel.ChapterListUrl, item.Value);

                _chapterDownloadQueue.Enqueue(new Chapter
                {
                    Title = chapterName,
                    Url = chapterDownloadUrl,
                    Sort = sort++,
                    TitleWithNoSpace = titleWithNoSpace,
                    NovelId = novel.Id
                });
            }
        }

        private void UpdateNovelTypes(List<string> typesList)
        {
            using (var _db = new NsContext())
            {
                var typesInDb = _db.NovelTypes.Where(t => t.IsDelete == false).Select(t => t.TypeName);
                var typesToInsert = typesList.Except(typesInDb);

                typesToInsert.ForEach(t =>
                {
                    _db.NovelTypes.Add(new NovelType
                    {
                        AddTime = DateTime.Now,
                        TypeName = t
                    });
                });

                _db.SaveChanges();

                TypeList = _db.NovelTypes.Where(t => t.IsDelete == false).ToList();
            }
        }
        #endregion
    }
}