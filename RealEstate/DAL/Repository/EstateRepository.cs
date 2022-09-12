using AutoMapper;
using RealEstate.Common;
using RealEstate.DAL.IRepository;
using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.DAL.Repository
{
    public class EstateRepository : IEstateRepository, IDisposable
    {
        private PerfectRealDataContext _data;     

        CompanyViewModel config = (CompanyViewModel)HttpContext.Current.Cache.Get("MyConfig");
      

        public EstateRepository(PerfectRealDataContext dbContext)
        {
            this._data = dbContext;
        }

        public async Task<List<EstateViewModel>> GetEstatesAsync(EstateArguments estateArguments)
        {
            var dayAgo = DateTime.Now.AddDays(-(Convert.ToInt32(config.NumberOfExpirationDates)));
            var dayAgo2 = DateTime.Now.AddDays(-(Convert.ToInt32(config.NumberOfExpirationDates))).AddDays(-30);

            var expression = PredicateBuilder.True<Estate>();
            switch (estateArguments.SortId)
            {
                case 0:
                    expression = PredicateBuilder.And(expression, x => dayAgo <= x.UpdateDate && x.IsDelete == false);
                    break;
                case 1:
                    expression = PredicateBuilder.And(expression, x => dayAgo2 > x.UpdateDate );   // Sản phẩm hết hạn
                    break;
                case 2:
                    expression = PredicateBuilder.And(expression, x => dayAgo > x.UpdateDate);   // Sản phẩm hết hạn
                    break;
                case 3:
                    expression = PredicateBuilder.And(expression, x => x.IsDelete ==  true);   // Sản phẩm hết hạn
                    break;
                default:
                    break;
            }
            
            // expression = PredicateBuilder.And(expression, x => x.IsDelete != null && x.IsDelete == false);
            if(estateArguments.AccountId != 0)
            {
                expression = PredicateBuilder.And(expression, x => x.AccountId != null && x.AccountId == estateArguments.AccountId);
            }
            if (estateArguments.IsHot != false)
                expression = PredicateBuilder.And(expression, x => x.IsHot != null && x.IsHot == estateArguments.IsHot);
            if (estateArguments.isOutSide != false)
                expression = PredicateBuilder.And(expression, x => x.IsOutSide != null && x.IsOutSide == estateArguments.isOutSide);

            if (!string.IsNullOrEmpty(estateArguments.EstateCode))
                expression = PredicateBuilder.And(expression, x => ((x.Code ?? "").Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ", "").Contains(estateArguments.EstateCode.Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ",""))));
            if (!string.IsNullOrEmpty(estateArguments.houseNumber))
                expression = PredicateBuilder.And(expression, x => ((x.HouseNo ?? "").Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ", "").Contains(estateArguments.houseNumber.Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ", ""))));

            if (estateArguments.Estate_TypeId.HasValue && estateArguments.Estate_TypeId != 0)
                expression = PredicateBuilder.And(expression, x => x.Estate_TypeId == estateArguments.Estate_TypeId.Value);
            if (estateArguments.Estate_GroupId.HasValue && estateArguments.Estate_GroupId != 0)
                expression = PredicateBuilder.And(expression, x => x.Estate_GroupId == estateArguments.Estate_GroupId.Value);
            if (estateArguments.TownId.HasValue && estateArguments.TownId != 0)
                expression = PredicateBuilder.And(expression, x => x.TownId == estateArguments.TownId.Value);
            if (estateArguments.Estate_ProjectId.HasValue && estateArguments.Estate_ProjectId != 0)
                expression = PredicateBuilder.And(expression, x => x.Estate_ProjectId == estateArguments.Estate_ProjectId.Value);
            if (estateArguments.Estate_StatusId.HasValue && estateArguments.Estate_StatusId != 0)
                expression = PredicateBuilder.And(expression, x => x.Estate_StatusId == estateArguments.Estate_StatusId.Value);
            if (estateArguments.Estate_StatusId.HasValue && estateArguments.Estate_StatusId != 0)
                expression = PredicateBuilder.And(expression, x => x.Estate_StatusId == estateArguments.Estate_StatusId.Value);
            if (estateArguments.Number_Paper != 0)
                expression = PredicateBuilder.And(expression, x => x.Number_Paper == estateArguments.Number_Paper);
            if (estateArguments.Number_Lot != 0)
                expression = PredicateBuilder.And(expression, x => x.Number_Lot == estateArguments.Number_Lot);
            if (estateArguments.PriceRange.From > 0)
            {
                expression = PredicateBuilder.And(expression, x => x.SalePrice >= estateArguments.PriceRange.From && x.SalePrice <= estateArguments.PriceRange.To);
                expression = PredicateBuilder.And(expression, x => x.RentPrice >= estateArguments.PriceRange.From && x.RentPrice <= estateArguments.PriceRange.To);
            }
            if (estateArguments.AreaRange.From > 0)
                expression = PredicateBuilder.And(expression, x => x.LandArea >= estateArguments.AreaRange.From && x.LandArea <= estateArguments.AreaRange.To);

            var model = await _data.Estates.Where(expression).OrderByDescending(x => x.UpdateDate).Include(x=> x.Province).Include(x => x.Ward).Include(x => x.District).Select(x => new EstateViewModel
            {
                LotId = x.LotId,
                Account = x.Account,
                UpdateDate = x.UpdateDate,
                AccountId = x.AccountId,
                AlleyOfHouse = x.AlleyOfHouse,
                AreaOffUse = x.AreaOffUse,
                SaleRent = x.SaleRent,
                Certificate_TypeId = x.Certificate_TypeId,
                Comments = x.Comments,
                ConstructionArea = x.ConstructionArea,               
                SaleUnit = x.SaleUnit,
                SaleUnitId = x.SaleUnitId,
                RentUnit = x.RentUnit,
                RentUnitId = x.RentUnitId,
                DriectionOfTheHouse = x.DriectionOfTheHouse,
                Estate_Address = x.Estate_Address,              
                Estate_GroupId = x.Estate_GroupId,
                Estate_Groups = x.Estate_Groups,
                Estate_Images = x.Estate_Images,
                Estate_ProjectId = x.Estate_ProjectId,
                Estate_Projects = x.Estate_Projects,
                Estate_Statuses = x.Estate_Statuses,
                Estate_StatusId = x.Estate_StatusId,
                Estate_TypeId = x.Estate_TypeId,
                Estate_Types = x.Estate_Types,
                ExpandAfter = x.ExpandAfter,
                Frontispiece = x.Frontispiece,
                FrontOfHouse = x.FrontOfHouse,
                FullDescription = x.FullDescription,
                Note = x.Note,
                SalePrice = x.SalePrice,
                RentPrice = x.RentPrice,
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                IsDelete = x.IsDelete,
                IsHot = x.IsHot,
                Town = x.Town,
                TownId = x.TownId,
                Code = x.Code,
                LandArea = x.LandArea,
                Latitude = x.Latitude,
                Link360 = x.Link360,
                LinkVideoYoutube = x.LinkVideoYoutube,
                LogBy = x.LogBy,
                Longitude = x.Longitude,
                LotLength = x.LotLength,
                Map = x.Map,
                CreateDate = x.CreateDate,
                Number_Bedrooms = x.Number_Bedrooms,
                Number_Floors = x.Number_Floors,
                Number_Livingrooms = x.Number_Livingrooms,
                Number_Lot = x.Number_Lot,
                Number_Paper = x.Number_Paper,
                Number_Toilets = x.Number_Toilets,
                ShotDescription = x.ShotDescription,
                Mobile = x.Mobile,
                HouseNo = x.HouseNo,
                OwnerName = x.OwnerName,
                Views = x.Views,
                WardId = x.WardId, 
                Ward = x.Ward,
                District = x.District,
                Province = x.Province,
                IsOutSide = x.IsOutSide
            }).ToListAsync();
            return model;
        }
        
        public bool Edit(Estate model, bool hasChanged3D = false)
        {
            try
            {
                Estate estate = _data.Estates.Where(n => n.Id == model.Id).FirstOrDefault();
                if (hasChanged3D)
                {
                    if (!string.IsNullOrEmpty(estate.Active360Folders))
                    {
                        var folders = estate.Active360Folders.Split(new char[] { '.' });
                        if (folders != null)
                        {
                            foreach (var item in folders)
                            {
                                var folder = System.Web.HttpContext.Current.Server.MapPath("~") + item;
                                if (Directory.Exists(folder))
                                {
                                    try
                                    {
                                        Directory.Delete(folder, true);
                                    }
                                    catch(Exception ex)
                                    {

                                    }
                                    
                                }
                            }
                        }
                    }
                    estate.Active360Folders = model.Active360Folders;
                }
                estate.Estate_Images = model.Estate_Images;
                if (model.AccountId != null)
                    estate.AccountId = model.AccountId;
                if (model.Estate_TypeId != null)
                    estate.Estate_TypeId = model.Estate_TypeId;
                if (model.SaleUnitId != null)
                    estate.SaleUnitId = model.SaleUnitId;
                if (model.RentUnitId != null)
                    estate.RentUnitId = model.RentUnitId;
                if (model.TownId != null)
                    estate.TownId = model.TownId;               
                if (model.SalePrice != null)
                    estate.SalePrice = model.SalePrice;
                if (model.RentPrice != null)
                    estate.RentPrice = model.RentPrice;
                if (model.Code != null)
                    estate.Code = model.Code;
                if (model.LotId != null)
                    estate.LotId = model.LotId;
                if (model.CreateDate != null)
                    estate.CreateDate = model.CreateDate;
                if (model.UpdateDate != null)
                    estate.UpdateDate = model.UpdateDate;
                if (model.Mobile != null)
                    estate.Mobile = model.Mobile;
                if (model.HouseNo != null)
                    estate.HouseNo = model.HouseNo;
                if (model.OwnerName != null)
                    estate.OwnerName = model.OwnerName;
                if (model.Note != null)
                    estate.Note = model.Note;
                if (model.IsDelete != null)
                    estate.IsDelete = model.IsDelete;
                if (model.LogBy != null)
                    estate.LogBy = model.LogBy;
                if (model.Estate_ProjectId != null)
                    estate.Estate_ProjectId = model.Estate_ProjectId;
                if (model.Estate_StatusId != null)
                    estate.Estate_StatusId = model.Estate_StatusId;
                if (model.Estate_TypeId != null)
                    estate.Estate_TypeId = model.Estate_TypeId;
                if (model.Estate_GroupId != null)
                    estate.Estate_GroupId = model.Estate_GroupId;
                if (model.Certificate_TypeId != null)
                    estate.Certificate_TypeId = model.Certificate_TypeId;


                if (model.AreaOffUse != null)
                    estate.AreaOffUse = model.AreaOffUse;
                if (model.AlleyOfHouse != null)
                    estate.AlleyOfHouse = model.AlleyOfHouse;
                if (model.ConstructionArea != null)
                    estate.ConstructionArea = model.ConstructionArea;
                if (model.DriectionOfTheHouse != null)
                    estate.DriectionOfTheHouse = model.DriectionOfTheHouse;
                if (model.Estate_Address != null)
                    estate.Estate_Address = model.Estate_Address;
                if (model.ExpandAfter != null)
                    estate.ExpandAfter = model.ExpandAfter;
                if (model.Frontispiece != null)
                    estate.Frontispiece = model.Frontispiece;
                if (model.FrontOfHouse != null)
                    estate.FrontOfHouse = model.FrontOfHouse;


                if (model.FullDescription != null)
                    estate.FullDescription = model.FullDescription;
                if (model.ShotDescription != null)
                    estate.ShotDescription = model.ShotDescription;
                if (model.Number_Paper != null)
                    estate.Number_Paper = model.Number_Paper;
                if (model.Number_Lot != null)
                    estate.Number_Lot = model.Number_Lot;
                if (model.Number_Livingrooms != null)
                    estate.Number_Livingrooms = model.Number_Livingrooms;

                if (model.Number_Floors != null)
                    estate.Number_Floors = model.Number_Floors;
                if (model.Number_Bedrooms != null)
                    estate.Number_Bedrooms = model.Number_Bedrooms;


                if (model.LandArea != null)
                    estate.LandArea = model.LandArea;
                if (model.Latitude != null)
                    estate.Latitude = model.Latitude;
                if (model.Link360 != null)
                    estate.Link360 = model.Link360;
                if (model.Longitude != null)
                    estate.Longitude = model.Longitude;


                if (model.SaleRent != null)
                {
                    estate.SaleRent = model.SaleRent;
                }
                if (model.IsHot != null)
                    estate.IsHot = model.IsHot;
                if (model.IsOutSide != null)
                    estate.IsOutSide = model.IsOutSide;
                estate.UpdateDate = DateTime.UtcNow;
                _data.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

       
        // Check Exit 2: Kiểm tra sản phẩm trùng với sản phẩm có trong kho nhưng chưa bị xóa.  
        //- Nếu chung khu phố mà trùng ký hiệu và số nhà thì không cho nhập.
        //- Còn trùng tên, trùng khu phố nhưng sản phẩm đã quá hạn thì không báo.
        // Return False thì cho nhập, Return true thì không cho nhập
        //public bool CheckExit2(Estate model)
        //{
        //    var flag = false;
        //    List<Estate> lst = new List<Estate>();
        //    if (!string.IsNullOrEmpty(model.HouseNo))
        //    {
        //        // lấy sản phẩm đã tồn tại trùng số nhà, chưa bị xóa, và trùng khu phố
        //        lst = _data.Estates.Where(x => x.HouseNo != null && x.HouseNo.Replace(".","-") == model.HouseNo.Replace(".", "-") && x.IsDelete == false && x.TownId == model.TownId).ToList();
        //    }
        //    if (lst.Count == 0)
        //    {
        //        if (!string.IsNullOrEmpty(model.Code))
        //        {
        //            // lấy sản phẩm đã tồn tại trùng ký hiệu, chưa bị xóa, và trùng khu phố
        //            lst = _data.Estates.Where(x =>x.Code != null &&  x.Code.Replace(".", "-") == model.Code.Replace(".", "-") && x.IsDelete == false && x.TownId == model.TownId).ToList();
                    
        //        }
        //    }         
        //    if (lst.Count != 0)
        //    {
        //        DateTime today = DateTime.Now;
        //        foreach (Estate m in lst)
        //        {
        //            if(m.UpdateDate != null)
        //            {
        //                TimeSpan ngay = today.Subtract(Convert.ToDateTime(m.UpdateDate));
        //                // Nếu sản phẩm trong kho quá 90 ngày thì không kiểm tra trùng
        //                if (ngay.Days > 90)
        //                {
        //                    Debug.WriteLine(m);
        //                    lst = lst.Where(x=> x.Id != m.Id).ToList();
        //                }
        //            }                                   
        //        }                
        //    }
        //    if (lst.Count != 0)
        //    {
        //        foreach (var m in lst)
        //        {            
        //            // nếu trong kho giá bán = 0 còn thì cho phép nhập giá bán != 0
        //            if (m.SalePrice == 0 && model.SalePrice != 0)
        //            {
        //                flag = false;
        //                return flag;
        //            }
        //            // nếu trong kho giá thuê = 0 còn thì cho phép nhập giá thuê
        //            if (m.RentPrice == 0 && model.RentPrice != 0)
        //            {
        //                flag =  false;
        //                return flag;
        //            }
        //        }
        //        flag = true;
        //    }
        //    return flag;
        //}

        public bool IsEstateExisting(Estate model)
        {
            var ninetyDayAgos = DateTime.Now.AddDays(-90);
            var expression = PredicateBuilder.True<Estate>();
            if (model.Id > 0)
                expression = PredicateBuilder.And(expression, x => x.Id != model.Id);

            if (model.Estate_ProjectId.HasValue && model.Estate_ProjectId.Value > 0)
            {
                expression = PredicateBuilder.And(expression, x => x.TownId == model.TownId && x.IsDelete == false && x.UpdateDate >= ninetyDayAgos
                    && ((x.RentPrice > 0 && model.RentPrice > 0) || (x.SalePrice > 0 && model.SalePrice > 0)));
                var products = _data.Estates.Where(expression).ToList();
                if (products != null)
                {
                    var isInValidProduct = products.Any(x => (model.Code ?? "").IsEqual(x.Code) || (model.HouseNo ?? "").IsEqual(x.HouseNo));
                    return isInValidProduct;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //san pham ngoai
                expression = PredicateBuilder.And(expression, x => x.DistrictId == model.DistrictId && x.IsDelete == false && x.Number_Paper == model.Number_Paper
                && x.Number_Lot == model.Number_Lot && x.UpdateDate >= ninetyDayAgos
                   && ((x.RentPrice > 0 && model.RentPrice > 0) || (x.SalePrice > 0 && model.SalePrice > 0)));
                return _data.Estates.Any(expression);
            }
        }
        // Kiểm tra khi chỉnh sửa sản phẩm trong hệ thống
        //public bool CheckExit3(Estate model)
        //{
        //    var flag = false;
        //    List<Estate> lst = new List<Estate>();
        //    if (!string.IsNullOrEmpty(model.HouseNo))
        //    {
        //        // lấy sản phẩm đã tồn tại trùng số nhà, chưa bị xóa, và trùng khu phố
        //        lst = _data.Estates.Where(x => x.HouseNo != null && x.HouseNo.Replace(".", "-") == model.HouseNo.Replace(".", "-") && x.IsDelete == false && x.TownId == model.TownId && x.Id != model.Id).ToList();
        //    }
        //    if (lst.Count == 0)
        //    {
        //        if (!string.IsNullOrEmpty(model.Code))
        //        {
        //            // lấy sản phẩm đã tồn tại trùng ký hiệu, chưa bị xóa, và trùng khu phố
        //            lst = _data.Estates.Where(x => x.Code != null && x.Code.Replace(".", "-") == model.Code.Replace(".", "-") && x.IsDelete == false && x.TownId == model.TownId && x.Id != model.Id).ToList();

        //        }
        //    }
        //    if (lst.Count != 0)
        //    {
        //        DateTime today = DateTime.Now;
        //        foreach (Estate m in lst)
        //        {
        //            if (m.UpdateDate != null)
        //            {
        //                TimeSpan ngay = today.Subtract(Convert.ToDateTime(m.UpdateDate));
        //                // Nếu sản phẩm trong kho quá endDate ngày thì không kiểm tra trùng
        //                if (ngay.Days > (Convert.ToInt32(config.NumberOfExpirationDates)))
        //                {
        //                    lst = lst.Where(x => x.Id != m.Id).ToList();
        //                }
        //            }
        //        }
        //    }
        //    if (lst.Count != 0)
        //    {
        //        foreach (var m in lst)
        //        {
        //            // nếu trong kho giá bán = 0 còn thì cho phép nhập giá bán != 0
        //            if (m.SalePrice == 0 && model.SalePrice != 0)
        //            {
        //                flag = false;
        //                return flag;
        //            }
        //            // nếu trong kho giá thuê = 0 còn thì cho phép nhập giá thuê
        //            if (m.RentPrice == 0 && model.RentPrice != 0)
        //            {
        //                flag = false;
        //                return flag;
        //            }
        //        }
        //        flag = true;
        //    }
        //    return flag;
        //}
      
        public EstateViewModel Create(Estate model)
        {
            try
            {
                model.CreateDate = DateTime.Now;
                model.UpdateDate = DateTime.Now;
                if (!IsEstateExisting(model))
                {
                    _data.Estates.Add(model);
                    _data.SaveChanges();
                    return Mapper.Map<EstateViewModel>(model);
                }
                return null;
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
      
        public List<Estate> GetAllByAccountID(long accountId)
        {
            List<Estate> lstEstate = new List<Estate>();
            lstEstate = _data.Estates.Where(x => x.AccountId == accountId).ToList();
            return lstEstate;
        }

        private void ConvertBackPrice(Estate estateViewModel)
        {
            var saleUnitId = estateViewModel.SaleUnitId;
            var rentUnitId = estateViewModel.RentUnitId;
            var salePrice = estateViewModel.SalePrice;
            var rentPrice = estateViewModel.RentPrice;
            if (salePrice > 0)
            {
                switch (saleUnitId)
                {
                    case 1:
                        break;
                    case 2:
                        estateViewModel.SalePrice /= 1000000;
                        break;
                    case 3:
                        estateViewModel.SalePrice /= 1000000000;
                        break;
                    case 4:
                        estateViewModel.SalePrice /= 100000 * ((estateViewModel.LandArea.HasValue && estateViewModel.LandArea.Value > 0) ? estateViewModel.LandArea.Value : 1);
                        break;
                    case 5:
                        estateViewModel.SalePrice /= 1000000 * ((estateViewModel.LandArea.HasValue && estateViewModel.LandArea.Value > 0) ? estateViewModel.LandArea.Value : 1);
                        break;
                    case 6: //USD
                        break;
                    default:
                        break;
                }
            }
            if (rentPrice > 0)
            {
                switch (rentUnitId)
                {
                    case 1:
                        break;
                    case 2:
                        estateViewModel.RentPrice /= 100000;
                        break;
                    case 3:
                        estateViewModel.RentPrice /= 1000000;
                        break;
                    case 4:
                        estateViewModel.RentPrice /= 100000 * ((estateViewModel.LandArea.HasValue && estateViewModel.LandArea.Value > 0) ? estateViewModel.LandArea.Value : 1);
                        break;
                    case 5:
                        estateViewModel.RentPrice /= 1000000 * ((estateViewModel.LandArea.HasValue && estateViewModel.LandArea.Value > 0) ? estateViewModel.LandArea.Value : 1);
                        break;
                    case 6:
                        estateViewModel.RentPrice /= 1000 * ((estateViewModel.LandArea.HasValue && estateViewModel.LandArea.Value > 0) ? estateViewModel.LandArea.Value : 1);
                        break;
                    default:
                        break;
                }
            }
        }

        public List<Estate> GetAllThisDay(bool isDelete)
        {
            DateTime ngay = DateTime.Now;
            List<Estate> lstEstate = new List<Estate>();
            lstEstate = _data.Estates.Where(x => x.IsDelete == isDelete && x.CreateDate.Value.Day == ngay.Day && x.CreateDate.Value.Month == ngay.Month && x.CreateDate.Value.Year == ngay.Year)
                .OrderByDescending(x => x.CreateDate).Skip(0).Take(20).ToList();
            foreach(var item in lstEstate)
            {
                ConvertBackPrice(item);
            }
            return lstEstate;
        }

        public List<Estate> GetAll()
        {
            return _data.Estates.Where(x => x.IsDelete == false).ToList();
        }
        public int  CountTotalEstates()
        {
            return _data.Estates.Count(x => x.IsDelete == false);
        }
        public int CountTotalEstatesHot()
        {
            return _data.Estates.Count(x => x.IsDelete == false && x.IsHot == true);
        }
        public Estate GetById(long keyword)
        {
            try
            {
                return _data.Estates.Where(n => n.Id == keyword).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public EstateDetailViewModel GetEstateDetailViewById (long id)
        {
            try
            {
                var model = _data.Estates.Select(x => new EstateDetailViewModel
                {
                    LotId = x.LotId,
                    Account = x.Account,
                    UpdateDate = x.UpdateDate,
                    AccountId = x.AccountId,
                    AlleyOfHouse = x.AlleyOfHouse,
                    AreaOffUse = x.AreaOffUse,
                    SaleRent = x.SaleRent,
                    Certificate_TypeId = x.Certificate_TypeId,
                    Comments = x.Comments,
                    ConstructionArea = x.ConstructionArea,               
                    SaleUnit = x.SaleUnit,
                    SaleUnitId = x.SaleUnitId,
                    RentUnit = x.RentUnit,
                    RentUnitId = x.RentUnitId,
                    DriectionOfTheHouse = x.DriectionOfTheHouse,
                    Estate_Address = x.Estate_Address,                   
                    Estate_GroupId = x.Estate_GroupId,
                    Estate_Groups = x.Estate_Groups,                  
                    Estate_ProjectId = x.Estate_ProjectId,
                    Estate_Projects = x.Estate_Projects,
                    Estate_Statuses = x.Estate_Statuses,
                    Estate_StatusId = x.Estate_StatusId,
                    Estate_TypeId = x.Estate_TypeId,
                    Estate_Types = x.Estate_Types,
                    ExpandAfter = x.ExpandAfter,
                    Frontispiece = x.Frontispiece,
                    FrontOfHouse = x.FrontOfHouse,
                    FullDescription = x.FullDescription,
                    Note = x.Note,
                    SalePrice = x.SalePrice,
                    RentPrice = x.RentPrice,
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    IsDelete = x.IsDelete,
                    IsHot = x.IsHot,
                    Town = x.Town,
                    TownId = x.TownId,
                    Code = x.Code,
                    LandArea = x.LandArea,
                    Latitude = x.Latitude,
                    Link360 = x.Link360,
                    LinkVideoYoutube = x.LinkVideoYoutube,
                    LogBy = x.LogBy,
                    Longitude = x.Longitude,
                    LotLength = x.LotLength,
                    Map = x.Map,
                    CreateDate = x.CreateDate,
                    Number_Bedrooms = x.Number_Bedrooms,
                    Number_Floors = x.Number_Floors,
                    Number_Livingrooms = x.Number_Livingrooms,
                    Number_Lot = x.Number_Lot,
                    Number_Paper = x.Number_Paper,
                    Number_Toilets = x.Number_Toilets,
                    ShotDescription = x.ShotDescription,
                    Mobile = x.Mobile,
                    HouseNo = x.HouseNo,
                    OwnerName = x.OwnerName,
                    Views = x.Views,
                    WardId = x.WardId
                }).Where(x=> x.Id == id).FirstOrDefault();


                return model;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public EstateViewModel GetBySetMapLocation(long keyword)
        {
            try
            {
                var model =  _data.Estates.Where(n => n.Id == keyword).Select(x=>new EstateViewModel {
                    Id = x.Id
                }).FirstOrDefault();
                return model;
            }
            catch
            {
                return null;
            }
        }
        public string DeleteMedia(int mediaId)
        {
            var media = _data.Estate_Images.FirstOrDefault(x => x.ItemId == mediaId);
            if (media == null)
                return null;
            media.IsDelete = true;
            media.Modified = DateTime.Now;
            _data.SaveChanges();
            return media.Url;
        }
        public bool Delete(long id, bool IsDelete)
        {
            try
            {
                Estate pd = _data.Estates.Find(id);
                pd.IsDelete = IsDelete;
                if (IsDelete == false)
                {
                    pd.UpdateDate = DateTime.Now;
                }
                _data.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateGhiChu(Estate model)
        {
            try
            {
                Estate pd = _data.Estates.Find(model.Id);
                pd.Note = model.Note;
                pd.UpdateDate = DateTime.Now;           
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
                Estate pd = _data.Estates.Find(id);
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