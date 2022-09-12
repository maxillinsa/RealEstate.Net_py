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
    public class ProductController : Controller
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
        public ProductController()
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
        public ActionResult Detail(long id)
        {
            Product model = new Product();
            model = _IproductRepository.GetById(id);

            List<Product> lstModel = new List<Product>();

            lstModel = _IproductRepository.TakeByCategoryId2(model.CategoryId, model.Category.CategoryTypeId, 4).ToList();

            lstModel.Remove(model);

            Account accDetail = new Account();
            if (model.CreatedBy == null)
            {
                accDetail = _accountRepository.GetById(1);
            }
            else
            {
                accDetail = _accountRepository.GetById(Convert.ToInt64(model.CreatedBy));
            }
            ViewBag.accDetail = accDetail;

            ViewBag.lstModel = lstModel;
            return View(model);
        }
        public ActionResult List(int id)
        {
            return View();
        }
    }
}