﻿using System.ComponentModel.DataAnnotations;

namespace NFKApplication.Models
{

    public class Product
    {
        [Key]
        public int Sku { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }
}
