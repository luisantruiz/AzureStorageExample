using AzureStorage.Models;
using AzureStorage.Services;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace AzureStorage.Controllers
{
    /// <summary>
    /// Storage Controller
    /// </summary>
    public class StorageController : Controller
    {
        private AzureService _azureService;

        /// <summary>
        /// Constructor
        /// </summary>
        public StorageController()
        {
            _azureService = new AzureService();
        }

        // GET: Storage/Blobs
        public ActionResult Blobs()
        {
            // List of Blobs
            var blobsList = _azureService.GetAllBlobsFromContainer();

            return View(blobsList);
        }

        // POST: Storage/SaveFile
        [HttpPost]
        public ActionResult SaveFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                _azureService.SaveFileToBlob(fileName, file.InputStream);
            }

            return RedirectToAction("Blobs");
        }

        // GET: Storage/Queues
        public ActionResult Queues()
        {
            // List of messages
            var messagesList = _azureService.GetAllMessagesFromQueue();

            return View(messagesList);
        }

        // POST: Storage/Queues
        [HttpPost]
        public ActionResult Queues(string message)
        {
            _azureService.SaveMessageToQueue(message);

            return RedirectToAction("Queues");
        }

        // GET: Storage/Tables
        public ActionResult Tables()
        {
            // List of Tasks
            var tasksList = _azureService.GetAllTasksFromTable();

            return View(tasksList);
        }

        // POST: Storage/Tables
        [HttpPost]
        public ActionResult Tables(Task task)
        {
            _azureService.SaveTaskToTable(task);

            return RedirectToAction("Tables");
        }
    }
}
