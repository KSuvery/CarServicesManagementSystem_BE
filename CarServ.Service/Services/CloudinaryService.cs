using CarServ.service.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using CarServ.service.Services.Exceptions;
using CarServ.service.Services.Configuration;

namespace CarServ.service.Services
{
    public class CloudinaryService(IServiceProvider serviceProvider) : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary = new Cloudinary(CloudinarySetting.Instance.CloudinaryUrl);
        private readonly ILogger _logger = serviceProvider.GetRequiredService<ILogger>();

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            _logger.Information("Uploading image to Cloudinary");
            ImageValidate(file);
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Quality(80).Crop("fit")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
        }

        public Task DeleteImageAsync(string imageUrl)
        {
            _logger.Information($"Deleting image from Cloudinary. Url: {imageUrl}");
            var publicId = UrlValidate(imageUrl);
            var deleteParams = new DeletionParams(publicId);
            return _cloudinary.DestroyAsync(deleteParams);
        }

        private void ImageValidate(IFormFile file)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".jfif" };
            
            if (file.Length > 5 * 1024 * 1024) // 5 MB
            {
                throw new ArgumentException("File size exceeds the maximum limit of 5 MB.");
            }

            if (file.Length == 0 || file == null)
            {
                throw new ArgumentException("File is empty.");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!validExtensions.Contains(fileExtension))
            {
                throw new ArgumentException($"Invalid file type. Supported types are: {string.Join(", ", validExtensions)}");
            }
        }

        private string UrlValidate(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentException("Image URL cannot be null or empty.", nameof(imageUrl));
            }

            // Parse the URL
            var uri = new Uri(imageUrl);

            // Validate hostname
            var host = "res.cloudinary.com";
            if (!uri.Host.Equals(host, StringComparison.OrdinalIgnoreCase))
            {
                throw new AppException("Bad Request!",
                    "Đường dẫn hình ảnh không hợp lệ. " + $" Host must be {host}",
                    StatusCodes.Status400BadRequest);
            }

            // Validate that the cloud name matches (first segment in the path)
            var cloudName = CloudinarySetting.Instance.CloudinaryUrl.Split("@").Last();
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length < 2 || !segments[0].Equals(cloudName, StringComparison.OrdinalIgnoreCase))
            {
                throw new AppException("Bad Request!",
                    "Đường dẫn hình ảnh không hợp lệ. " + $" CloudName required: {cloudName}",
                                                                 StatusCodes.Status400BadRequest);
            }

            // Validate path structure (e.g., /image/upload/v1234567890/<public-id>)
            if (segments.Length < 4 || segments[1] != "image" || segments[2] != "upload")
            {
                throw new AppException("Bad Request!",
                    "Đường dẫn hình ảnh không hợp lệ. " + " (path structure)",
                    StatusCodes.Status400BadRequest);
            }

            // Extract and validate public ID (last segment without extension)
            var publicIdWithExtension = segments[^1];
            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);

            // Ensure public ID is not empty and matches a safe pattern
            var isValidPublicId = !string.IsNullOrEmpty(publicId) &&
                                  Regex.IsMatch(publicId, @"^[a-zA-Z0-9_\-]+$");

            if (!isValidPublicId)
            {
                throw new AppException("Bad Request!",
                    "Đường dẫn hình ảnh không hợp lệ. " + " (public ID)",
              StatusCodes.Status400BadRequest);
            }

            return publicId;
        }
    }
}
