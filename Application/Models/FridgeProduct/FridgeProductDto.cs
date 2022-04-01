﻿using System;

namespace Application.Models.Fridge
{
    public class FridgeProductDto
    {
        public Guid Id { get; set; }
        public Guid FridgeId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
    }
}