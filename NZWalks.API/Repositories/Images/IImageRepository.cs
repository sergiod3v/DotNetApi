using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.Images {
    public interface IImageRepository {
        Task<Image> Upload(Image image);
    }
}