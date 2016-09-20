using System;

namespace AzureStorage.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}