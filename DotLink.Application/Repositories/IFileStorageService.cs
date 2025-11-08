using System.IO;

namespace DotLink.Application.Services
{
	public interface IFileStorageService
	{
		Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string containerName);

		Task DeleteFileAsync(string fileUrl);

		Task<Stream> GetFileStreamAsync(string fileKey);
	}
}