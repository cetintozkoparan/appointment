using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SG_BLL.Tools.Report
{
    public class rptSinavGorevlendirme
    {

        //public string Ad { get; set; }
        //public string Soyad { get; set; }
        //public Int64 TCKimlik { get; set; }
        //public string Email { get; set; }
        //public string Tel { get; set; }
        //public string Kidem { get; set; }
        //public int Unvan { get; set; }

        //public rptSinavGorevlendirme(string Ad, string Soyad, Int64 TCKimlik, string Email, string Tel, string Kidem, int Unvan)
        //{
        //    this.Ad = Ad;
        //    this.Soyad = Soyad;
        //    this.TCKimlik = TCKimlik;
        //    this.Email = Email;
        //    this.Tel = Tel;
        //    this.Kidem = Kidem;
        //    this.Unvan = Unvan;
        //}


        public string SinavAdi { get; set; }
        public string SinavTarihi { get; set; }
        public string SinavSaati { get; set; }
        public string SinavOkulAdi { get; set; }
        public string KomisyonBaskani { get; set; }
        public string KomisyonUyesi { get; set; }
        public string KomisyonUyesi2 { get; set; }
        public int SinavOkulMebKodu { get; set; }
        public string GorevliOlduguOkulAdi { get; set; }

        public rptSinavGorevlendirme(string SinavAdi, DateTime SinavTarihi, string SinavSaati, string SinavOkulAdi, string KomisyonBaskani, string KomisyonUyesi, string KomisyonUyesi2, int SinavOkulMebKodu, string GorevliOlduguOkulAdi)
        {
            this.SinavAdi = SinavAdi;
            this.SinavTarihi = SinavTarihi.ToShortDateString();
            this.SinavSaati = SinavSaati;
            this.SinavOkulAdi = SinavOkulAdi;
            this.KomisyonBaskani = KomisyonBaskani;
            this.KomisyonUyesi = KomisyonUyesi;
            this.KomisyonUyesi2 = KomisyonUyesi2;
            this.SinavOkulMebKodu = SinavOkulMebKodu;
            this.GorevliOlduguOkulAdi = GorevliOlduguOkulAdi;
        }
    }
}