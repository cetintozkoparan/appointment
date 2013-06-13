using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Models
{
    public class SinavWrapperModel
    {
        public Sinav sinav { get; set; }
        public SinavOturum sinavoturum { get; set; }

        public SinavWrapperModel(Sinav sinav, SinavOturum sinavoturum)
        {
            this.sinav = sinav;
            this.sinavoturum = sinavoturum;
        }
    }
}