using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Models
{
    public class SinavGorevlendirmeWrapperModel
    {
        public List<School> okul { get; set; }
        public List<Teacher> ogretmen { get; set; }
        public SinavOturum oturum { get; set; }
        public Setting setting { get; set; }
        public List<SinavOturumOkullari> snvOtrOkl { get; set; }

        public SinavGorevlendirmeWrapperModel(List<Teacher> ogretmen, List<School> okul, SinavOturum oturum, Setting setting, List<SinavOturumOkullari> snvOtrOkl)
        {
            this.ogretmen = ogretmen;
            this.okul = okul;
            this.oturum = oturum;
            this.setting = setting;
            this.snvOtrOkl = snvOtrOkl;
        }
    }
}