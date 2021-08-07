﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Simav.Models
{
    public partial class Duyuru
    {
        public int DuyuruId { get; set; }
        public byte Durum { get; set; }
        public DateTime KayıtTarihi { get; set; }
        public int KaydedenKulId { get; set; }
        public int DegistirenKulId { get; set; }
        public DateTime DegistirmeTarihi { get; set; }
        [Display (Name ="Kısa Açıklama")]
        public string KisaAciklama { get; set; }
        [Display(Name = "İçerik")]
        public string Icerik { get; set; }
        public string Ad { get; set; }
        public DateTime? Tarih { get; set; }
        public byte? Onay { get; set; }
    }
}
