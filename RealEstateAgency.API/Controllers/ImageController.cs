using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Services;

namespace RealEstateAgency.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController(IImageService imageService, IPropertyService propertyService) : ControllerBase
{
    // [HttpPost("upload")]
    // public async Task<IActionResult> Upload([FromBody] IFormFile file)
    // {
    //     if (file.Length == 0)
    //     {
    //         return BadRequest("No file uploaded");
    //     }
    //     
    //     await using var ms = new MemoryStream();
    //     await file.CopyToAsync(ms);
    //     var bytes = ms.ToArray();
    //     await imageService.InsertAsync(bytes);
    //     
    //     return Ok();
    // }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetByStatementId(Guid id)
    {
        var res = await propertyService.GetBytesByPropertyIdAsync(id);
        return File(res, "image/jpeg"); 
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file) {
        var result = await imageService.UploadImageAsync(file);

        if (result.Error != null) {
            return BadRequest(result.Error.Message);
        }

        // Возвращаем объект с URL и PublicId для сохранения в базу
        return Ok(new { 
            Url = result.SecureUrl.ToString(), 
            PublicId = result.PublicId 
        });
    }
}