using Azure.Storage.Blobs;
using MusicApi.Models;

namespace MusicApi.Helpers
{
    public static class FileHelper
    {
        
        public static async Task<string>  UploadImage(IFormFile file)
        {
           
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstorageaccountby;AccountKey=NHbvWolrnDgosh7+HvuzYUFkDUIDuTX5srq52OFLvZaVxybGH8sxPAZ2napp7C8UdIgNZHYmygW3+ASt7lXelg==;EndpointSuffix=core.windows.net";
            string containerName = "songscover"; // storage account container name

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName); // dosya adı alınması için 
            var memoryStream = new MemoryStream(); // dosyanın okunması için 
            await file.CopyToAsync(memoryStream); // dosya içeriğini almak için 
            memoryStream.Position = 0; // bellekte konumun 0 olması için 
            await blobClient.UploadAsync(memoryStream);//dosyayı azure yükler

           
            return blobClient.Uri.AbsoluteUri;
        }

        public static async Task<string> UploadFile(IFormFile file)
        {

            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstorageaccountby;AccountKey=NHbvWolrnDgosh7+HvuzYUFkDUIDuTX5srq52OFLvZaVxybGH8sxPAZ2napp7C8UdIgNZHYmygW3+ASt7lXelg==;EndpointSuffix=core.windows.net";
            string containerName = "audiofiles";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);  
            var memoryStream = new MemoryStream();  
            await file.CopyToAsync(memoryStream);  
            memoryStream.Position = 0; 
            await blobClient.UploadAsync(memoryStream);


            return blobClient.Uri.AbsoluteUri;
        }
    }
}
