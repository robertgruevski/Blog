using Blog.Web.Repositories.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

namespace Blog.Web.Repositories
{
	public class ImageRepository : IImageRepository
	{
		private readonly IConfiguration config;
		private readonly Account account;

		public ImageRepository(IConfiguration config)
		{
			this.config = config;
			account = new Account(
				config.GetSection("Cloudinary")["CloudName"], 
				config.GetSection("Cloudinary")["ApiKey"], 
				config.GetSection("Cloudinary")["ApiSecret"]);
		}
		public async Task<string> UploadAsync(IFormFile file)
		{
			var client = new Cloudinary(account);

			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(file.FileName, file.OpenReadStream()),
				DisplayName = file.FileName
			};

			var uploadResult = await client.UploadAsync(uploadParams);

			if(uploadResult is not null && uploadResult.StatusCode == HttpStatusCode.OK)
			{
				return uploadResult.SecureUrl.ToString();
			}

			return null;
		}
	}
}
