using System;
using System.Linq;
using AutoMapper;
using NHibernate.Linq;
using PortalScrape.DataAccess;
using PortalScrape.DataAccess.Entities;
using LINQtoCSV;

namespace PortalScrape.Processing
{
    public class Export
    {
        public void Run()
        {
            Configure();

            using (var session = NHibernateHelper.OpenSession())
            {
                var cc = new CsvContext();

                var articles = session.Query<Article>();
                var flatArticles = articles.Select(a => Mapper.Map<FlatArticle>(a));
                cc.Write(flatArticles, @"F:\PortalScrapeService\Data\articles.csv");

                var commments = session.Query<Comment>();
                var flatComments = commments.Select(c => Mapper.Map<FlatComment>(c));
                cc.Write(flatComments, @"F:\PortalScrapeService\Data\comments.csv");
            }
        }

        public static void Configure()
        {
            Mapper.Reset();
            Mapper.Initialize(x =>
            {
                x.AddProfile<ExportMappingProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }

        private class FlatArticle
        {
            public virtual Portal Portal { get; set; }
            public virtual string ExternalId { get; set; }
            public virtual DateTime DateScraped { get; set; }

            public virtual string Url { get; set; }
            public virtual int CommentCount { get; set; }
            public virtual DateTime? DatePublished { get; set; }
            public virtual DateTime? DateModified { get; set; }

            public virtual string Title { get; set; }
            public virtual string AuthorName { get; set; }
            public virtual string Body { get; set; }

            public virtual string Tags { get; set; }
            public virtual string Keywords { get; set; }
            public virtual string RelatedArticles { get; set; }
        }

        private class FlatComment
        {
            public virtual Portal Portal { get; set; }
            public virtual string ExternalId { get; set; }

            public virtual DateTime DateScraped { get; set; }

            public virtual string ArticleExternalId { get; set; }
            public virtual DateTime DateCreated { get; set; }
            public virtual string IpAddress { get; set; }

            public virtual string UserName { get; set; }
            public virtual string CommentText { get; set; }
            public virtual int InResponseToCommentId { get; set; }
            public virtual int Upvotes { get; set; }
            public virtual int DownVotes { get; set; }
        }

        private class ExportMappingProfile : Profile
        {
            protected override void Configure()
            {
                CreateMap<Article, FlatArticle>()
                    .ForMember(dest => dest.Portal, opt => opt.MapFrom(src => src.Id.Portal))
                    .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id.ExternalId));

                CreateMap<Comment, FlatComment>()
                    .ForMember(dest => dest.Portal, opt => opt.MapFrom(src => src.Id.Portal))
                    .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id.ExternalId));
            }
        }
    }
}