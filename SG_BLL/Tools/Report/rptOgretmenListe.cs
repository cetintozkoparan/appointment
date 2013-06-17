using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SG_BLL.Tools.Report
{
    public class rptOgretmenListe
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public Int64 TCKimlik { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Kidem { get; set; }
        public int Unvan { get; set; }
        
        public rptOgretmenListe(string Ad, string Soyad, Int64 TCKimlik, string Email, string Tel, string Kidem, int Unvan)
        {
            this.Ad = Ad;
            this.Soyad = Soyad;
            this.TCKimlik = TCKimlik;
            this.Email = Email;
            this.Tel = Tel;
            this.Kidem = Kidem;
            this.Unvan = Unvan;
        }
    }
}