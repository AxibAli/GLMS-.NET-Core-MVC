using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace GLMSMiddlewareAPI.Controllers
{
    //[AuthorizeAuthenticate]
    public class LogViewerController : Controller
    {
        // GET: LogViewer
        public ActionResult Index()
        {
            //Fetch all files in the Folder (Directory).

            var files = getFilesList();
            return View(files);
        }

        [NonAction]
        public List<FileModel> getFilesList(string todaysDate = null)
        {
            if (string.IsNullOrEmpty(todaysDate))
            {
                todaysDate = DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                todaysDate = Convert.ToDateTime(todaysDate).ToString("yyyyMMdd");
            }
            string LogPath = LogFile.Parentfolder;
            List<FileModel> files = new List<FileModel>();
            var path = $@"{LogPath}\{todaysDate}\";
            if (!Directory.Exists(path))
            {
                return files;
            }



            string[] filePaths = Directory.GetDirectories($@"{LogPath}\{todaysDate}\");
            string[] subdirectories;
            FileInfo fileInfo = null;

            //Copy File names to Model collection.
            List<FileModel> filesSubDirectories = new List<FileModel>();
            foreach (string filePath in filePaths)
            {
                subdirectories = Directory.GetFiles(filePath);
                fileInfo = new FileInfo(filePath);
                fileInfo.Refresh();
                foreach (string filePathSubDirectory in subdirectories)
                {
                    filesSubDirectories.Add(new FileModel { FileName = Path.GetFileName(filePathSubDirectory), FilePath = filePathSubDirectory, FileFullName = Path.GetFileNameWithoutExtension(filePathSubDirectory), FileModificationDate = fileInfo.LastWriteTime });
                }

                files.Add(new FileModel { FileName = Path.GetFileName(filePath), FilePath = filePath, NoOfFiles = filesSubDirectories.ToList() });
                filesSubDirectories.Clear();
            }
            return files;
        }



        public ActionResult ViewLog(string filePath)
        {
            var fileContents = System.IO.File.ReadAllText(filePath);
            return Json(new { Status = true, View = fileContents }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFiles(string todaysDate)
        {
            var files = getFilesList(todaysDate);
            string message = "";
            bool Response = true;
            var partialView = "";
            if (files.Count <= 0)
            {
                Response = false;
                message = "No Record Found";
                partialView = "";
            }
            else
            {
                partialView = Utility.RenderPartialToString(this, "~/Views/LogViewer/PartialView/_Index.cshtml", files);
            }

            return Json(new { Response, View = partialView, message }, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            //string path = Server.MapPath("~/Files/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(fileName);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

    }

    public class FileModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileFullName { get; set; }
        public DateTime FileModificationDate { get; set; }
        public List<FileModel> NoOfFiles { get; set; }
    }
}