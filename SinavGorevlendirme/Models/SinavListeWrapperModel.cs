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

        public SinavListeWrapperModel(List<Sinav> sinav, List<SinavOturum> sinavoturum)
        {
            this.sinav = sinav;
            this.sinavoturum = sinavoturum;
        }
    }
}