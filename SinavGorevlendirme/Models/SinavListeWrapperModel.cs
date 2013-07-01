using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Models
{
    public class SinavListeWrapperModel
    {
        public List<Sinav> sinav { get; set; }
        public List<SinavOturum> sinavoturum { get; set; }
        public List<SinavOturum> sinavoturumbasvuru { get; set; }
        public List<SinavOturum> sinavoturumgorevli { get; set; }
        public Setting ayar { get; set; }

        public SinavListeWrapperModel(List<Sinav> sinav, List<SinavOturum> sinavoturum, Setting ayar, List<SinavOturum> sinavoturumbasvuru, List<SinavOturum> sinavoturumgorevli)
        {
            this.sinav = sinav;
            this.sinavoturum = sinavoturum;
            this.ayar = ayar;
            this.sinavoturumbasvuru = sinavoturumbasvuru;
            this.sinavoturumgorevli = sinavoturumgorevli;
        }
    }
}