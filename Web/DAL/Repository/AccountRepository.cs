
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;
using Web.DAL.IRepository;
using Web.Common;

namespace Web.DAL.Repository
{
    public class AccountRepository : IAccountRepository , IDisposable
    {

        private db_bdsEntities _data;
        public AccountRepository()
        {
            this._data = new db_bdsEntities();
        }

        //Get All Account
        public IList<Account> GetAll()
        {
            return _data.Accounts.ToList();
        }
        public IList<Account> GetAll(bool iDelete)
        {
            return _data.Accounts.Where(x=> x.IsDelete == iDelete).ToList();
        }
        ////Get Information Account By Id
        public Account GetById(long id)
        {
            return _data.Accounts.Where(x => x.AccountId == id).FirstOrDefault();
        }

        //Edit Account
        public bool Edit(Account collection)
        {
            try
            {
                Account my = new Account();
                my = _data.Accounts.Where(x => x.AccountId == collection.AccountId).FirstOrDefault();
                if (collection.DepartmentId != null)
                {
                    my.DepartmentId = collection.DepartmentId;
                }
            
                my.Email = collection.Email;
                if (collection.Password != null)
                {
                    my.Password = collection.Password;
                }
                my.FirstName = collection.FirstName;
                my.LastName = collection.LastName;
                
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(long id, bool isDelete)
        {
            try
            {
                Account my = new Account();
                my = _data.Accounts.Where(x => x.AccountId == id).FirstOrDefault();

                my.IsDelete = isDelete;
               
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Create Account
        public bool Create(Account model)
        {
            try
            {
                _data.Accounts.Add(model);
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        ////Edit password
        //public string EditPassword(AccountComplex1 model)
        //{
        //    try
        //    {
        //        var query = _data.Accounts.Where(k => k.Password.Equals(model.Password) && k.AccountId.Equals(model.AccountId)).FirstOrDefault();

        //        if (query != null)
        //        {
        //            Account acc = _data.Accounts.Where(u => u.AccountId == query.AccountId).SingleOrDefault();
        //            acc.Password = model.NewPassword;
        //            _data.SubmitChanges();

        //            return model.NewPassword;
        //        }
        //        else
        //        {
        //            return model.Password;
        //        }
        //    }
        //    catch
        //    {
        //        return model.Password;
        //    }
        //}

        ////Get information account By Email
        public LoginModel GetLoginByEmail(string Email)
        {
            LoginModel lg = new LoginModel();


            //var query = (from src in _data.Accounts
            //             where src.Email == Email
            //             select new LoginModel
            //             {
            //                 AccountId = src.AccountId,
            //                 Email = src.Email,
            //                 Password = src.Password,
            //                 FirstName = src.FirstName,
            //                 LastName = src.LastName
            //             }
            //                 ).FirstOrDefault();
            //lg = query;

            Account acc = new Account();
            acc = _data.Accounts.Where(x => x.Email == Email && x.IsDelete == false).FirstOrDefault();
            if(acc != null)
            {
                  lg.AccountId = acc.AccountId;
                             lg.Email = acc.Email;
                             lg.Password = acc.Password;
                             lg.FirstName = acc.FirstName;
                             lg.LastName = acc.LastName;
            }

            return lg;
        }
        public bool ChangePassword(Account acc)
        {
            try
            {
                Account oldAcc = new Account();
                oldAcc = _data.Accounts.Where(x => x.AccountId == acc.AccountId).FirstOrDefault();
                oldAcc.Password = acc.Password;
                _data.SaveChanges();
                return true;
            }
            catch
            { return false; }
        }

        public bool CheckExit(Account acc)
        {
            Account m = new Account();
            m = _data.Accounts.Where(x => x.Email == acc.Email).FirstOrDefault();
            if (m != null)
            {
                if(m.Email == acc.Email)
                {
                    return false;
                }
                return true;
            }
            else
                return false;
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