﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.WeiXin
{

    public class WeiXinDbInitializer : DropCreateDatabaseIfModelChanges<WeiXinDbContext>
    {
        protected override void Seed(WeiXinDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }


        public static void InitializeIdentityForEF(WeiXinDbContext db)
        {
            /*             
                drop table [dbo].[__MigrationHistory];
                drop table [dbo].[weixin_m_userbinds];
                drop table [dbo].[weixin_m_userinfos];
                drop table [dbo].[weixin_m_mpmenuinfos];
                drop table [dbo].[weixin_m_mps];
             */
        }
    }

}
