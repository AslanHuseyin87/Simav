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
    public class BilgilendirmeController : Controller
    {
        private readonly IService<Bilgilendirme> _service;
        public BilgilendirmeController(IService<Bilgilendirme> service)
        {
            _service = service;
        }
        [AutFilter]
        [HttpGet]
        public IActionResult Index()
        {
            var entity = _service.FindAll(x => x.Durum.Equals((byte)Enums.KayitDurumu.Aktif)); ;
            return View(entity);
        }
        [AutFilter]
        [HttpGet]
        public IActionResult YeniBilgilendirme()
        {
            ViewBag.Baslik = "Yeni Bilgilendirme";
            return View();
        }
        [AutFilter]
        [HttpPost]
        public IActionResult YeniBilgilendirme(Bilgilendirme entity)
        {
            ViewBag.Baslik = "Yeni Bilgilendirme Ekle";
            entity.DegistirenKulId = SessionInfo.GirisYapanKullaniciId;
            entity.DegistirmeTarihi = DateTime.Now;
            entity.KaydedenKulId = SessionInfo.GirisYapanKullaniciId;
            entity.KayıtTarihi = DateTime.Now;
            entity.Durum = (byte)Enums.KayitDurumu.Aktif;
            _service.Save(entity);
            return RedirectToAction("Index", "Bilgilendirme");
        }
        [AutFilter]
        [HttpGet]
        public IActionResult BilgilendirmeGuncelle(int? id)
        {
            ViewBag.Baslik = "Bilgilendirme Düzenle";
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
        [HttpPost]
        public IActionResult BilgilendirmeGuncelle(Bilgilendirme entity)
        {
            ViewBag.Baslik = "Bilgilendirme Güncelle";
            if (ModelState.IsValid)
            {
                entity.DegistirenKulId = SessionInfo.GirisYapanKullaniciId;
                entity.DegistirmeTarihi = DateTime.Now;
                _service.Update(entity);
                return RedirectToAction("Index");
            }
            return View(entity);
        }
        [AutFilter]
        [HttpGet]
        public IActionResult BilgilendirmeDetaylar(int? id)
        {
            ViewBag.Baslik = "Bilgilendirme Detayları";
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
        public JsonResult BilgilendirmeSil(int pId)
        {
            try
            {
                var entity = _service.GetById(pId);
                if (entity == null)
                {
                    return Json(new { basarili = false, id = pId, mesaj = "İşlem Başarısız" });
                }
                entity.Durum = (byte)Enums.KayitDurumu.Silinmiş;
                _service.Update(entity);
            }
            catch (Exception)
            {
                return Json(new { basarili = false, id = pId, mesaj = "İşlem Başarısız" });
            }
            return Json(new { basarili = true, id = pId, mesaj = "İşlem Başarılı" });
        }
    }
}
