using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private db_bdsEntities _data;
        public ProductRepository()
        {
            _data = new db_bdsEntities();
        }
      
        public List<Product> TakeOrtherTopByCateId(long id, int take)
        {
            return _data.Products.Where(x => x.IsDelete == false && x.CategoryId != id).OrderByDescending(x => x.CreatedDate).Take(take).ToList();
        }
        public List<Product> TakeRecentTopBy(long id, int take)
        {
            return _data.Products.Where(x => x.IsDelete == false && x.ProductId != id).OrderByDescending(x => x.CreatedDate).Take(take).ToList();
        }
        public List<Product> TakeByCategoryId2(long? id, long? catType, int take)
        {
            return _data.Products.Where(x => x.Category.CategoryTypeId == catType && x.CategoryId == id).OrderByDescending(x => x.CreatedDate).Take(take).ToList();
        }
        public List<Product> TakeTopProducts(bool isDelete, int take)
        {
            List<Product> lstProduct = new List<Product>();
            lstProduct = _data.Products.Where(x => x.IsDelete == isDelete).OrderByDescending(x=> x.CreatedDate).Take(take).ToList();
            return lstProduct;
        }
        public bool Edit(Product product)
        {
            try
            {
                Product rs = _data.Products.Where(n => n.ProductId == product.ProductId).FirstOrDefault();
                rs.ProductName = product.ProductName;
                //if (product.ProductDetailId != null)
                //    rs.ProductDetailId = product.ProductDetailId;
                if (product.CategoryId != null)
                rs.CategoryId = product.CategoryId;
                rs.VendorId = product.VendorId;
                if (product.ProductCode != null)
                rs.ProductCode = product.ProductCode;
                if (product.Image != null)
                rs.Image = product.Image;
                if (product.BasePrice != null)
                rs.BasePrice = product.BasePrice;
                if (product.SalePrice != null)
                rs.SalePrice = product.SalePrice;
                rs.Detail = product.Detail;
                if (product.VendorId != null)
                    rs.VendorId = product.VendorId;
                rs.CreatedDate = product.CreatedDate;
                rs.Description = product.Description;
                if (product.AlbumId != null)
                    rs.AlbumId = product.AlbumId;
                if (product.CreatedBy != null)
                    rs.CreatedBy = product.CreatedBy;
                if (product.ModifiedDate != null)
                    rs.ModifiedDate = product.ModifiedDate;

                rs.MetaKeywords = product.MetaKeywords;
                rs.MetaDescription = product.MetaDescription;
                if(product.MetaTitle != null)
                rs.MetaTitle = product.MetaTitle;
                else
                {
                    rs.MetaTitle = product.ProductName;
                }
                rs.Tags = product.Tags;
                rs.AllowComments = rs.AllowComments;
                if (product.IsDelete != null)
                    rs.IsDelete = product.IsDelete;

                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool CheckExit(string  productName)
        {
            Product m = null;
            m = _data.Products.Where(x => x.ProductName == productName).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(Product product)
        {
            try
            {
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                _data.Products.Add(product);
                _data.SaveChanges();
                return product.ProductId;
            }
            catch
            {
                return -1;
            }
        }
        public List<Product> GetAll(bool isDelete)
        {
            List<Product> lstProduct = new List<Product>();
            lstProduct = _data.Products.Where(x => x.IsDelete == isDelete).ToList();
            return lstProduct;
        }

        public List<Product> GetAll()
        {
            return _data.Products.ToList();
        }
        public List<Product> GetAllByCateId(long id)
        {
            return _data.Products.Where(x=> x.CategoryId == id).OrderByDescending(x=> x.CreatedDate).ToList();
        }
        public Product GetById(long keyword)
        {
            try
            {
                return _data.Products.Where(n => n.ProductId == keyword).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(long id, bool IsDelete)
        {
            try
            {
                Product pd = _data.Products.Find(id);
                pd.IsDelete = IsDelete;
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