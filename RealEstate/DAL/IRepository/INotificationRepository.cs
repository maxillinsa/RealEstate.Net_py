using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.DAL.IRepository
{
    public interface INotificationRepository
    {
        bool Edit(Notification model);
        long Insert(Notification model);
        List<Notification> GetAll();
        Notification GetById(long keyword);

        bool Delete(long id, bool isDelete);
         bool CheckExit(string Title);

   

    }
}