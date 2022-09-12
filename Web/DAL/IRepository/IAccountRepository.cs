using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.DAL.IRepository
{
    public interface IAccountRepository
    {

        IList<Account> GetAll();
        bool CheckExit(Account acc);
        IList<Account> GetAll(bool iDelete);

        bool Edit(Account collection);
        bool Create(Account model);
        Account GetById(long id);
        bool Delete(long id, bool isDelete);
        LoginModel GetLoginByEmail(string Email);
        bool ChangePassword(Account acc);
    }
}
