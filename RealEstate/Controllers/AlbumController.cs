using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealEstate.Models;
using RealEstate.DAL.IRepository;
using RealEstate.DAL.Repository;
using System.IO;
using RealEstate.Common;
using CustomRoles;
using PagedList;
namespace RealEstate.Controllers
{
    [CustomAuthorize(Roles = "Admin,Editor")]
    public class AlbumController : Controller
    {
        private IAlbumRepository _albumRepository;
        private IImageRepository _imageRepository;
        private const int pageSize = 30;
        private string ImageUploadsFolder
        {
            get
            {
                return Path.Combine(Server.MapPath("/Media/Images/album"));
            }
        }
        public AlbumController(IAlbumRepository albumRepository, IImageRepository imageRepository)
        {
            _albumRepository = albumRepository;
            _imageRepository = imageRepository;
        }
        //
        // GET: /Services/

        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            List<Album> ListModel = _albumRepository.GetAll();
            return View(ListModel.ToPagedList(pageNum, pageSize));
        }

        //
        // GET: /Services/Details/5

        //
        // GET: /Services/Create

        public ActionResult Create()
        {
            ViewBag.ListTags = GetTagsAlbum();
            return View();
        }

        //
        // POST: /Services/Create

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlbumImageView collection, HttpPostedFileBase txtFile)
        {
            long albumId = 0;
            if (ModelState.IsValid)
            {
                if (txtFile != null)
                {
                    if (txtFile.ContentLength / 1024 > 1500)
                    {
                        ModelState.AddModelError("txtFile", "Image maximum 1.5MB");
                    }
                    string server = string.Empty;
                    server = ImageUploadsFolder;
                    String nameImage = CreateNewName(Path.GetExtension(txtFile.FileName));
                    var fullName = Path.Combine(server, nameImage);
                    ImageHelper.UploadImage(txtFile, fullName);
                    collection.album.Image = nameImage;
                }

                collection.album.CreateDate = DateTime.Now;
                collection.album.IsDelete = false;
                if (collection.album.Alias == null)
                    collection.album.Alias = Functions.generateAlias(collection.album.Name);
                if (collection.album.MetaTitle == null)
                    collection.album.MetaTitle = collection.album.Name;
                albumId = _albumRepository.Insert(collection.album);
                if (collection.Images != null)
                {
                    foreach (var item in collection.Images)
                    {
                        //FileReader
                        string server = string.Empty;
                        server = ImageUploadsFolder;
                        String nameImage = CreateNewName(".jpg");
                        var fullName = Path.Combine(server, nameImage);
                        var myString = item.Name.Split(new char[] { ',' });
                        byte[] bytes = Convert.FromBase64String(myString[1]);
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            image.Save(fullName);
                        }
                        Image img = new Image();
                        img.Name = nameImage;
                        img.Title = item.Title;
                        img.Link = item.Link;
                        img.CreateDate = DateTime.Now;
                        img.IsDelete = false;
                        img.AlbumId = albumId;
                        img.ImageId = _imageRepository.Insert(img);
                    }
                }
                return RedirectToAction("index");
            }
            else
                ViewBag.ListTags = GetTagsAlbum();
            return View(collection);


        }

        //
        // GET: /Services/Edit/5

        public ActionResult Edit(long id)
        {
            AlbumImageView model = new AlbumImageView();
            ViewBag.ListTags = GetTagsAlbum();
            Album album = _albumRepository.GetById(id);
            model.album = album;
            model.Images = _imageRepository.GetAllByAlbumId(id);
            return View(model);
        }
        //
        // POST: /Services/Edit/5

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AlbumImageView collection, HttpPostedFileBase txtFile)
        {
            if (ModelState.IsValid)
            {
                if (txtFile != null)
                {
                    if (txtFile.ContentLength / 1024 > 1500)
                    {
                        ModelState.AddModelError("txtFile", "Image maximum 1.5MB");
                    }
                    string server = string.Empty;
                    server = ImageUploadsFolder;
                    String nameImage = CreateNewName(Path.GetExtension(txtFile.FileName));
                    var fullName = Path.Combine(server, nameImage);
                    ImageHelper.UploadImage(txtFile, fullName);
                    collection.album.Image = nameImage;
                }

                collection.album.CreateDate = DateTime.Now;
                collection.album.IsDelete = false;
                if (collection.album.Alias == null)
                    collection.album.Alias = Functions.generateAlias(collection.album.Name);
                if (collection.album.MetaTitle == null)
                    collection.album.MetaTitle = collection.album.Name;
                _albumRepository.Edit(collection.album);
                var lstImageAlbum = _imageRepository.GetAllByAlbumId(collection.album.AlbumId);
                if (collection.Images != null)
                {
                    foreach (var item in lstImageAlbum)
                    {
                        var temp = false;
                        foreach (var item1 in collection.Images)
                            if (item.ImageId == item1.ImageId)
                                temp = true;
                        if (temp == false)
                            _imageRepository.DeleteByAblumId(Convert.ToInt64(item.ImageId));
                    }
                    foreach (var item in collection.Images)
                    {
                        if (item.ImageId <= 0)
                        {
                            //FileReader
                            string server = string.Empty;
                            server = ImageUploadsFolder;
                            String nameImage = CreateNewName(".jpg");
                            var fullName = Path.Combine(server, nameImage);
                            var myString = item.Name.Split(new char[] { ',' });
                            byte[] bytes = Convert.FromBase64String(myString[1]);
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                                image.Save(fullName);
                            }
                            Image img = new Image();
                            img.Name = nameImage;
                            img.Title = item.Title;
                            img.Link = item.Link;

                            img.CreateDate = DateTime.Now;
                            img.IsDelete = false;
                            img.AlbumId = collection.album.AlbumId;
                            img.ImageId = _imageRepository.Insert(img);
                        }
                    }
                }
                return RedirectToAction("index");
            }
            else
                ViewBag.ListTags = GetTagsAlbum();
            return View(collection);

        }

        //
        // GET: /Services/Delete/5

        public ActionResult Delete(long id)
        {
            _albumRepository.Delete(id, true);
            return RedirectToAction("Index");
        }
        public ActionResult UnDelete(long id)
        {
            _albumRepository.Delete(id, false);
            return RedirectToAction("Index");
        }
        private string CreateNewName(string str)
        {
            return DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString() + str;
        }
        private string GetTagsAlbum()
        {
            var list = _albumRepository.GetAll();
            string temp = "";
            foreach (var item in list)
            {
                temp += item.Tags;
            }
            var listTemp = temp.Split(',').Distinct().ToList();
            var rs = "";
            foreach (var item in listTemp)
            {
                rs += item + ",";
            }
            return rs;
        }
    }
}