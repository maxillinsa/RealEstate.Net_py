using CustomRoles;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using RealEstate.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Text.RegularExpressions;

namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "SuperAdmin,Admin,Staff")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerCategoriesRepository _customerCategoriesRepository;
        private const int pageSize = 500;
        //
        // GET: /Customer/
        public CustomerController(ICustomerRepository customerRepository, ICustomerCategoriesRepository _customerCategoriesRepository)
        {
            this._customerRepository = customerRepository;
            this._customerCategoriesRepository = _customerCategoriesRepository;
        }
        public ActionResult Index(int? page, string customerMobile, string customerName, int CustomerCategoryId = 0)
        {
            LoadData();
            ViewBag.customerName = customerName;
            int pageNum = (page ?? 1);
            ViewBag.page = pageNum;
            ViewBag.CustomerCategoryId = CustomerCategoryId;
            var lst = _customerRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(customerMobile))
            {
                ViewBag.customerMobile = customerMobile.ToLower();
                string customerMobile2 = customerMobile.Replace(".", "").Replace("-", "").Replace("_", "");
                lst = lst.Where(x => x.Mobile != null).ToList();
                lst = lst.Where(p => p.Mobile.ToLower().Replace(".", "").Replace("-", "").Replace("_", "").Contains(customerMobile2.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                ViewBag.customerName = customerName.ToLower();
                string customerName2 = customerName.Replace(".", "").Replace("-", "").Replace("_", "");
                lst = lst.Where(x => x.CustomerName != null).ToList();
                lst = lst.Where(p => p.CustomerName.ToLower().Replace(".", "").Replace("-", "").Replace("_", "").Contains(customerName2.ToLower())).ToList();
            }
            if (CustomerCategoryId != 0)
            {
                lst = lst.Where(x => x.CustomerCategoryId == CustomerCategoryId).ToList();
            }
            if (Request.IsAjaxRequest())
                return PartialView("AjaxCustomer", lst.ToPagedList(pageNum, pageSize));
            return View(lst.ToPagedList(pageNum, pageSize));
        }
        public ActionResult ImportCustomer()
        {
            LoadData();
            return View();
        }
        [HttpPost]
        public ActionResult ImportCustomer(Customer customer, HttpPostedFileBase FileUpload)
        {
            if (FileUpload != null)
            {


                string filename = Path.GetFileName(Server.MapPath(FileUpload.FileName)); //get the uploaded file name  
                var fileExt = Path.GetExtension(filename); //get the extension of uploaded excel file  
                ISheet sheet;
                if (fileExt == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(FileUpload.InputStream); //HSSWorkBook object will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(FileUpload.InputStream); //XSSFWorkBook will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first Excel sheet from workbook   
                }
                for (int row = 1; row <= sheet.LastRowNum; row++) //Loop the records upto filled row  
                {
                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells   
                    {
                        var CustomerCategoryId = sheet.GetRow(row).GetCell(0).NumericCellValue;
                        var CustomerName = sheet.GetRow(row).GetCell(1).StringCellValue;
                        var Mobile = sheet.GetRow(row).GetCell(2).StringCellValue;
                        if (CustomerCategoryId != null && CustomerName != "" && CustomerName != null && Mobile != "" && Mobile != null)
                        {
                            Customer myCustomer = new Customer();
                            myCustomer.CreateDate = DateTime.Now;
                            myCustomer.IsDelete = false;
                            myCustomer.CustomerName = CustomerName;
                            myCustomer.Mobile = Regex.Replace(Mobile, @"[^\d]", "");
                            myCustomer.CustomerCategoryId = Convert.ToInt32(CustomerCategoryId);
                            if (_customerRepository.CheckExitNameAndMobileCustomer(myCustomer) == false)
                            {
                                _customerRepository.Insert(myCustomer);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ExportCustomer(string customerMobile, string customerName)
        {
            var lst = _customerRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(customerMobile))
            {
                ViewBag.customerMobile = customerMobile.ToLower();
                string customerMobile2 = customerMobile.Replace(".", "").Replace("-", "").Replace("_", "");
                lst = lst.Where(x => x.Mobile != null).ToList();
                lst = lst.Where(p => p.Mobile.ToLower().Replace(".", "").Replace("-", "").Replace("_", "").Contains(customerMobile2.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                ViewBag.customerName = customerName.ToLower();
                string customerName2 = customerName.Replace(".", "").Replace("-", "").Replace("_", "");
                lst = lst.Where(x => x.CustomerName != null).ToList();
                lst = lst.Where(p => p.CustomerName.ToLower().Replace(".", "").Replace("-", "").Replace("_", "").Contains(customerName2.ToLower())).ToList();
            }
            GridView gv = new GridView();
            gv.DataSource = lst;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=dskh.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", new { customerMobile = customerMobile, customerName = customerName });
        }
        // GET: /Customer/Details/5

        public ActionResult Details(long id = 0)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            LoadData();
            ViewBag.IdLastCustomer = _customerRepository.GetLastId() + 1;
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Create(Customer model)
        {
            LoadData();
            var error = validateCustomer(model);
            try
            {
                JsonModelReturnViewCustomer json = new JsonModelReturnViewCustomer();
                var now = DateTime.Now;
                var created = now;
                var modified = now;
                var isDelete = false;
                var my = new Customer()
                {
                    CustomerName = model.CustomerName,
                    Mobile = model.Mobile,
                    CreateDate = created,
                    CustomerCategoryId = model.CustomerCategoryId,
                    IsDelete = isDelete,
                };
                if (_customerRepository.CheckExitNameAndMobileCustomer(my))
                {
                    json.Customer = my;
                    json.createSuccess = false;
                    json.isExit = true;
                    return Json(json);
                }
                if (_customerRepository.Insert(my) != 0)
                {
                    json.Customer = my;
                    return Json(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public ActionResult Edit(int id)
        {
            LoadData();
            ViewBag.IdLastCustomer = _customerRepository.GetLastId() + 1;
            Customer model = new Customer();
            model = _customerRepository.GetById(id);
            return View(model);
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Edit(Customer model)
        {
            LoadData();
            var error = validateCustomer(model);
            try
            {
                JsonModelReturnViewCustomer json = new JsonModelReturnViewCustomer();
                var now = DateTime.Now;
                var created = now;
                var modified = now;

                var isDelete = false;
                var my = new Customer()
                {
                    CustomerId = model.CustomerId,
                    CustomerName = model.CustomerName,
                    Mobile = model.Mobile,
                    CreateDate = created,
                    CustomerCategoryId = model.CustomerCategoryId,
                    IsDelete = isDelete,
                };
                if (_customerRepository.CheckExitNameAndMobileCustomer(my))
                {
                    json.Customer = my;
                    json.createSuccess = false;
                    json.isExit = true;
                    return Json(json);
                }
                if (_customerRepository.Edit(my))
                {
                    json.Customer = my;
                    return Json(json);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        private bool validateCustomer(Customer customer)
        {
            try
            {
                if (customer.CustomerName == null || customer.CustomerName.Trim().Length == 0)
                {
                    ViewBag.MessageError = "Customer name is empty!";
                    return true;
                }
                if (customer.Mobile == null || customer.Mobile.Trim().Length == 0)
                {
                    ViewBag.MessageError = "Contact number is empty!";
                    return true;
                }
                if (customer.CustomerCategoryId == null )
                {
                    ViewBag.MessageError = "Category type can not empty!";
                    return true;
                }
            }
            catch
            {
                return true;
            }
            return false;
        }

        //
        // GET: /Customer/Delete/5

        public JsonResult Delete(long customerId = 0)
        {
            _customerRepository.Delete(customerId, true);
            return Json("Delete success");
        }
        public JsonResult UnDelete(long customerId = 0)
        {
            _customerRepository.Delete(customerId, false);
            return Json("Restore success");
        }

        private void LoadData()
        {
            ViewBag.CustomerCategories = new SelectList(_customerCategoriesRepository.GetAll(false), "CustomerCategoryId", "Name", null);
        }
    }
}