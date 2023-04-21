using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;

namespace WebAdvertisementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddAdvertisementController : ControllerBase
    {
        private readonly AdvertisementContext _db;
        private readonly ILogger<AddAdvertisementController> _logger;

        public AddAdvertisementController(ILogger<AddAdvertisementController> logger, AdvertisementContext db)
        {
            _logger = logger;
            _db = db;
        }
        /// <summary>
        /// Добовляет новое объявление
        /// </summary>
        /// <param name="advertisement">Объявление</param>
        /// <param name="image">Картинка</param>
        /// <param name="userId">Id пользователя, которому принадлежит объявление</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add(Advertisement advertisement, IFormFile image, Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime dateTimeUnspecified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            DateTime dateTimeUtc = dateTimeUnspecified.ToUniversalTime();
            advertisement.Created = dateTimeUtc;

            dateTimeUtc = advertisement.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            advertisement.ExpirationDate = expirationDate;

            advertisement.UserId = userId;
            await _db.Advertisements.AddAsync(advertisement);

            Image img = new Image();
            img.AdvertisementId = advertisement.Id;
            img.Name = image.FileName;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                img.Img = memoryStream.ToArray();
            }
            img.AdvertisementId = advertisement.Id;

            await _db.Images.AddAsync(img);

            await _db.SaveChangesAsync();

            return Ok(new { Message = "Add successfully" });
        }
        /// <summary>
        /// Добовляет новое объявление
        /// </summary>
        /// <param name="advertisement">Объявление в формате JSON</param>
        /// <param name="image">Картинка в формате JSON</param>
        /// <param name="userId">Id пользователя, которому принадлежит объявление в формате JSON</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPost("AddJSON")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddJSON([FromBody]Advertisement advertisement,
            [FromBody] IFormFile image,
            [FromBody] Guid userId) => await Add(advertisement, image, userId);
    }
}
