﻿using LibAdvertisementDB;

namespace WebAdvertisementApi.Models
{
    public class AddAdvertisement
    {
        public int Number { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; } 
        public int Rating { get; set; }
        public DateTime Created { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string NameImg { get; set; }
        public byte[] Img { get; set; }
    }
}