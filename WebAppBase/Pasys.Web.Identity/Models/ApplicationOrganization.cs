using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Web.Identity.Models
{
    public enum OrganizationStatus
    {
        /// <summary>
        /// 初始
        /// </summary>
        Init,
        /// <summary>
        /// 审核中
        /// </summary>
        Checking,
        /// <summary>
        /// 运营中
        /// </summary>
        Open,
        /// <summary>
        /// 已关闭
        /// </summary>
        Closed
    }


    public class ApplicationOrganization
    {
        public string OrganizationId { get; set; }
        public string ParentOrganizationId { get; set; }
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 利用状态
        /// </summary>
        public int Status { get; set; }
        public string OfficeName { get; set; }
        public string StaffName { get; set; }
        public string Postal { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string MailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LastUserID { get; set; }
        public DateTime LastUpdatetime { get; set; }

        public virtual List<ApplicationUser> Users { get; set; }
        public virtual List<ApplicationRole> Roles { get; set; }

    }

    public interface IOrganizationStore : IDisposable
    {
        bool Create(ApplicationOrganization organization);
        bool Update(ApplicationOrganization organization);
        bool Delete(string organizationID);
    }

    public class OrganizationStore : IOrganizationStore,IDisposable
    {
        private bool _disposed;
        public DbContext Context { get; private set; }
        private DbSet<ApplicationOrganization> _roleStore;

        public OrganizationStore(DbContext context)
        {
            this.Context = context;
            _roleStore= context.Set<ApplicationOrganization>();
        }

        public bool Create(ApplicationOrganization organization)
        {
            if (string.IsNullOrEmpty(organization.OrganizationId))
            {
                throw new ArgumentException();
            }

            var org= _roleStore.Find(organization.OrganizationId);
            if (org == null)
            {
                _roleStore.Add(organization);
                Context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Update(ApplicationOrganization organization)
        {
            if (string.IsNullOrEmpty(organization.OrganizationId))
            {
                throw new ArgumentException();
            }

            var org= _roleStore.Find(organization.OrganizationId);
            if (org==null)
            {
                throw new ArgumentException();
            }

            org.OrganizationCode = organization.OrganizationCode;
            org.OfficeName = organization.OfficeName;
            org.StaffName = organization.StaffName;
            org.Postal = organization.Postal;
            org.Address1 = organization.Address1;
            org.Address2 = organization.Address2;
            org.MailAddress = organization.MailAddress;
            org.PhoneNumber = organization.PhoneNumber;
            org.FaxNumber = organization.FaxNumber;
            org.StartDate = organization.StartDate;
            org.EndDate = organization.EndDate;
            org.LastUserID = organization.LastUserID;
            org.LastUpdatetime = DateTime.Now;

            Context.Entry(org).State = EntityState.Modified;
            Context.SaveChanges();          
            return true;
        }

        public bool Delete(string organizationID)
        {
            if (string.IsNullOrEmpty(organizationID))
            {
                throw new ArgumentException();
            }

            var org = _roleStore.Find(organizationID);
            if (org == null)
            {
                throw new ArgumentException();
            }

            Context.Entry(org).State = EntityState.Deleted;
            Context.SaveChanges();
            return true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// If disposing, calls dispose on the Context.  Always nulls out the Context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && Context != null)
            {
                Context.Dispose();
            }
            _disposed = true;
            Context = null;
            _roleStore = null;
        }



    }

    public class OrganizationManager : IDisposable
    {
        private bool _disposed;
        protected IOrganizationStore Store { get; private set; }
        public OrganizationManager(IOrganizationStore store)
        {
            this.Store = store;
        }

        public static OrganizationManager Create(IdentityFactoryOptions<OrganizationManager> options, IOwinContext context)
        {
            return new OrganizationManager(new OrganizationStore(context.Get<AppIdentityDbContext>()));
        }

        public bool Create(ApplicationOrganization organization)
        {
            return this.Store.Create(organization);
        }
        public bool Update(ApplicationOrganization organization)
        {
            return this.Store.Update(organization);
        }
        public bool Delete(string organizationID)
        {
            return this.Store.Delete(organizationID);
        }


        /// <summary>
        ///     Dispose this object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Store.Dispose();
            }
            _disposed = true;
        }
    }


}
