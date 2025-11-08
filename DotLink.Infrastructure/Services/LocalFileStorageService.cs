using DotLink.Application.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _localStoragePath;
        public LocalFileStorageService(string localStoragePath) { 
            _localStoragePath = localStoragePath;
        }

        public async Task<string> UploadFileAsync(
            Stream fileStream, 
            string fileName, 
            string contentType, 
            string containerName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            var objectKey = $"{containerName}/{uniqueFileName}";

            var containerPath = Path.Combine(_localStoragePath, containerName);
            if (!Directory.Exists(containerPath))
            {
                Directory.CreateDirectory(containerPath);
            }

            var filePath = Path.Combine(containerPath, uniqueFileName);

            using (var file = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(file);
            }

            return objectKey;
        }

        public Task DeleteFileAsync(string fileKey)
        {
            if (string.IsNullOrWhiteSpace(fileKey)) return Task.CompletedTask;

            var filePath = Path.Combine(_localStoragePath, fileKey);


            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public Task<Stream> GetFileStreamAsync(string fileKey)
        {
            var filePath = Path.Combine(_localStoragePath, fileKey);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found on local storage.", filePath);
            }

            return Task.FromResult<Stream>(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }
    }
}
