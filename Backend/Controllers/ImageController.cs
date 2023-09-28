using JWT_Token_Example.ImageServices;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;

public class ImageController : ControllerBase
{
    private S3Service? S3service;
    private readonly IAWSConfiguration appConfiguration;
    
    public ImageController(IAWSConfiguration appConfig)
    {

        this.appConfiguration = appConfig;
        //this.S3service = s3ser;
    }


    [HttpPost]
    [Route("uploadImg")]
    public IActionResult UploadDocumentToS3([FromBody]IFormFile file)
    {
        try
        {
            if (file is null || file.Length <= 0)
                return BadRequest("File is empty");


            S3service = new S3Service(appConfiguration.AwsAccessKey, appConfiguration.AwsSecretAccessKey, appConfiguration.Region, appConfiguration.BucketName);
            var result = S3service.UploadFile(file);

            return Ok("File uploaded");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("{folder}/{imageName}")]
    public IActionResult GetDocumentFromS3(string folder, string imageName)
    {
        try
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(imageName))
                return BadRequest("Folder and image name parameters are required");

            S3service = new S3Service(appConfiguration.AwsAccessKey, appConfiguration.AwsSecretAccessKey, appConfiguration.Region, appConfiguration.BucketName);

            // Combine the folder and image name to create the full S3 object key
            var objectKey = $"{folder}/{imageName}";

            var document = S3service.DownloadFileAsync(objectKey).Result;

            return File(document, "application/octet-stream", imageName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}