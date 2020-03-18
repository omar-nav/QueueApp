using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace QueueApp
{
  class Program
  {
    // use key vault instead
    private const string ConnectionString = "";
    static void Main(string[] args)
    {
      Console.WriteLine("Holi Mundo!");
    }
    static async Task SendArticleAsync(string newsMessage)
    {
      CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

      CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

      CloudQueue queue = queueClient.GetQueueReference("newsqueue");
      bool createdQueue = await queue.CreateIfNotExistsAsync();
      if (createdQueue)
      {
        Console.WriteLine("The queue of news articles was created.");
      }

      CloudQueueMessage articleMessage = new CloudQueueMessage(newsMessage);
      await queue.AddMessageAsync(articleMessage);
    }
  }
}
