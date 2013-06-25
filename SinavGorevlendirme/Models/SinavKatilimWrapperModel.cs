using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SG_BLL.Tools.Report;
using SG_DAL.Entities;

namespace SinavGorevlendirme.Models
{
    public class SinavKatilimWrapperModel
    {
        public List<rptSinavGorevlendirme> sinav { get; set; }
        public List<SinavGorevli> teachers { get; set; }

        public SinavKatilimWrapperModel(List<rptSinavGorevlendirme> sinav, List<SinavGorevli> teachers)
        {
            this.sinav = sinav;
            this.teachers = teachers;
        }
    }
}