
using Data;
using System.Web.Mvc;
using Web.Models;
using System.Linq;
using System;
using System.Web;
using System.IO;
using System.Data;
using LumenWorks.Framework.IO.Csv;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
//using System.Web.UI.WebControls;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly Services.Administration.IAdministrationService _administrationService;
        //public HomeController(Services.Administration.IAdministrationService administrationService)
        //{
        //    _administrationService = administrationService;
        //}
        public HomeController()
        {

        }
        private static DataTable ProcessCSV(string fileName)
        {
            //Set up our variables
            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            // work out where we should split on comma, but not in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Set the filename in to our stream
            StreamReader sr = new StreamReader(fileName);

            //Read the first line and split the string at , with our regular expression in to an array
            line = sr.ReadLine();
            strArray = r.Split(line);

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

            //Read each line in the CVS file until it’s empty
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();

                //add our current value to our data row
                row.ItemArray = r.Split(line);
                dt.Rows.Add(row);
            }

            //Tidy Streameader up
            sr.Dispose();

            //return a the new DataTable
            return dt;

        }
        private static String ProcessBulkCopy(DataTable dt)
        {
            string Feedback = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            //make our connection and dispose at the end
            using (SqlConnection conn = new SqlConnection(connString))
            {
                //make our command and dispose at the end
                using (var copy = new SqlBulkCopy(conn))
                {

                    //Open our connection
                    conn.Open();

                    ///Set target table and tell the number of rows
                    copy.DestinationTableName = "JournalEntryLine";
                    copy.BatchSize = dt.Rows.Count;
                    try
                    {
                        //Send it to the server
                        copy.WriteToServer(dt);
                        Feedback = "Upload complete";
                    }
                    catch (Exception ex)
                    {
                        Feedback = ex.Message;
                    }
                }
            }

            return Feedback;
        }

        [Audit]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            //ViewBag.Message = "Your application description page.";

            return View();
        }
        [Audit]
        public ActionResult Home()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //if (_administrationService.GetDefaultCompany() == null)
            //    Data.DbInitializerHelper.Initialize();

            return View();

        }
        public ActionResult AddBatchUpload()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddBatchUpload(HttpPostedFileBase upload)
        //public ActionResult AddBatchUpload(HttpPostedFileBase FileUpload)
        //{

        //    // Set up DataTable place holder
        //    DataTable dt = new DataTable();

        //    //check we have a file
        //    if (FileUpload.ContentLength > 0)
        //    {
        //        //Workout our file path
        //        string fileName = Path.GetFileName(FileUpload.FileName);
        //        string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

        //        //Try and upload
        //        try
        //        {
        //            FileUpload.SaveAs(path);
        //            //Process the CSV file and capture the results to our DataTable place holder
        //            dt = ProcessCSV(path);

        //            //Process the DataTable and capture the results to our SQL Bulk copy
        //            ViewData["Feedback"] = ProcessBulkCopy(dt);
        //        }
        //        catch (Exception ex)
        //        {
        //            //Catch errors
        //            ViewData["Feedback"] = ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        //Catch errors
        //        ViewData["Feedback"] = "Please select a file";
        //    }

        //    //Tidy up
        //    dt.Dispose();

        //    return View("Index", ViewData["Feedback"]);
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {

                    if (upload.FileName.EndsWith(".csv"))
                    {
                        // Set up DataTable place holder
                        // DataTable dt = new DataTable();
                        var fileName = Path.GetFileName(upload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                        upload.SaveAs(path);
                        //Process the CSV file and capture the results to our DataTable place holder
                         //dt = ProcessCSV(path);
                        //Try and upload
                        //        try
                        //        {
                        //            FileUpload.SaveAs(path);
                        //            

                        //            //Process the DataTable and capture the results to our SQL Bulk copy
                        //            ViewData["Feedback"] = ProcessBulkCopy(dt);
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            //Catch errors
                        //            ViewData["Feedback"] = ex.Message;
                        //        }
                        ModelState.Clear();
                        Stream stream = upload.InputStream;
                        DataTable csvTable = new DataTable();
                        using (CsvReader csvReader =
                            new CsvReader(new StreamReader(stream), true))
                        {
                            csvTable.Load(csvReader);

                            //Process the DataTable and capture the results to our SQL Bulk copy
                            //ViewData["Feedback"] = ProcessBulkCopy(csvTable);
                            
                        }
                        return View(csvTable);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View();
        }
        //public ActionResult AddBatchUpload()
        //{
        //    return View();
        //}
        //[Audit]
        //[HttpPost]
        //public ActionResult AddBatchUpload(HttpPostedFileBase file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (file == null)
        //        {
        //            ModelState.AddModelError("File", "Please Upload Your file");
        //        }
        //        else if (file.ContentLength > 0)
        //        {
        //            int MaxContentLength = 1024 * 1024 * 3; //3 MB
        //            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf",".csv",".xls",".xlsx",".docx" };

        //            if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
        //            {
        //                ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
        //            }

        //            else if (file.ContentLength > MaxContentLength)
        //            {
        //                ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
        //            }
        //            else
        //            {
        //                //TO:DO
        //                var fileName = Path.GetFileName(file.FileName);
        //                var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
        //                file.SaveAs(path);
        //                ModelState.Clear();
        //                ViewBag.Message = "File uploaded successfully";
        //            }
        //        }
        //    }
        //    return View();
        //}

        [Audit]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PopupWIndowTest()
        {
            return PartialView("_PopupWindowTest");
        }
    }

   
}
