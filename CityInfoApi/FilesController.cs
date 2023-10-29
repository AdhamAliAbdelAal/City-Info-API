using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfoApi;
[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private FileExtensionContentTypeProvider _fileExtensionContentTypeProvider { get; }

    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetFile(string id)
    {
        // file class is defined in ControllerBase class
        var filePath = "creating-the-api-and-returning-resources-slides.pdf";
        if (!System.IO.File.Exists(filePath)){
            return NotFound();
        }
        if (!_fileExtensionContentTypeProvider.TryGetContentType(filePath,out var contentType))
        {
            contentType = "application/octet-stream";
        }
        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, contentType, "kokowawa.pdf");
    }
}