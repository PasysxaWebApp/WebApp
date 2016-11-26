using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{
    public class WeiXinDbContext : DbContext
    {
        public WeiXinDbContext()
            : base("DefaultConnection")
        {
        }

        DbSet<MP> MPs { get; set; }
        DbSet<MPMenuInfo> MPMenuInfos { get; set; }
        DbSet<UserBind> UserBinders { get; set; }
        DbSet<WeiXinUserInfo> UserInfos { get; set; }

        DbSet<ApplicationRequestMessageBase> AllRequestMessages { get; set; }
        DbSet<ApplicationRequestMessageImage> RequestMessageImage { get; set; }
        DbSet<ApplicationRequestMessageLink> RequestMessageLink { get; set; }
        DbSet<ApplicationRequestMessageLocation> RequestMessageLocation { get; set; }
        DbSet<ApplicationRequestMessageShortVideo> RequestMessageShortVideo { get; set; }
        DbSet<ApplicationRequestMessageText> RequestMessageText { get; set; }
        DbSet<ApplicationRequestMessageVideo> RequestMessageVideo { get; set; }
        DbSet<ApplicationRequestMessageVoice> RequestMessageVoice { get; set; }


        DbSet<ApplicationArticle> Article { get; set; }
        DbSet<ApplicationCustomerServiceAccount> CustomerServiceAccount { get; set; }
        DbSet<ApplicationResponseMessageBase> AllResponseMessages { get; set; }
        DbSet<ApplicationResponseMessageImage> ResponseMessageImage { get; set; }
        DbSet<ApplicationResponseMessageMusic> ResponseMessageMusic { get; set; }
        DbSet<ApplicationResponseMessageNews> ResponseMessageNews { get; set; }
        DbSet<ApplicationResponseMessageText> ResponseMessageText { get; set; }
        DbSet<ApplicationResponseMessageTransferCustomerService> ResponseMessageTransferCustomerService { get; set; }
        DbSet<ApplicationResponseMessageVideo> ResponseMessageVideo { get; set; }
        DbSet<ApplicationResponseMessageVoice> ResponseMessageVoice { get; set; }
        

        public static void CreateForce()
        {
            var db = new WeiXinDbContext();
            db.Database.Initialize(false);
            WeiXinDbInitializer.InitializeIdentityForEF(db);
        }


        public static WeiXinDbContext Create()
        {
            // 在第一次启动网站时初始化数据库            
            //Database.SetInitializer<MemberCardDbContext>(new MemberCardDbInitializer());
            return new WeiXinDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            var membercard = modelBuilder.Entity<MP>()
            .HasKey(m => m.MPId)
            .Ignore(m => m.EntityName)
            .ToTable("weixin_m_mps");
            //membercard.Property(m => m.MemberCardId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);


            var consumption = modelBuilder.Entity<MPMenuInfo>()
            .HasKey(m => m.MenuId)
            .Ignore(m => m.EntityName)
            .ToTable("weixin_m_mpmenuinfos");
            //consumption.Property(m => m.ConsumptionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            var userinfo = modelBuilder.Entity<WeiXinUserInfo>()
            .HasKey(m => m.OpenId)
            .Ignore(m => m.EntityName)            
            .ToTable("weixin_m_userinfos");

            var userbind = modelBuilder.Entity<UserBind>()
            .HasKey(m => m.UserBinderId)            
            .Ignore(m => m.EntityName)
            .ToTable("weixin_m_userbinds");
            //1：1，并且级联删除
            //userbind.HasRequired(m => m.WxUser).WithOptional().WillCascadeOnDelete();
            #region Reqeuest
            var requestMessageBase = modelBuilder.Entity<ApplicationRequestMessageBase>()
            .HasKey(m => m.MsgId)            
            .Ignore(m => m.EntityName)
            .ToTable("weixin_t_request_logs");

            var requestMessageImage = modelBuilder.Entity<ApplicationRequestMessageImage>()
            .HasKey(m => m.MsgId)
            .ToTable("weixin_t_request_logs");
            requestMessageImage.Property(t => t.MediaId).HasColumnName("MediaId");

            var requestMessageLink = modelBuilder.Entity<ApplicationRequestMessageLink>()
            .HasKey(m => m.MsgId)
            .ToTable("weixin_t_request_logs");

            var requestMessageLocation = modelBuilder.Entity<ApplicationRequestMessageLocation>()
            .HasKey(m => m.MsgId)
            .ToTable("weixin_t_request_logs");

            var requestMessageShortVideo = modelBuilder.Entity<ApplicationRequestMessageShortVideo>()
            .HasKey(m => m.MsgId)
            .ToTable("weixin_t_request_logs");
            requestMessageShortVideo.Property(t => t.MediaId).HasColumnName("MediaId");
            requestMessageShortVideo.Property(t => t.ThumbMediaId).HasColumnName("ThumbMediaId");
            
            var requestMessageText = modelBuilder.Entity<ApplicationRequestMessageText>()
            .HasKey(m => m.MsgId)
            .ToTable("weixin_t_request_logs");
            requestMessageText.Property(t => t.Content).HasColumnName("TextContent");

            var requestMessageVideo = modelBuilder.Entity<ApplicationRequestMessageVideo>()
            .HasKey(m => m.MsgId)
            .ToTable("weixin_t_request_logs");
            requestMessageVideo.Property(t => t.MediaId).HasColumnName("MediaId");
            requestMessageVideo.Property(t => t.ThumbMediaId).HasColumnName("ThumbMediaId");

            var requestMessageVoice = modelBuilder.Entity<ApplicationRequestMessageVoice>()
            .HasKey(m => m.MsgId)
            .ToTable("weixin_t_request_logs");
            requestMessageVoice.Property(t => t.MediaId).HasColumnName("MediaId");
            #endregion

            #region Response

            modelBuilder.ComplexType<ApplicationImage>();
            modelBuilder.ComplexType<ApplicationMusic>();
            modelBuilder.ComplexType<ApplicationVideo>();
            modelBuilder.ComplexType<ApplicationVoice>();

            var responseMessageBase = modelBuilder.Entity<ApplicationResponseMessageBase>()
            .HasKey(m => m.ResponseId)
            .Ignore(m => m.EntityName)
            .ToTable("weixin_t_responses");

            var responseMessageImage = modelBuilder.Entity<ApplicationResponseMessageImage>()
            .HasKey(m => m.ResponseId)
            .ToTable("weixin_t_responses");
            responseMessageImage.Property(t => t.Image.MediaId).HasColumnName("MediaId");

            var responseMessageMusic = modelBuilder.Entity<ApplicationResponseMessageMusic>()
            .HasKey(m => m.ResponseId)
            .ToTable("weixin_t_responses");
            responseMessageMusic.Property(t => t.Music.Title).HasColumnName("Title");
            responseMessageMusic.Property(t => t.Music.Description).HasColumnName("Description");
            responseMessageMusic.Property(t => t.Music.MusicUrl).HasColumnName("MusicUrl");
            responseMessageMusic.Property(t => t.Music.HQMusicUrl).HasColumnName("HQMusicUrl");
            responseMessageMusic.Property(t => t.Music.ThumbMediaId).HasColumnName("ThumbMediaId");

            var responseMessageNews = modelBuilder.Entity<ApplicationResponseMessageNews>()
            .HasKey(m => m.ResponseId)
            .Ignore(m => m.ArticleCount)
            .ToTable("weixin_t_responses");

            var responseMessageText = modelBuilder.Entity<ApplicationResponseMessageText>()
            .HasKey(m => m.ResponseId)
            .ToTable("weixin_t_responses");
            responseMessageText.Property(t => t.Content).HasColumnName("TextContent");

            var responseMessageTransfer_Customer_Service = modelBuilder.Entity<ApplicationResponseMessageTransferCustomerService>()
            .HasKey(m => m.ResponseId)
            .ToTable("weixin_t_responses");

            var responseMessageVideo = modelBuilder.Entity<ApplicationResponseMessageVideo>()
            .HasKey(m => m.ResponseId)
            .ToTable("weixin_t_responses");
            responseMessageVideo.Property(t => t.Video.Title).HasColumnName("Title");
            responseMessageVideo.Property(t => t.Video.MediaId).HasColumnName("MediaId");
            responseMessageVideo.Property(t => t.Video.Description).HasColumnName("Description");

            var responseMessageVoice = modelBuilder.Entity<ApplicationResponseMessageVoice>()
            .HasKey(m => m.ResponseId)
            .ToTable("weixin_t_responses");
            responseMessageVoice.Property(t => t.Voice.MediaId).HasColumnName("MediaId");

            //========================================================================
            var responseArticle = modelBuilder.Entity<ApplicationArticle>()
            .HasKey(m => m.ArticleId)
            .ToTable("weixin_t_response_articles");

            var responseCustomerServiceAccount = modelBuilder.Entity<ApplicationCustomerServiceAccount>()
            .HasKey(m => m.CustomerServiceAccountId)
            .ToTable("weixin_t_response_customerserviceaccounts");
            //========================================================================

            #endregion

        }
    }

}
