using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationShop.Models
{
    public class OrderObject
    {
        public int OrderObjectId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ProductQnt { get; set; }
        public decimal Price { get; set; }
        public Boolean BulkPrice { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}