using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using SG_BLL.Tools;
using SG_DAL.Context;
using SG_DAL.Entities;
using SG_DAL.Pattern;

namespace SG_BLL
{
    public class FileManager
    {
        public static List<Teacher> ReadTeachersFromExcel(string filePath)
        {
            var data = new StringBuilder();
            try
            {
                Net.SourceForge.Koogra.IWorkbook wb = null;

                string fileExt = Path.GetExtension(filePath);

                if (string.IsNullOrEmpty(fileExt))
                {
                    throw new Exception("File extension not found");
                }

                if (fileExt.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    wb = Net.SourceForge.Koogra.WorkbookFactory.GetExcel2007Reader(filePath);
                }
                else if (fileExt.Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    wb = Net.SourceForge.Koogra.WorkbookFactory.GetExcelBIFFReader(filePath);
                }

                Net.SourceForge.Koogra.IWorksheet ws = wb.Worksheets.GetWorksheetByIndex(0);
                List<Teacher> list = new List<Teacher>();

                for (uint r = ws.FirstRow + 1; r <= ws.LastRow; ++r)
                {
                    Net.SourceForge.Koogra.IRow row = ws.Rows.GetRow(r);
                    if (row != null)
                    {
                        Teacher teacher = new Teacher();
                        teacher.User = new User();

                        teacher.Unvan = (int)SG_DAL.Enums.EnumUnvan.Ogretmen;

                        teacher.GenelBasvuru = true;
                        teacher.User.Rol = (int)SG_DAL.Enums.EnumRol.ogretmen;
                        teacher.GorevSayisi = 0;

                        for (uint colCount = ws.FirstCol; colCount <= ws.LastCol; ++colCount)
                        {
                            string cellData = string.Empty;

                            if (row.GetCell(colCount) != null && row.GetCell(colCount).Value != null)
                            {
                                cellData = row.GetCell(colCount).Value.ToString();

                                switch (colCount)
                                {
                                    case 0: teacher.User.Ad = cellData;
                                        break;
                                    case 1: teacher.User.Soyad = cellData;
                                        break;
                                    case 2: teacher.User.TCKimlik = Convert.ToInt64(cellData);
                                            teacher.User.Sifre = cellData;
                                        break;
                                    case 3: teacher.User.Email = cellData;
                                        break;
                                    case 4: teacher.User.Tel = cellData;
                                        break;
                                    case 5: teacher.Kidem = cellData;
                                        break;
                                    case 6:
                                        using (SGContext db = new SGContext())
                                        {
                                            var schoolRepo = new Repository<School>(db);
                                            int mebkodu = Convert.ToInt32(cellData);
                                            School sch = schoolRepo.First(d => d.MebKodu == mebkodu);
                                            teacher.SchoolId = sch.SchoolId;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        list.Add(teacher);
                    }
                }
                return list;
            }
            catch (Exception)
            {
                return new List<Teacher>();
                //Exception exception = ex;
                //exception.Source = string.Format("Error occured on row {0} col {1}", rowNum, colNum);
                //throw ex;
            }
        }

        public static string FileUpload(System.Web.HttpPostedFileBase file, string path)
        {
            if (file != null && file.ContentLength > 0)
            {
                file.SaveAs(HttpContext.Current.Server.MapPath(path + Utility.SetPagePlug(file.FileName.Split('.')[0].ToString()) + Path.GetExtension(file.FileName)));
                return path + Utility.SetPagePlug(file.FileName.Split('.')[0].ToString()) + Path.GetExtension(file.FileName);
            }
            else
            {
                return "";
            }
        }
    }
}
