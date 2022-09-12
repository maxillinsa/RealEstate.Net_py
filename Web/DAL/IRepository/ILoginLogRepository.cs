using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.DAL.IRepository
{
    public interface ILoginLogRepository
    {
        IList<LoginLog> GetAll();
        bool Insert(LoginLog LoginLog);
        LoginLog GetById(long keyword);
        IList<LoginLog> GetAllByAccId(long AccountId);
    }
}
