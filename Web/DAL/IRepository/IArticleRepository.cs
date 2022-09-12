using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.IRepository
{
    public interface IArticleRepository
    {
        List<Article> TakeByCategoryId2(long? id, long? catType, int take);
        List<Article> TakeRecentTopBy(long id, int take);
        List<Article> TakeTopArticle(bool isDelete, int take);
        bool Edit(Article model);
        long Insert(Article model);
        List<Article> GetAll();
        Article GetById(long keyword);

        bool Delete(long id, bool IsDelete); 
        List<Article> GetAll(bool isDelete);

        List<Article> GetAllByCategoryId(long id, long catType);
        List<Article> TakeByCategoryId(long id, long catType, int take);
    }
}