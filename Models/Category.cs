using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplicationShop.Models
{
    [Bind(Exclude = "CategoryId")]
    public partial class Category
    {
        [ScaffoldColumn(false)]
        public int CategoryId { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "A Category name is required")]
        [StringLength(160)]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "An Album Title is required")]
        [StringLength(160)]
        public string Description { get; set; }


        //public virtual Category ParentCategory { get; set; }
        public virtual List<Product> Products { get; set; }

        //public List<Category> SubCategories { get; }

        //public void AddSubCategory(Category _subCategory)
        //{
        //    if (!_subCategory.Equals(null) && !IsSubCategory(_subCategory.CategoryName))
        //    {
        //        SubCategories.Add(_subCategory);
        //    }
        //}

        //public void RemSubCategory(string _subCategoryName)
        //{ 
        //    SubCategories.Remove(GetSubCategory(_subCategoryName));
        //}

        //private Category GetSubCategory(string _subCategoryName)
        //{
        //    foreach (Category subCategory in SubCategories)
        //    {
        //        if (subCategory.CategoryName.Equals(_subCategoryName))
        //        {
        //            return subCategory;
        //        }
        //    }
        //    return null;
        //}

        //public Boolean IsSubCategory(string _subCategoryName)
        //{
        //    if (GetSubCategory(_subCategoryName).Equals(null)){
        //        return false;
        //    }
        //    return true;
        //}
    }
}