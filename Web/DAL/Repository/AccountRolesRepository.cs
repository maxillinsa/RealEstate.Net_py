

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;
using Web.DAL.IRepository;
using System.Data;

namespace Web.DAL.Repository
{
    public class AccountRolesRepository : IAccountRolesRepository, IDisposable
    {

        private db_bdsEntities _data;
        public AccountRolesRepository()
        {
            this._data = new db_bdsEntities();
        }
        public IList<AccountRole> GetAll ()
        {
            return _data.AccountRoles.ToList();
        }
        public IList<AccountRole> GetAll(bool isDelete)
        {
            return _data.AccountRoles.Where(x => x.IsDelete == isDelete).ToList();
        }
        public bool CheckExit(AccountRole acc)
            {

                AccountRole m = new AccountRole();
                m = _data.AccountRoles.Where(x => x.AccountId == acc.AccountId && x.RoleId == acc.RoleId).FirstOrDefault();
                if (m != null)
                    return true;
                else
                    return false;
                
            }

        public List<string> GetRoleNameByAccountId(long accountId)
        {
            //_data.AccountRoles.Join().Where(x=> x.AccountId == accountId).Select(x=> x.ro)
            var Ar = _data.AccountRoles.Where(x => x.AccountId == accountId && x.IsDelete == false).ToList();

            //var rs = (from ar in _data.AccountRoles
            //          join r in _data.Roles
            //          on ar.RoleId equals r.RoleId
            //          where ar.IsDelete == false
            //          select new { r.RoleName }).ToList();
            List<string> kq = new List<string>();
            if(Ar != null)
            { 
                foreach (var item in Ar)
                {
                    kq.Add(item.Role.RoleName);
                }
            }
            return kq;
        }
        public bool Edit(AccountRole collection)
        {
            try
            {
                //_data.Entry(collection).State = EntityState.Modified;
                AccountRole my = new AccountRole();
                my = _data.AccountRoles.Where(x => x.AccountRoleId == collection.AccountRoleId).FirstOrDefault();
                if (collection.AccountId != null)
                {
                    my.AccountId = collection.AccountId;
                }


                if (collection.RoleId != null)
                {
                    my.RoleId = collection.RoleId;
                }
                if (collection.CreateDate != null)
                {
                    my.CreateDate = collection.CreateDate;
                }
                if (collection.IsDelete != null)
                {
                    my.IsDelete = collection.IsDelete;
                }
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public AccountRole GetById(long id)
        {
            AccountRole ac = new AccountRole();
            try
            {
               
                ac = _data.AccountRoles.Find(id);
                return ac;
            }
            catch
            {
                return ac;
            }
        }
        //Create Account
        public bool Create(AccountRole model)
        {
            try
            {
                AccountRole acc = new AccountRole();
                acc.CreateDate = DateTime.Now;
                acc.IsDelete = false;
                acc.RoleId = model.RoleId;
                acc.AccountId = model.AccountId;
                _data.AccountRoles.Add(acc);
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(long id, bool IsDelete)
        {
            try
            {
                AccountRole sv = _data.AccountRoles.Find(id);
                sv.IsDelete = IsDelete;
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region disposed
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _data.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}