using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Core;

namespace ArticleService
{
    public class ArticleService : Articles.ArticlesBase
    {
        private readonly ArticleList _list = new ArticleList();

        public ArticleService()
        {
            List<Article> articles = new List<Article>
            {
                new Article
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = "Article One",
                    Content = "Article one's content",
                    Published = true,
                    PublishedAt = DateTime.Now.AddDays(1).ToString()
                },
                new Article
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = "Article Two",
                    Content = "Article two's content",
                    Published = false,
                    PublishedAt = DateTime.Now.ToString()
                },
                new Article
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = "Article Three",
                    Content = "Article three's content",
                    Published = true,
                    PublishedAt = DateTime.Now.ToString()
                }
            };

            _list.Articles.AddRange(articles);
        }
        
        public override Task<ArticleList> GetArticleList(EmptyRequest request, ServerCallContext context)
        {
            return Task.FromResult(_list);
        }

        public override Task<Article> GetArticleDetail(ArticleDetail request, ServerCallContext context)
        {
            return Task.FromResult(_list.Articles.First(a => a.Guid.Equals(request.Guid)));
        }

        public override Task<Article> CreateArticle(Article request, ServerCallContext context)
        {
            _list.Articles.Add(request);

            return Task.FromResult(_list.Articles.First(a => a.Guid.Equals(request.Guid)));
        }

        public override Task<Article> PublishArticle(ArticleDetail request, ServerCallContext context)
        {
            Article article = _list.Articles.First(a => a.Guid.Equals(request.Guid));
            article.Published = true;
            article.PublishedAt = DateTime.Now.ToString();

            return Task.FromResult(article);
        }

        public override Task<Article> UnpublishArticle(ArticleDetail request, ServerCallContext context)
        {
            Article article = _list.Articles.First(a => a.Guid.Equals(request.Guid));
            article.Published = false;
            article.PublishedAt = DateTime.Now.ToString();

            return Task.FromResult(article);
        }
    }
}