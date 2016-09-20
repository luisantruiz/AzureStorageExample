using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace AzureStorage.Models
{
    public class Task : TableEntity
    {
        public string Description
        {
            get
            {
                return PartitionKey;
            }
            set
            {
                PartitionKey = value;
            }
        }

        public string CreatedDate
        {
            get
            {
                return RowKey;
            }
            set
            {
                RowKey = value;
            }
        }
    }
}