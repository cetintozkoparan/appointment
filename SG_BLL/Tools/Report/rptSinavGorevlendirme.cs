using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SG_BLL.Tools.Report
{
    public class rptSinavGorevlendirme
    {
        public string SinavAdi { get; set; }
        public string SinavTarihi { get; set; }
        public string SinavSaati { get; set; }
        public string SinavOkulAdi { get; set; }
        public string KomisyonBaskani { get; set; }
        public string KomisyonUyesi { get; set; }
        public string KomisyonUyesi2 { get; set; }
        public int SinavOkulMebKodu { get; set; }
        public string KadroluOlduguOkulAdi { get; set; }
        public Int64 PersonelTC { get; set; }
        public string PersonelAdSoyad { get; set; }
        public int PersonelSira { get; set; }
        public string PersonelGorev { get; set; }
        public int SinavOkulId { get; set; }
        public bool KatilimDurumu { get; set; }
        public int OgretmenId { get; set; }

        public rptSinavGorevlendirme(string SinavAdi, DateTime SinavTarihi, string SinavSaati, string SinavOkulAdi, string KomisyonBaskani,
                                     string KomisyonUyesi, string KomisyonUyesi2, int SinavOkulMebKodu, string KadroluOlduguOkulAdi,
                                     Int64 GozetmenTC, string GozetmenAdSoyad, int GozetmenSira, string PersonelGorev, int SinavOkulId, 
                                     bool KatilimDurumu, int OgretmenId
            )
        {
            this.SinavAdi = SinavAdi;
            this.SinavTarihi = SinavTarihi.ToShortDateString();
            this.SinavSaati = SinavSaati;
            this.SinavOkulAdi = SinavOkulAdi;
            this.KomisyonBaskani = KomisyonBaskani;
            this.KomisyonUyesi = KomisyonUyesi;
            this.KomisyonUyesi2 = KomisyonUyesi2;
            this.SinavOkulMebKodu = SinavOkulMebKodu;
            this.SinavOkulId = SinavOkulId;
            this.KadroluOlduguOkulAdi = KadroluOlduguOkulAdi;
            this.PersonelTC = GozetmenTC;
            this.PersonelAdSoyad = GozetmenAdSoyad;
            this.PersonelSira = GozetmenSira;
            this.PersonelGorev = PersonelGorev;
            this.KatilimDurumu = KatilimDurumu;
            this.OgretmenId = OgretmenId;
        }
    }
}