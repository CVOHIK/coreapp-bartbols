using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplicationShop.Models
{
    public class Product
    {
        [ScaffoldColumn(false)]
        public int ProductId { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "A name is required")]
        [StringLength(160)]
        public string PublicName { get; set; }

        [DisplayName("Product description")]
        [Required(ErrorMessage = "A description of some sorts is required")]
        [StringLength(1024)]
        public string Description { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [DisplayName("Private description")]
        [StringLength(1024)]
        public string PrivateDescription { get; set; }

        [Required(ErrorMessage = "Base pricing is required")]
        [Range(0.01, 1000000.00, ErrorMessage = "Quantity must be between 0.01 and 1000000.00")]
        public decimal BasePrice { get; set; }

        [Required(ErrorMessage = "Bulk pricing is required")]
        [Range(0.01, 1000000.00, ErrorMessage = "Quantity must be between 0.01 and 1000000.00")]
        public decimal BulkPrice { get; set; }

        [Required(ErrorMessage = "Bulk quantity is required")]
        [Range(2, 100, ErrorMessage = "Quantity must be between 2 and 100")]
        public int BulkQnt { get; set; }

        [DisplayName("Product Art URL")]
        [StringLength(1024)]
        public string ImageUrl { get; set; }


        public virtual Category Category { get; set; }

    }
}