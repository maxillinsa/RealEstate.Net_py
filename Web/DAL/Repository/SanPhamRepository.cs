using Web.DAL.IRepository;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.DAL.Repository
{
    public class SanPhamRepository : ISanPhamRepository, IDisposable
    {
        private db_bdsEntities _data;
        public SanPhamRepository()
        {
            _data = new db_bdsEntities();
        }
        public bool Edit(SanPham model)
        {
            try
            {
                SanPham rs = _data.SanPhams.Where(n => n.Id == model.Id).FirstOrDefault();
                //if (model.AccountId != null)
                //    rs.AccountId = model.AccountId;
                if (model.DonViBanId != null)
                    rs.DonViBanId = model.DonViBanId;
                if (model.DonViThueId != null)
                    rs.DonViThueId = model.DonViThueId;
                if (model.KhuPhoId != null)
                    rs.KhuPhoId = model.KhuPhoId;
                if (model.DienTich != null)
                rs.DienTich = model.DienTich;
                if (model.GiaBan != null)
                rs.GiaBan = model.GiaBan;
                if (model.GiaThue != null)
                rs.GiaThue = model.GiaThue;
                if (model.KyHieu != null)
                rs.KyHieu = model.KyHieu;
                if (model.MaLo != null)
                rs.MaLo = model.MaLo;
                if(model.NgayNhap !=null)
                rs.NgayNhap = model.NgayNhap;
                if(model.NgaySua !=null)
                rs.NgaySua = model.NgaySua;
                if (model.SoDienThoai != null)
                rs.SoDienThoai = model.SoDienThoai;
                if (model.SoNha != null)
                rs.SoNha = model.SoNha;
                if (model.TenChuNha != null)
                rs.TenChuNha = model.TenChuNha;
                if (model.GhiChu != null)
                    rs.GhiChu = model.GhiChu;
                if(model.IsDelete != null)
                    rs.IsDelete = model.IsDelete;
                if(model.BanChoThue != null)
                {
                    rs.BanChoThue = model.BanChoThue;
                }
                if (model.IsHot != null)
                    rs.IsHot = model.IsHot;

                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool EditByUser(SanPham model)
        {
            try
            {
                SanPham rs = _data.SanPhams.Where(n => n.Id == model.Id).FirstOrDefault();
                //if (model.AccountId != null)
                //    rs.AccountId = model.AccountId;
                if (model.DonViBanId != null)
                    rs.DonViBanId = model.DonViBanId;
                if (model.DonViThueId != null)
                    rs.DonViThueId = model.DonViThueId;
                //if (model.KhuPhoId != null)
                    //rs.KhuPhoId = model.KhuPhoId;
                if (model.DienTich != null)
                    rs.DienTich = model.DienTich;
                if (model.GiaBan != null)
                    rs.GiaBan = model.GiaBan;
                if (model.GiaThue != null)
                    rs.GiaThue = model.GiaThue;
                //if (model.KyHieu != null)
                //    rs.KyHieu = model.KyHieu;
                if (model.MaLo != null)
                    rs.MaLo = model.MaLo;
                if (model.NgayNhap != null)
                    rs.NgayNhap = model.NgayNhap;
                if (model.NgaySua != null)
                    rs.NgaySua = model.NgaySua;
                if (model.SoDienThoai != null)
                    rs.SoDienThoai = model.SoDienThoai;
                if (model.SoNha != null)
                    rs.SoNha = model.SoNha;
                if (model.TenChuNha != null)
                    rs.TenChuNha = model.TenChuNha;
                if (model.GhiChu != null)
                    rs.GhiChu = model.GhiChu;
                if (model.IsDelete != null)
                    rs.IsDelete = model.IsDelete;
                if (model.BanChoThue != null)
                {
                    rs.BanChoThue = model.BanChoThue;
                }
                if (model.IsHot != null)
                    rs.IsHot = model.IsHot;

                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool CheckExit(string kyHieu)
        {
            SanPham m = null;
            m = _data.SanPhams.Where(x => x.KyHieu == kyHieu && x.IsDelete == false).FirstOrDefault();
            if (m != null)
            {
                return true;
            }
            else
                return false;
        }
        public long Insert(SanPham model)
        {
            try
            {
                model.NgayNhap = DateTime.Now;
                model.NgaySua = DateTime.Now;
                _data.SanPhams.Add(model);
                _data.SaveChanges();
                return model.Id;
            }
            catch
            {
                return -1;
            }
        }
        public List<SanPham> GetAll(bool isDelete)
        {
            List<SanPham> lstSanPham = new List<SanPham>();
            lstSanPham = _data.SanPhams.Where(x => x.IsDelete == isDelete).ToList();
            return lstSanPham;
        }
        public List<SanPham> LayDanhSachSPTheoKyHieu(string kyHieu)
        {
            List<SanPham> lstSanPham = new List<SanPham>();
            lstSanPham = _data.SanPhams.Where(x => x.KyHieu == kyHieu).ToList();
            return lstSanPham;
        }
        public List<SanPham> GetAllThisMonth(bool isDelete)
        {
            DateTime ngay = DateTime.Now;
            List<SanPham> lstSanPham = new List<SanPham>();
            lstSanPham = _data.SanPhams.Where(x => x.IsDelete == isDelete && x.NgayNhap.Value.Month == ngay.Month && x.NgayNhap.Value.Year == ngay.Year).ToList();
            return lstSanPham;
        }
        public List<SanPham> GetAllThisDay(bool isDelete)
        {
            DateTime ngay = DateTime.Now;
            List<SanPham> lstSanPham = new List<SanPham>();
            lstSanPham = _data.SanPhams.Where(x => x.IsDelete == isDelete && x.NgayNhap.Value.Day == ngay.Day && x.NgayNhap.Value.Month == ngay.Month && x.NgayNhap.Value.Year == ngay.Year).ToList();
            return lstSanPham;
        }

        public List<SanPham> GetAll()
        {
            return _data.SanPhams.ToList();
        }
        public List<SanPham> GetAllNewProduct()
        {
            return _data.SanPhams.ToList();
        }
        public SanPham GetById(long keyword)
        {
            try
            {
                return _data.SanPhams.Where(n => n.Id == keyword).FirstOrDefault();
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
                SanPham pd = _data.SanPhams.Find(id);
                pd.IsDelete = IsDelete;
                pd.NgaySua = DateTime.Now;
               
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateIsHot(long id, bool isHot)
        {
            try
            {
                SanPham pd = _data.SanPhams.Find(id);
                pd.IsHot = isHot;
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