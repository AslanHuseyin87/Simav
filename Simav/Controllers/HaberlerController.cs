﻿using App.Common.Filters;
using Microsoft.AspNetCore.Mvc;
using Simav.Common;
using Simav.Core;
using Simav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simav.Controllers
{
    [AutFilter]
    public class HaberlerController : Controller
    {
        private readonly IService<Haberler> _service;
        public HaberlerController(IService<Haberler> service)
        {
            _service = service;
        }
        [AutFilter]
        public IActionResult Index()
        {
            ViewBag.Baslik = "Haber Listesi";
            var haberListesi = _service.FindAll(x=>x.Durum.Equals((byte)Enums.KayitDurumu.Aktif) && x.Onay.Equals((byte)Enums.HaberDurumu.Onaylanmis));
            return View(haberListesi);
        }
        [HttpGet]
        [AutFilter]
        public IActionResult YeniHaber()
        {
            ViewBag.Baslik = "Yeni Haber";
            return View();
        }
        [AutFilter]
        [HttpPost]
        public IActionResult YeniHaber(Haberler haber)
        {
            haber.DegistirenKulId = SessionInfo.GirisYapanKullaniciId;
            haber.DegistirmeTarihi = DateTime.Now;
            haber.KaydedenKulId = SessionInfo.GirisYapanKullaniciId;
            haber.KayıtTarihi = DateTime.Now;
            _service.Save(haber);
            return RedirectToAction("Index", "Haberler");
        }
        [AutFilter]
        [HttpGet]
        public IActionResult HaberDetaylar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _service.GetById(id.Value);
            if (entity == null)
            {
                return NotFound();//Bulunamadı
            }
            return View(entity);
        }
        [AutFilter]
        [HttpGet]
        public IActionResult HaberGuncelle(int? id)
        {
            ViewBag.Baslik = "Haber Düzenle";
            if (id == null)
            {
                return NotFound();
            }
            var entity = _service.GetById(id.Value);
            if (entity == null)
            {
                return NotFound();//Bulunamadı
            }
            return View(entity);
        }
        [HttpPost]
        public IActionResult HaberGuncelle(Haberler entity)
        {
            if (ModelState.IsValid)
            {
                entity.DegistirenKulId = SessionInfo.GirisYapanKullaniciId;
                entity.DegistirmeTarihi = DateTime.Now;
                _service.Update(entity);
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        public JsonResult HaberSil(int pId)
        {
            try
            {
                var haber=_service.GetById(pId);
                if (haber == null)
                {
                    return Json(new { basarili = false, id=pId ,mesaj = "İşlem Başarısız" });
                }
                haber.Durum = (byte)Enums.KayitDurumu.Silinmiş;
                _service.Update(haber);
            }
            catch (Exception)
            {
                return Json(new { basarili = false, id = pId, mesaj = "İşlem Başarısız" });
            }
           return Json(new { basarili = true, id = pId , mesaj = "İşlem Başarılı" }); 
        }
    }
}
