using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomRoles;
using Web.DAL.IRepository;
using Web.DAL.Repository;
using Web.Models;
namespace Web.Controllers
{
    public class HomeController : Controller
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
        public HomeController()
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
       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [ChildActionOnly]
        public ActionResult MenuHomePartial()
        {
            List<Category> model = new List<Category>();
            model = _ICategoryRepository.GetAll(false).OrderBy(x=>x.OrderNumber).ToList();
            return PartialView(model);

        }
         [ChildActionOnly]
        public ActionResult HotNewsPartial()
        {
            List<Article> model = new List<Article>();
            model = _articleRepository.GetAllByCategoryId(5, 1).OrderByDescending(x => x.CreateDate).Take(6).ToList();
            return PartialView(model);
        }
         [ChildActionOnly]
        public ActionResult BannerOuterPartial()
        {
            Album model = new Album();
            model = _albumRepository.GetById(1);
            ViewBag.slideShow = model;

            List<Image> lstImage = new List<Image>();
            lstImage = _imageRepository.GetAllByAlbumId(1);
            ViewBag.lstImage = lstImage;

            Article new1 = new Article();
            new1 = _articleRepository.GetById(1);
            ViewBag.new1 = new1;

            List<Article> Lstnew1 = new List<Article>();
            Lstnew1 = _articleRepository.GetAllByCategoryId(5, 1).OrderByDescending(x => x.CreateDate).Take(2).ToList(); ;
            ViewBag.Lstnew1 = Lstnew1;

            return PartialView();

        }
         [ChildActionOnly]
        public ActionResult RightSidePartial()
        {
            //List<Comment> model = new List<Comment>();
            //model = _commentRepository.GetAllToDay();
            return PartialView();

        }
         [ChildActionOnly]
        public ActionResult LeftSidePartial()
        {

            List<Article> model = new List<Article>();
            ViewProductModel view1 = new ViewProductModel(); ViewProductModel view2 = new ViewProductModel();
            ViewArticleModel view3 = new ViewArticleModel(); ViewArticleModel view4 = new ViewArticleModel();
            ViewBag.view1 = null; ViewBag.view2 = null; ViewBag.view3 = null; ViewBag.view4 = null; 
            Category cate1 = new Category();
            cate1 = _ICategoryRepository.GetById(2);
            List<Product> Lstpro1 = new List<Product>();
            view1.category = cate1;
            Lstpro1 = _IproductRepository.GetAllByCateId(2);
             if(Lstpro1.Count != 0)
             { 
                Product pro1 = new Product();
                var random1 = new Random();
                int index1 = random1.Next(Lstpro1.Count);
                pro1 = Lstpro1.ElementAt(index1);
                Lstpro1.RemoveAt(index1);
                view1.productTop = pro1;
                view1.ListPro = Lstpro1;
             }

            Category cate2 = new Category();
            cate2 = _ICategoryRepository.GetById(3);
            List<Product> Lstpro2 = new List<Product>();
            view2.category = cate2;
            Lstpro2 = _IproductRepository.GetAllByCateId(3);
            if (Lstpro2.Count != 0)
            {
                Product pro2 = new Product();
                var random2 = new Random();
                int index2 = random2.Next(Lstpro2.Count);
                pro2 = Lstpro2.ElementAt(index2);
                Lstpro2.RemoveAt(index2);
                view2.productTop = pro2;
                view2.ListPro = Lstpro2;
            }
            Category cate3 = new Category();
            cate3 = _ICategoryRepository.GetById(5);
            view3.category = cate3;
            List<Article> LstArt3 = new List<Article>();
            LstArt3 = _articleRepository.GetAllByCategoryId(5, 1);
            if (LstArt3.Count != 0)
            {
                Article art3 = new Article();
                var random3 = new Random();
                int index3 = random3.Next(LstArt3.Count);
                art3 = LstArt3.ElementAt(index3);
                LstArt3.RemoveAt(index3);
                view3.articleTop = art3;
                view3.ListArt = LstArt3;
            }

            Category cate4 = new Category();
            cate4 = _ICategoryRepository.GetById(6);
            view4.category = cate4;
            List<Article> LstArt4 = new List<Article>();
            LstArt4 = _articleRepository.GetAllByCategoryId(6, 1);
            if (LstArt4.Count != 0)
            {
                Article art4 = new Article();
                var random4 = new Random();
                int index4 = random4.Next(LstArt4.Count);
                art4 = LstArt4.ElementAt(index4);
                LstArt4.RemoveAt(index4);
                view4.articleTop = art4;
                view4.ListArt = LstArt4;
            }
            ViewBag.view4 = view4; ViewBag.view3 = view3; ViewBag.view2 = view2; ViewBag.view1 = view1;

            model = _articleRepository.GetAllByCategoryId(4, 1).OrderByDescending(x => x.CreateDate).Take(6).ToList();
            return PartialView(model);
        }
         [ChildActionOnly]
        public ActionResult TopNewsFooterPartial()
        {
            return PartialView();
        }
         [ChildActionOnly]
        public ActionResult FooterCategory()
        {
            List<Category> model = new List<Category>();
            model = _ICategoryRepository.GetAll(false).Where(x => x.CateLevel == 1).ToList();
            return PartialView(model);

        }
    }
}