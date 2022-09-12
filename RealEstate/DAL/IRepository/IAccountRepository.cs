using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Models;
using RealEstate.Models.ViewModels;

namespace RealEstate.DAL.IRepository
{
    public interface IAccountRepository
    {
        bool ListingTransferAccountToAccount(int idChuyen, int idNhan);
        IList<Account> GetAll();
        bool CheckExit(Account acc);
        IList<Account> GetAll(bool iDelete);

        bool Edit(Account collection);
        bool Create(Account model);
        Account GetById(long id);
        bool Delete(long id, bool isDelete);
        LoginModel GetLoginByEmail(string Email);
        bool ChangePassword(Account acc);
        bool UpdateLastSignIn(long id);
        ProfileViewModel GetProfileViewById(long id);
    }
}
