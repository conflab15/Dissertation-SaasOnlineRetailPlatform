﻿namespace OnlineRetailPlatformDiss.Models
{
    public class BasketModel
    {
        public int Id { get; set; }
        public string? BasketId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public string? ProductColour { get; set; }
        public string? CustomOptions { get; set; }
        public DateTime DateCreated { get; set; }
        public ProductModel? Product { get; set; }
    }
}
