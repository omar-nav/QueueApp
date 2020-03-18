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
    static async void Main(string[] args)
    {
      if (args.Length > 0)
      {
        string value = String.Join(" ", args);
        SendArticleAsync(value).Wait();
        Console.WriteLine($"Sent: {value}");
      }
      else
      {
        // use Result property from the returning task
        string value = await ReceiveArticleAsync();
        Console.WriteLine($"Received {value}");
      }
    }
    static async Task SendArticleAsync(string newsMessage)
    {
      CloudQueue queue = GetQueue();
      bool createdQueue = await queue.CreateIfNotExistsAsync();
      if (createdQueue)
      {
        Console.WriteLine("The queue of news articles was created.");
      }

      CloudQueueMessage articleMessage = new CloudQueueMessage(newsMessage);
      await queue.AddMessageAsync(articleMessage);
    }
    static async Task<string> ReceiveArticleAsync()
    {
      CloudQueue queue = GetQueue();
      bool exists = await queue.ExistsAsync();
      if (exists)
      {
        CloudQueueMessage retrievedArticle = await queue.GetMessageAsync();
        if (retrievedArticle != null)
        {
          string newsMessage = retrievedArticle.AsString;
          await queue.DeleteMessageAsync(retrievedArticle);
          return newsMessage;
        }
      }

      return "<queue empty or not created>";
    }
    static CloudQueue GetQueue()
    {
      CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
      CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
      return queueClient.GetQueueReference("newsqueue33");
    }
  }
}
