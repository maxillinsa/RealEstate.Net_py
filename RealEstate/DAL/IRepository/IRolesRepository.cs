using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Models;

namespace RealEstate.DAL.IRepository
{
    public interface IRolesRepository
    {
        IList<Role> GetAll();
         bool Edit(Role role);
         IList<Role> GetAll(bool isDelete);
         long Insert(Role role);
         Role GetById(long keyword);
    }
}
