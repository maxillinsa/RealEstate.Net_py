using AutoMapper;
using CustomRoles;
using MvcPaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealEstate.Common;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using RealEstate.Models;
using RealEstate.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace RealEstate.Controllers
{
   
    [CustomAuthorize(Roles = "SuperAdmin,Admin,Staff")]
    public class EstatesController : Controller
    {
        #region declare
        private static CompanyViewModel config = (CompanyViewModel)System.Web.HttpContext.Current.Cache.Get("MyConfig");
        private int pageSize = Convert.ToInt32(config.DefaulPageSize);
        private IEstateRepository _IEstateRepository;
        private IEstate_GroupRepository _realestateGroupRepository;
        private IEstate_ProjectRepository _realestateProjectRepository;
        private IEstate_StatusRepository _realestateStatusRepository;
        private ITownRepository _ITownRepository;
        private ISaleUnitRepository _ISaleUnitRepository;
        private IRentUnitRepository _IRentUnitRepository;
        private IAccountRepository _accountRepository;
        private ICommentRepository _commentRepository;
        private IEstate_TypesRepository _Estate_TypesRepository;
        private IEstate_GroupRepository _estate_GroupRepository;
        private IEstate_StatusRepository _estate_StatusRepository;
        private IProvinceRepository _provinceRepository;
        private IStreetRepository _streetRepository;
        private IDistrictRepository _districtRepository;
        private IWardRepository _wardRepository;
        private ICertificate_TypeRepository _certificate_TypeRepository;
        private IEstate_ImageRepository _estate_ImageRepository;
        private IEstate_InvestorRepository _estate_InvestorRepository;
        public EstatesController(IEstateRepository _IEstateRepository,
       IEstate_GroupRepository _realestateGroupRepository,
        IEstate_ProjectRepository _realestateProjectRepository,
         IEstate_StatusRepository _realestateStatusRepository,
         ITownRepository _ITownRepository,
         ISaleUnitRepository _ISaleUnitRepository,
         IRentUnitRepository _IRentUnitRepository,
         IAccountRepository _accountRepository,
         ICommentRepository _commentRepository,
         IEstate_TypesRepository _Estate_TypesRepository,
         IEstate_GroupRepository _estate_GroupRepository,
         IEstate_StatusRepository _estate_StatusRepository,
         IProvinceRepository _provinceRepository,
         IDistrictRepository _districtRepository,
         IWardRepository _wardRepository,
         ICertificate_TypeRepository _certificate_TypeRepository,
         IEstate_InvestorRepository estate_InvestorRepository,
         IStreetRepository streetRepository,
        IEstate_ImageRepository estate_ImageRepository)
        {
            this._ITownRepository = _ITownRepository;
            this._ISaleUnitRepository = _ISaleUnitRepository;
            this._IRentUnitRepository = _IRentUnitRepository;
            this._IEstateRepository = _IEstateRepository;
            this._commentRepository = _commentRepository;
            this._realestateGroupRepository = _realestateGroupRepository;
            this._realestateProjectRepository = _realestateProjectRepository;
            this._realestateStatusRepository = _realestateStatusRepository;
            this._accountRepository = _accountRepository;
            this._Estate_TypesRepository = _Estate_TypesRepository;
            this._estate_GroupRepository = _estate_GroupRepository;
            this._estate_StatusRepository = _estate_StatusRepository;
            this._provinceRepository = _provinceRepository;
            this._districtRepository = _districtRepository;
            this._wardRepository = _wardRepository;
            this._certificate_TypeRepository = _certificate_TypeRepository;
            _estate_ImageRepository = estate_ImageRepository;
            _estate_InvestorRepository = estate_InvestorRepository;
            _streetRepository = streetRepository;
        }

        private string ImageUploadsFolder
        {
            get
            {
                return @"/Media/Images/Estate/";
            }
        }
        public string[] SupportedExtensions = new[] { ".png", ".jpeg", ".jpg", ".bmp" };
        #endregion

        #region Estate

        public ActionResult CreateEstate()
        {
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(manv);
            ViewBag.manv = manv;
            ViewBag.currentAccountId = currentAccount;
            loadData();
            return PartialView();
        }

        [HttpPost]
        public JsonResult DeleteMedia(int mediaId)
        {
            string fileUrl = _IEstateRepository.DeleteMedia(mediaId);
            if (!string.IsNullOrEmpty(fileUrl))
            {
                string fullFilePath = Server.MapPath("~") + fileUrl;
                if (System.IO.File.Exists(fullFilePath))
                    System.IO.File.Delete(fullFilePath);
            }
            return Json(new { message = "OK" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult CreateEstate(EstateViewModel model)
        {
            var dataModels = Generate3DDataModel();
            if (dataModels != null && dataModels.Any())
            {
                dataModels = GenerateCubicImage(dataModels);
                string link360 = Generate360ViewJsonData(dataModels);
                model.Link360 = link360;
                model.Active360Folders = string.Join(".", dataModels.Select(x => x.BasePath).ToList());
            }

            model.IsDelete = false;
            model.Code = model.Code?.Replace(" ", "");
            model.Code = model.Code?.Replace("_", "");
            model.LandArea = model.LandArea ?? 0;
            if (_IEstateRepository.IsEstateExisting(model))
            {
                model.Id = 0;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var estate = Mapper.Map<Estate>(model);
                SetMedias(estate);

                var product = _IEstateRepository.Create(estate);
                if (product != null)
                {
                    return Json(product, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        private Estate SetMedias(Estate estate)
        {
            if (Request.Files.AllKeys.Any())
            {
                if (!Directory.Exists(ImageUploadsFolder))
                    Directory.CreateDirectory(Server.MapPath("~") + ImageUploadsFolder);

                HttpFileCollection httpFileCollection = System.Web.HttpContext.Current.Request.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    HttpPostedFile httpPostedFile = httpFileCollection[i];
                    string fileName = Guid.NewGuid().ToString();
                    string finalName = fileName + Path.GetExtension(httpPostedFile.FileName);

                    if (!SupportedExtensions.Any(x => x.Equals(Path.GetExtension(httpPostedFile.FileName), StringComparison.CurrentCultureIgnoreCase)))
                        continue;

                    int type = GetImageUploadType(Request.Files.AllKeys[i]);
                    if (type == 3)
                        continue;

                    string fileUrl = ImageUploadsFolder + finalName;
                    httpPostedFile.SaveAs(Server.MapPath(fileUrl));

                    estate.Estate_Images.Add(new Estate_Images
                    {
                        Type = type,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Title = finalName,
                        Url = fileUrl,
                        IsDelete = false,
                        MetaKeywords = ""
                    });
                }
            }
            return estate;
        }
        private List<ThreeSixtyViewJsonDataModel> Generate3DDataModel()
        {
            if (Request.Files.AllKeys.Any())
            {
                if (!Directory.Exists(ImageUploadsFolder))
                    Directory.CreateDirectory(Server.MapPath("~") + ImageUploadsFolder);

                List<ThreeSixtyViewJsonDataModel> threeSixtyViewJsonDataModels = new List<ThreeSixtyViewJsonDataModel>();
                HttpFileCollection httpFileCollection = System.Web.HttpContext.Current.Request.Files;
                for (int i = 0; i < httpFileCollection.Count; i++)
                {
                    //TODO: check if image is 360 photo

                    HttpPostedFile httpPostedFile = httpFileCollection[i];
                    int type = GetImageUploadType(Request.Files.AllKeys[i]);
                    if (type == 3)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        string finalName = fileName + Path.GetExtension(httpPostedFile.FileName);

                        string fileUrl = ImageUploadsFolder + finalName;
                        string tempFile = Server.MapPath("~") + fileUrl;
                        httpPostedFile.SaveAs(tempFile);

                        string folder = Guid.NewGuid().ToString();
                        threeSixtyViewJsonDataModels.Add(new ThreeSixtyViewJsonDataModel
                        {
                            OriginalFileName = httpPostedFile.FileName.Substring(0, httpPostedFile.FileName.LastIndexOf(".")),
                            PanoramaUrl = ImageUploadsFolder + finalName,
                            InputFile = tempFile,
                            BasePath = ImageUploadsFolder + folder,
                            OutputFolder = Server.MapPath("~") + ImageUploadsFolder + folder,
                            SceneId = $"Scene{i}"
                        });
                    }
                }
                return threeSixtyViewJsonDataModels;
            }
            return null;
        }

        public List<ThreeSixtyViewJsonDataModel> GenerateCubicImage(List<ThreeSixtyViewJsonDataModel> threeSixtyViewJsonDataModels)
        {
            string basePath = ConfigurationManager.AppSettings["PythonPath"];
            var requiredExternalLibs = Convert.ToBoolean(ConfigurationManager.AppSettings["RequiredExternalLibs"] ?? "True");
            if (!requiredExternalLibs)
                basePath = Server.MapPath("~") + @"/Libs/Python37-32";
            foreach (var item in threeSixtyViewJsonDataModels)
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.WorkingDirectory = basePath;
                start.FileName = $@"{basePath}\python.exe";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.Arguments = $@"generate.py -n {basePath}\bin\nona.exe -o {item.OutputFolder} {item.InputFile}";

                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string stderr = process.StandardError.ReadToEnd();
                        string result = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(result))
                        {
                            try
                            {
                                ThreeSixtyViewJsonDataModel cubicInfo = JsonConvert.DeserializeObject<ThreeSixtyViewJsonDataModel>(result);
                                item.CubeResolution = cubicInfo.CubeResolution;
                                item.MaxLevel = cubicInfo.MaxLevel;
                                item.Extension = cubicInfo.Extension;
                                item.TileResolution = cubicInfo.TileResolution;

                                if (System.IO.File.Exists(item.InputFile))
                                    System.IO.File.Delete(item.InputFile);
                            }
                            catch(Exception ex)
                            {
                                if (Directory.Exists(item.OutputFolder))
                                    Directory.Delete(item.OutputFolder, true);
                            }
                        }
                    }
                }
            }
            return threeSixtyViewJsonDataModels;
        }
        private string Generate360ViewJsonData(List<ThreeSixtyViewJsonDataModel> threeSixtyViewJsonDataModels)
        {
            var jsonObject = new JObject();
            dynamic defaultObject = new JObject();
            defaultObject.firstScene = threeSixtyViewJsonDataModels.FirstOrDefault()?.SceneId;
            defaultObject.author = "Demo";
            defaultObject.sceneFadeDuration = "1000";
            jsonObject.Add("default", defaultObject);
            var data = threeSixtyViewJsonDataModels.Select(x => GenerateProperty(threeSixtyViewJsonDataModels, x));

            jsonObject.Add(new JProperty("scenes", new JObject(from src in data select src)));
            return jsonObject.ToString();
        }
        private JProperty GenerateProperty(List<ThreeSixtyViewJsonDataModel> allScenes, ThreeSixtyViewJsonDataModel model)
        {
            JObject scene = new JObject();
            if (model.IsMultiResType)
            {
                scene.Add(new JProperty("hfov", 110));
                scene.Add(new JProperty("pitch", 0));
                scene.Add(new JProperty("yaw", 0));
                scene.Add(new JProperty("type", "multires"));
                scene.Add(GenerateMultiResProperty(model));
                scene.Add(GenerateHotspotProperty(allScenes, model));
            }
            else
            {
                scene.Add(new JProperty("title", model.Title));
                scene.Add(new JProperty("hfov", 110));
                scene.Add(new JProperty("pitch", 0));
                scene.Add(new JProperty("jaw", 0));
                scene.Add(new JProperty("type", "equirectangular"));
                scene.Add(new JProperty("panorama", model.PanoramaUrl));
                scene.Add(GenerateHotspotProperty(allScenes, model));
            }

            var firstProperty = new JProperty(model.SceneId, scene);
            return firstProperty;
        }
        private JProperty GenerateHotspotProperty(List<ThreeSixtyViewJsonDataModel> allScenes, ThreeSixtyViewJsonDataModel model)
        {
            var hotspots = new JArray();
            if (allScenes.Count == 1)
                return new JProperty("hotSpots", hotspots);


            List<ThreeSixtyViewJsonDataModel> validScenes = GetSceneForHotSpot(allScenes, model);


            for (int i = 0; i < validScenes.Count; i++)
            {
                var selectScene = validScenes.ElementAt(i);

                JObject hotSpotItem = new JObject();
                hotSpotItem.Add(new JProperty("type", "scene"));
                hotSpotItem.Add(new JProperty("pitch", 0));
                hotSpotItem.Add(new JProperty("yaw", i * 180));
                hotSpotItem.Add(new JProperty("sceneId", selectScene.SceneId));
                hotSpotItem.Add(new JProperty("text", selectScene.Title));

                if (selectScene.IsMultiResType)
                {
                    hotSpotItem.Add(GenerateMultiResProperty(selectScene));
                }
                else
                {
                    hotSpotItem.Add(new JProperty("panorama", selectScene.PanoramaUrl));
                }
                hotspots.Add(hotSpotItem);
            }
            return new JProperty("hotSpots", hotspots);
        }
        private List<ThreeSixtyViewJsonDataModel> GetSceneForHotSpot(List<ThreeSixtyViewJsonDataModel> allScenes, ThreeSixtyViewJsonDataModel model)
        {
            int currentItemIndex = allScenes.IndexOf(model);
            if (currentItemIndex == 0)
            {
                var scenes = new List<ThreeSixtyViewJsonDataModel> { allScenes.ElementAt(1) };
                if (allScenes.Count > 2)
                {
                    scenes.Add(allScenes.LastOrDefault());
                }
                return scenes;
            }
            else if (currentItemIndex == allScenes.Count - 1)
            {
                var scenes = new List<ThreeSixtyViewJsonDataModel> { allScenes.ElementAt(0) };
                if (allScenes.Count > 2)
                {
                    scenes.Add(allScenes.ElementAt(allScenes.Count - 2));
                }
                return scenes;
            }
            else
            {
                return new List<ThreeSixtyViewJsonDataModel> { allScenes.ElementAt(currentItemIndex - 1), allScenes.ElementAt(currentItemIndex + 1) };
            }
        }
        private JProperty GenerateMultiResProperty(ThreeSixtyViewJsonDataModel model)
        {
            dynamic multiRes = new JObject();
            multiRes.basePath = model.BasePath;
            multiRes.path = "/%l/%s%y_%x";
            multiRes.fallbackPath = "/fallback/%s";
            multiRes.extension = model.Extension;
            multiRes.tileResolution = model.TileResolution;
            multiRes.maxLevel = model.MaxLevel;
            multiRes.cubeResolution = model.CubeResolution;

            return new JProperty("multiRes", multiRes);
        }
        private int GetImageUploadType(string image)
        {
            int type = 1;
            string firstTwoCharacters = image.Substring(0, 2);
            switch (firstTwoCharacters)
            {
                case "2D":
                    type = 2; //2d images
                    break;
                case "3D":
                    type = 3; //3d images
                    break;
                case "CE":
                    type = 4; // certificate images
                    break;
                case "NO":
                    type = 1; // other images
                    break;
                default:
                    type = 1;
                    break;
            }
            return type;
        }
        public ActionResult EditEstate(int id)
        {
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(manv);
            ViewBag.manv = manv;
            ViewBag.currentAccountId = currentAccount;
            loadData();
            Estate m = new Estate();
            m = _IEstateRepository.GetById(id);
            if (m == null)
            {
                return HttpNotFound();
            }
            EstateViewModel estateViewModel = Mapper.Map<EstateViewModel>(m);
            ConvertBackPrice(estateViewModel);
            return PartialView(estateViewModel);
        }
        private void ConvertBackPrice(EstateViewModel estateViewModel)
        {
            var saleUnitId = estateViewModel.SaleUnitId;
            var rentUnitId = estateViewModel.RentUnitId;
            var salePrice = estateViewModel.SalePrice;
            var rentPrice = estateViewModel.RentPrice;
            estateViewModel.InvestorId = estateViewModel.Estate_Projects != null ? estateViewModel.Estate_Projects.Estate_InvestorId : default(long?);
            estateViewModel.FinalSalePrice = salePrice;
            estateViewModel.FinalRentPrice = rentPrice;
            if (salePrice > 0)
            {
                switch(saleUnitId)
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
            if(rentPrice > 0)
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
                    case 7:
                        var exchangeRateInUSD = config != null ? config.ExchageRateUSD : 1;
                        var usd = estateViewModel.RentPrice / exchangeRateInUSD;
                        estateViewModel.RentPrice = usd;
                        break;
                    default:
                        break;
                }
            }
        }

        [HttpPost]
        public JsonResult EditEstate(EstateViewModel model)
        {
            Estate my = new Estate();
            model.Code = model.Code?.Replace(" ", "");
            model.Code = model.Code?.Replace("_", "");
            model.LandArea = model.LandArea ?? 0;
            my = _IEstateRepository.GetById(model.Id);

            if (_IEstateRepository.IsEstateExisting(model))
            {
                _IEstateRepository.UpdateGhiChu(model);
                model.Id = 0;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            bool hasChanged3D = false;
            var dataModels = Generate3DDataModel();
            if (dataModels != null && dataModels.Any())
            {
                dataModels = GenerateCubicImage(dataModels);
                string link360 = Generate360ViewJsonData(dataModels);
                model.Link360 = link360;
                model.Active360Folders = string.Join(".", dataModels.Select(x => x.BasePath).ToList());
                hasChanged3D = true;
            }
            var estate = Mapper.Map<Estate>(model);
            SetMedias(estate);
           
            if (_IEstateRepository.Edit(estate, hasChanged3D))
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region Detail
        public ActionResult Detail(long id)
        {
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            ViewBag.AccountId = manv;


            EstateDetailViewModel model = new EstateDetailViewModel();


            model = _IEstateRepository.GetEstateDetailViewById(id);
            var myAccountId = Convert.ToInt32(model.AccountId);
            model.TotalEstates = _IEstateRepository.GetAllByAccountID(myAccountId).Count;

            model.Estate_Images = _estate_ImageRepository.GetAllByEstateId(id,false);

            model.lstComments = _commentRepository.GetByEstateId(id);
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Detail(long EstateId, string content)
        {
            Comment cm = new Comment();
            cm.CreateDate = DateTime.Now;

            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());

            cm.CreateById = manv;
            cm.Contents = content;
            cm.IsDelete = false;
            cm.EstateId = EstateId;
            _commentRepository.Insert(cm);
            return RedirectToAction("Detail", new { id = EstateId });
        }
        #endregion

        public async Task<ActionResult> Index(
            int? page, 
            string kyHieu,
            string houseNumber,
            int khuPho = 0,
            int Estate_TypeId = 0,
            int typePriceList = 0,
            int Estate_GroupId = 0,
            int acreage = 0,
            bool IsHot = false,
            bool isOutSide = false,
            int Estate_ProjectId = 0,
            int Estate_StatusId = 0,         
            int Number_Paper = 0,
            int Number_Lot =0, 
            int accountId =0,
            int SortId = 0)
        {
            ViewBag.Code = null;
            if (HttpContext.Session["bds_Acc_id"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            long manv = Convert.ToInt64(HttpContext.Session["bds_Acc_id"].ToString());
            Account currentAccount = new Account();
            currentAccount = _accountRepository.GetById(manv);
            var myAccountId = 0;
            if (accountId !=0 && accountId == manv)
            {
               myAccountId = accountId;
            }
            ViewBag.manv = manv;
            int pageNum = (page ?? 1);
            ViewBag.page = pageNum;
            ViewBag.currentAccountId = currentAccount;
            EstateArguments estateArguments = new EstateArguments
            {
                EstateCode = kyHieu,
                TownId = khuPho,
                PriceRange = Functions.rangePriceDtoConvert(typePriceList),
                AreaRange = Functions.rangeAreaDtoConvert(acreage),
                Estate_TypeId = Estate_TypeId,
                Estate_GroupId = Estate_GroupId,
                IsHot = IsHot,
                Estate_ProjectId = Estate_ProjectId,
                Estate_StatusId = Estate_StatusId,
                isOutSide = isOutSide,
                Number_Lot = Number_Lot,
                Number_Paper = Number_Paper,
                AccountId = myAccountId,
                SortId = SortId
            };
            ViewData["kyHieu"] = kyHieu;
            ViewData["khuPho"] = khuPho;
            ViewData["Estate_TypeId"] = Estate_TypeId;
            ViewData["loaiMucGia"] = typePriceList;
            ViewData["Estate_GroupId"] = Estate_GroupId;
            ViewData["acreage"] = acreage;
            ViewData["IsHot"] = IsHot;
            ViewData["isOutSide"] = isOutSide;
            ViewData["Estate_ProjectId"] = Estate_ProjectId;
            ViewData["Estate_StatusId"] = Estate_StatusId;
            ViewData["Number_Paper"] = Number_Paper;
            ViewData["Number_Lot"] = Number_Lot;
            ViewData["houseNumber"] = houseNumber;
            ViewData["accountId"] = myAccountId;
            ViewData["SortId"] = SortId;
            loadDropdownList();
            var ListEstate = await _IEstateRepository.GetEstatesAsync(estateArguments);
            ConvertBackPrice(ListEstate);
            //if (currentAccount.DepartmentId == 1)
            //{
            //    ListEstate = _IEstateRepository.GetAll().Distinct().OrderByDescending(x => x.UpdateDate).ToList();
            //}
            // Đối với tài khoản thường thì sản phẩm quá endDate ngày ko cập nhật dù là thuê hay bán là ẩn       
            //if (currentAccount.DepartmentId != 1)
            //{
            //    foreach (var item in ListEstate)
            //    {
            //        DateTime today = DateTime.Now;
            //        TimeSpan ngay = today.Subtract(Convert.ToDateTime(item.UpdateDate));
            //        if (ngay.Days >= endDate)
            //        {
            //            ListEstate = ListEstate.Where(x => x.Id != item.Id).ToList();
            //        }
            //    }
            //}
            if (Request.IsAjaxRequest())
                return PartialView("AjaxEstates", ListEstate.ToPagedList(pageNum, pageSize));
            return View(ListEstate.ToPagedList(pageNum, pageSize));
        }
        private void ConvertBackPrice(List<EstateViewModel> estateViewModels)
        {
            foreach(var item in estateViewModels)
            {
                ConvertBackPrice(item);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult XoaEstate(long productId)
        {
            string message = string.Empty;

            try
            {

                _IEstateRepository.Delete(productId, true);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult PhucHoiEstate(long productId)
        {
            string message = string.Empty;

            try
            {
                var model = _IEstateRepository.GetById(productId);
                if (_IEstateRepository.IsEstateExisting(model))
                {
                    return null;
                }
                _IEstateRepository.Delete(productId, false);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
       
        #region Helper
        private void loadData()
        {
            ViewBag.Groups = new SelectList(_realestateGroupRepository.GetAll(false).OrderBy(x => x.Name).ToList(), "ItemId", "Name", null);
            ViewBag.Projects = new SelectList(_realestateProjectRepository.GetAll(false).OrderBy(x => x.Name).ToList(), "ItemId", "Name", null);
            ViewBag.Statuses = new SelectList(_realestateStatusRepository.GetAll(false).OrderBy(x => x.Name).ToList(), "ItemId", "Name", null);
            ViewBag.Khupho = new SelectList(_ITownRepository.GetAll(false).OrderBy(x => x.Name).ToList(), "TownId", "Name", null);
            ViewBag.Donviban = new SelectList(_ISaleUnitRepository.GetAll(false), "SaleUnitId", "Name", null);
            ViewBag.Donvithue = new SelectList(_IRentUnitRepository.GetAll(false), "RentUnitId", "Name", null);
            ViewBag.Estate_Types = new SelectList(_Estate_TypesRepository.GetAll(), "ItemId", "Name", null);
            ViewBag.Provinces = new SelectList(_provinceRepository.GetAll(false).OrderBy(x => x.Name), "ItemId", "Name", null);
            ViewBag.Districts = new SelectList(_districtRepository.GetAll(false).OrderBy(x=> x.Name), "ItemId", "Name", null);
            ViewBag.Streets = new SelectList(_streetRepository.GetAll(false).OrderBy(x => x.Name), "StreetId", "Name", null);
            ViewBag.Wards = new SelectList(_wardRepository.GetAll(false).OrderBy(x => x.Name), "ItemId", "Name", null);
            ViewBag.Directions = new SelectList(Settings.HouseDirections, "Id", "Name", null);
            ViewBag.Certificates = new SelectList(_certificate_TypeRepository.GetAll(false), "ItemId", "Name", null);
            ViewBag.Investors = new SelectList(_estate_InvestorRepository.GetAll(false), "ItemId", "Name", null);
        }
        private void loadDropdownList()
        {

            List<Town> khuPhoList = new List<Town>();
            khuPhoList = _ITownRepository.GetAll();
            ViewBag.Khupho = khuPhoList.Distinct().OrderBy(x => x.Name).ToList();

            List<Estate_Types> Estate_TypesList = new List<Estate_Types>();
            Estate_TypesList = _Estate_TypesRepository.GetAll();
            ViewBag.Estate_Types = Estate_TypesList.Distinct().OrderBy(x => x.Name).ToList();

            List<Estate_GroupViewModel> Estate_GroupsList = new List<Estate_GroupViewModel>();
            Estate_GroupsList = _estate_GroupRepository.GetAll(false);
            ViewBag.Estate_Groups = Estate_GroupsList.Distinct().OrderBy(x => x.Name).ToList();

            List<Estate_ProjectViewModel> Estate_ProjectsList = new List<Estate_ProjectViewModel>();
            Estate_ProjectsList = _realestateProjectRepository.GetAll(false);
            ViewBag.Estate_Projects = Estate_ProjectsList.Distinct().OrderBy(x => x.Name).ToList();

            List<Estate_StatusViewModel> Estate_StatusesList = new List<Estate_StatusViewModel>();
            Estate_StatusesList = _estate_StatusRepository.GetAll(false);
            ViewBag.Estate_Statuses = Estate_StatusesList.Distinct().OrderBy(x => x.Name).ToList();
        }
        private string CreateNewName(string str)
        {
            return DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString() + str;
        }

        [HttpPost]
        public ActionResult GetAllProjectsByInvestor(long investorId)
        {
            var data = _realestateProjectRepository.GetAllProjectsByInvestor(investorId, false);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAllTownsByProject(long projectId)
        {
            var data = _ITownRepository.GetAllTownsByProject(projectId, false);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
