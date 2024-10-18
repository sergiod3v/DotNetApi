using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.Images {
    public class LocalImageRepository(
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpAccessor,
        NZWalksDbContext dbContext
        ) : IImageRepository {
        private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
        private readonly IHttpContextAccessor httpAccessor = httpAccessor;
        private readonly NZWalksDbContext dbContext = dbContext;

        public async Task<Image> Upload(Image image) {
            string? root = webHostEnvironment.ContentRootPath;
            string localFilePath = Path.Combine(
                root,
                "Images",
                image.FileName
            );

            using FileStream stream = new(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            HttpRequest? requestContext = httpAccessor.HttpContext.Request;
            string urlFilePath = $"{requestContext.Scheme}://{requestContext.Host}{requestContext.Path}/Images/{image.FileName}";

            image.FilePath = urlFilePath;

            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}