using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Images;
using NZWalks.API.Repositories.Images;

namespace NZWalks.API.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImagesController(IImageRepository imageRepository) : ControllerBase {
        private readonly IImageRepository imageRepository = imageRepository;

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] UploadImageDto request) {
            ValidateFileUpload(request);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Image imageModel = new() {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileName = request.File.FileName,
                FileDescription = request.FileDescription,
            };

            Image? uploadedImage = await imageRepository.Upload(imageModel);

            if (uploadedImage == null) return BadRequest(ModelState);

            return Ok(uploadedImage);
        }

        private void ValidateFileUpload(UploadImageDto request) {
            string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName))) {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request.File.Length > 10485760) {
                ModelState.AddModelError("file", "File Size maximum is 10MB");
            }
        }
    }
}