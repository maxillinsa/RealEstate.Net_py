using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.DAL.IRepository
{
    public interface IAccountRolesRepository
    {
        IList<AccountRole> GetAll();
        IList<AccountRole> GetAll(bool isDelete);
        List<string> GetRoleNameByAccountId(long accountId);
        bool Create(AccountRole model);
        bool Edit(AccountRole collection);
        AccountRole GetById(long id);
        bool Delete(long id, bool IsDelete);
        bool CheckExit(AccountRole acc);
    }
}
