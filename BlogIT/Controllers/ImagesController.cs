using BlogIT.Models.Domain;
using BlogIT.Models.Dto;
using BlogIT.Repositories.Implementation;
using BlogIT.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageInterface _imageInterface;

        public ImagesController(IImageInterface imageInterface)
        {
            _imageInterface = imageInterface;
        }


        //GET {apibaseurl}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            //call image repository to get all images
            var images = await _imageInterface.GetAll();

            //Convert Domain Model To DTO
            var response = new List<BlogImageDto>();
            foreach (var image in images)
            {
                response.Add(new BlogImageDto
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url
                });
            }

            return Ok(response);
        }




        //POST: {apibaseurl}/api/images

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage(IFormFile file, string fileName, string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                //fileUpload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };

                blogImage = await _imageInterface.Upload(file, blogImage);

                //Convert Domain model to DTO
                var response = new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url

                };

                return Ok(response);

            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 1048760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
    }
}

