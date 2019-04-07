using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationShop.Models;
using WebApplicationShop.Data;


namespace webshop.Controllers
{
    public class ShopController : Controller
    {

        ShoppingContext shopDb = new ShoppingContext();

        //store
        public ActionResult Index()
        {
            //storeDb.Products.Add(new Product() { PublicName = "test" });
            //storeDb.Database.BeginTransaction();
            //shopDb.Database.CurrentTransaction.Dispose();
            //shopDb.Database.Initialize(force:true);
            var _categories = shopDb.Categories.ToList();
            return View(_categories);
        }
        public ActionResult Browse(string _shopCategory)
        {
            if (_shopCategory != null)
            {
                var _category = shopDb.Categories.Include("Products").First(g => g.CategoryName == _shopCategory);
                return View(_category);
            }
            return RedirectToAction("Shop");
        }

        // GET: store/Details/5
        public ActionResult Details(int _id)
        {
            if (!_id.Equals(null))
            {
                var _shopObject = shopDb.Products.Find(_id);
                return View(_shopObject);
            }
            return RedirectToAction("Shop");
        }
    }
}