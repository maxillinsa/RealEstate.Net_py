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
    public class CategoryController : Controller
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
        public CategoryController()
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

        public ActionResult Index(int id, int? page)
        {
          
            Category cate = new Category();
            cate = _ICategoryRepository.GetById(id);
            if (id == 8)
            {
                return RedirectToAction("Contact","Home");
            }
            if (id == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            if(cate.CategoryTypeId == 2)
            {
                return RedirectToAction("List", "Product", new { id = id });
            }
            if (cate.CategoryTypeId == 3)
            {
                return RedirectToAction("List", "Service", new { id = id });
            }
            return RedirectToAction("List", "Article", new { id = id});
         
        }


    }
}