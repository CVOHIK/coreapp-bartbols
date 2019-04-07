using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplicationShop.Models;

namespace WebApplicationShop.Models
{
    public class ShoppingCartViewModel
    {
        [Key]
        public int ShoppingCartViewModelId { get; set; }
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}