using AzureStorage.Models;
using System.Collections.Generic;
using System.IO;

namespace AzureStorage.Services
{
    public class AzureService
    {
        #region Public Members

        public void SaveFileToBlob(string fileName, Stream bytes)
        {
        }

        public List<string> GetAllBlobsFromContainer()
        {
            var blobsList = new List<string>();

            return blobsList;
        }

        public void SaveMessageToQueue(string message)
        {
        }

        public List<string> GetAllMessagesFromQueue()
        {
            var messagesList = new List<string>();

            return messagesList;
        }

        public void SaveTaskToTable(Task task)
        {
        }

        public List<Task> GetAllTasksFromTable()
        {
            var tasksList = new List<Task>();

            return tasksList;
        }

        #endregion

        #region Private Members



        #endregion
    }
}