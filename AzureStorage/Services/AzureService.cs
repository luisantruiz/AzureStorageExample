using AzureStorage.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AzureStorage.Services
{
    public class AzureService
    {
        #region Public Members

        public void SaveFileToBlob(string fileName, Stream stream)
        {
            GetContainerReference();

            CloudBlockBlob blob = _cloudBlobContainer.GetBlockBlobReference(fileName);
            blob.UploadFromStream(stream);
        }

        public List<string> GetAllBlobsFromContainer()
        {
            GetContainerReference();

            var blobsList = _cloudBlobContainer
                .ListBlobs()
                .Select(x => ((CloudBlockBlob)x).Name)
                .ToList();
            
            return blobsList;
        }

        public void SaveMessageToQueue(string message)
        {
            GetQueueReference();

            var queueMessage = new CloudQueueMessage(message);
            _cloudQueue.AddMessage(queueMessage);
        }

        public List<string> GetAllMessagesFromQueue()
        {
            GetQueueReference();

            var messagesList = new List<string>();

            _cloudQueue.FetchAttributes();
            var amount = _cloudQueue.ApproximateMessageCount;
            if (amount.HasValue && amount.Value > 0)
            {
                amount = amount <= 32 ? amount : 32;
                messagesList = _cloudQueue
                    .GetMessages(amount.Value, TimeSpan.FromSeconds(1))
                    .OrderByDescending(x => x.InsertionTime)
                    .Select(x => x.AsString)
                    .ToList();
            }

            return messagesList;
        }

        public void SaveTaskToTable(Task task)
        {
            GetTableReference();

            task.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            TableOperation insertOperation = TableOperation.Insert(task);

            _cloudTable.Execute(insertOperation);
        }

        public List<Task> GetAllTasksFromTable()
        {
            var tasksList = new List<Task>();

            GetTableReference();
            TableQuery<Task> query = new TableQuery<Task>();
            tasksList = _cloudTable.ExecuteQuery(query).ToList();

            return tasksList;
        }

        #endregion

        #region Private Members

        private CloudStorageAccount _cloudStorageAccount;

        private CloudBlobClient _cloudBlobClient;
        private CloudQueueClient _cloudQueueClient;
        private CloudTableClient _cloudTableClient;

        private readonly string _containerName;
        private readonly string _queueName;
        private readonly string _tableName;

        private CloudBlobContainer _cloudBlobContainer;
        private CloudQueue _cloudQueue;
        private CloudTable _cloudTable;

        private void CreateCloudBlobClient()
        {
            // Get a blob client
            if (_cloudBlobClient == null)
            {
                _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            }
        }

        private void GetContainerReference()
        {
            CreateCloudBlobClient();

            if (_cloudBlobContainer == null)
            {
                _cloudBlobContainer = _cloudBlobClient.GetContainerReference(_containerName);
                _cloudBlobContainer.CreateIfNotExists();

                _cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
        }

        private void CreateCloudQueueClient()
        {
            // Get a queue client
            if (_cloudQueueClient == null)
            {
                _cloudQueueClient = _cloudStorageAccount.CreateCloudQueueClient();   
            }
        }

        private void GetQueueReference()
        {
            CreateCloudQueueClient();

            if (_cloudQueue == null)
            {
                _cloudQueue = _cloudQueueClient.GetQueueReference(_queueName);
                _cloudQueue.CreateIfNotExists();
            }
        }

        private void CreateCloudTableClient()
        {
            // Get a table client
            if (_cloudTableClient == null)
            {
                _cloudTableClient = _cloudStorageAccount.CreateCloudTableClient();

            }
        }

        private void GetTableReference()
        {
            CreateCloudTableClient();

            if (_cloudTable == null)
            {
                _cloudTable = _cloudTableClient.GetTableReference(_tableName);
                _cloudTable.CreateIfNotExists();
            }
        }

        #endregion

        #region Constructors

        public AzureService()
        {
            _containerName = "storageexampleblobcontainer";
            _queueName = "storageexamplequeue";
            _tableName = "storageexampletable";
            _cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        #endregion
    }
}