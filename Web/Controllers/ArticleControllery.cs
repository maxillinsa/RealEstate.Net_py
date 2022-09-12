using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomRoles;
using Web.DAL.IRepository;
using Web.DAL.Repository;
using Web.Models;
using MvcPaging;
namespace Web.Controllers
{
    public class ArticleController : Controller
    {
        private ISanPhamRepository _ISanPhamRepository;
        private IAccountRepository _accountRepository;
        private ICommentRepository _commentRepository;
        private IThongBaoRepository _thongBaoRepository;
        private IProductRepository _IproductRepository;
        private ICategoryRepository _ICategoryRepository;
        private IVendorRepository _IvendorRepository;
        private IAlbumRepository _albumRepository;
        private IArticleRepository _articleRepository;
        private IImageRepository _imageRepository;
        public ArticleController()
        {

            _ISanPhamRepository = new SanPhamRepository();
            this._accountRepository = new AccountRepository();
            _thongBaoRepository = new ThongBaoRepository();
            _commentRepository = new CommentRepository();
            _IproductRepository = new ProductRepository();
            _IvendorRepository = new VendorRepository();
            _ICategoryRepository = new CategoryRepository();
            _albumRepository = new AlbumRepository();
            _articleRepository = new ArticleRepository();
            _imageRepository = new ImageRepository();


        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            Article model = new Article();
            model = _articleRepository.GetById(id);
            List<Article> lstModel = new List<Article>();

            lstModel = _articleRepository.TakeByCategoryId2(model.CategoryId, model.Category.CategoryTypeId, 4).ToList();

            lstModel.Remove(model);

            Account accDetail = new Account();
            if(model.CreateBy == null)
            {
                accDetail = _accountRepository.GetById(1);
            }
            else
            {
                accDetail = _accountRepository.GetById(Convert.ToInt64(model.CreateBy));
            }
            ViewBag.accDetail = accDetail;

            ViewBag.lstModel = lstModel;

            return View(model);
        }

        //public ActionResult IndexAjax()
        //{
        //    int currentPageIndex = 0;
        //    var products = this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize);
        //    return View(products);
        //}

        //public ActionResult AjaxPage(int? page)
        //{
        //    ViewBag.Title = "Browse all products";
        //    int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
        //    var products = this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize);
        //    return PartialView("_ProductGrid", products);
        //}
        public ActionResult List(int id, int? page)
        {
            Category cate = new Category();
            cate = _ICategoryRepository.GetById(id);
          
            List<Article> model = new List<Article>();
             model = _articleRepository.GetAllByCategoryId(id, 1);
             ViewBag.cate = cate;
             int currentPageIndex = page.HasValue ? page.Value : 1;
            if (Request.IsAjaxRequest())
                return PartialView("AjaxArticle", model.ToPagedList(currentPageIndex, 10));
            return View(model.ToPagedList(currentPageIndex, 10));
        }
    }
}