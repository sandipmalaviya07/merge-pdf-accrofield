using iTextSharp.text.pdf;
using MergePDFAcroFields.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MergePDFAcroFields.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("/test")]
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult MergePDF()
        {
            string firstPDFPath = string.Empty;
            string secondPDFPath = string.Empty;
            string thirdPDFPath = string.Empty;
            var myFileList = new List<string>();
            long fileTimeStamp = DateTime.Now.ToFileTime(); // prepare temp folder 
            int counter = 0; // used for creating files number wise
            string mainDirectoryPath = Server.MapPath("~/Document");
            DirectoryInfo dir = new DirectoryInfo(mainDirectoryPath);
            CommonService _commonService = new CommonService();
            try
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Document/"+ fileTimeStamp.ToString()));

                for (int i = 0; i < dir.GetFiles().Length; i++)
                {
                    firstPDFPath = Server.MapPath("~/Document/Test" + (i + 1) + ".pdf");

                    PdfReader _readerPayment = new PdfReader(firstPDFPath);

                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        PdfStamper stamper = new PdfStamper(_readerPayment, ms);
                        using (stamper)
                        {
                            Dictionary<string, string> info = _readerPayment.Info;
                            stamper.MoreInfo = info;
                            _commonService.SetPDFFields(stamper.AcroFields);
                        }
                        System.IO.MemoryStream msn = new System.IO.MemoryStream(ms.ToArray());
                        System.IO.FileStream file = new System.IO.FileStream(Server.MapPath("~/Document/" + fileTimeStamp.ToString() + "/" + (++counter) + ".pdf"), FileMode.Create, FileAccess.Write);
                        myFileList.Add(Server.MapPath("~/Document/" + fileTimeStamp.ToString() + "/" + (counter) + ".pdf"));
                        msn.WriteTo(file);
                        file.Close();
                        msn.Close();
                    }
                }
                string[] _allFiles = System.IO.Directory.GetFiles(Server.MapPath("~/Document/" + fileTimeStamp.ToString()));
                if (counter > 0 && myFileList.Count > 0)
                {
                    MergePDFs(Server.MapPath("~/Document/MainApp_"+fileTimeStamp+".pdf"), _allFiles);
                }

                return File(Server.MapPath("~/Document/MainApp_" + fileTimeStamp + ".pdf"), "application/pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public void MergePDFs(string outPutFilePath, params string[] filesPath)
        {
            try
            {
                List<PdfReader> readerList = new List<PdfReader>();
                foreach (string filePath in filesPath)
                {
                    PdfReader pdfReader = new PdfReader(filePath);
                    readerList.Add(pdfReader);
                }

                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0f, 0f, 0f, 20f);
                iTextSharp.text.pdf.PdfCopy writer = new iTextSharp.text.pdf.PdfCopy(document, new System.IO.FileStream(outPutFilePath, System.IO.FileMode.Create));

                document.Open();
                foreach (PdfReader reader in readerList)
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        document.Add(iTextSharp.text.Image.GetInstance(page));
                    }

                    writer.AddDocument(reader);
                    reader.Close();
                }
                document.Close();
                writer.Close();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}