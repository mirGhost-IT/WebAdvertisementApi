using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAdvertisementApi.Models;

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
        /// <param name="addAdvertisement">Модель объявления для добавления в формате JSON</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add(AddAdvertisement addAdvertisement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime dateTimeUnspecified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            DateTime dateTimeUtc = dateTimeUnspecified.ToUniversalTime();
            addAdvertisement.Created = dateTimeUtc;

            dateTimeUtc = addAdvertisement.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            addAdvertisement.ExpirationDate = expirationDate;

            var user = await _db.Users.FirstOrDefaultAsync(i => i.Id == addAdvertisement.UserId);

            Advertisement advertisement = new Advertisement
            {
                Text = addAdvertisement.Text,
                Number = addAdvertisement.Number,
                Rating = addAdvertisement.Rating,
                UserId = addAdvertisement.UserId,
                User = user,
                Created = addAdvertisement.Created,
                ExpirationDate = addAdvertisement.ExpirationDate
            };

            await _db.Advertisements.AddAsync(advertisement);

            Image img = new Image {Img = addAdvertisement.Img,
                Name = addAdvertisement.NameImg,
                AdvertisementId = advertisement.Id,
                Advertisement = advertisement
            };
            
            img.AdvertisementId = advertisement.Id;

            await _db.Images.AddAsync(img);

            await _db.SaveChangesAsync();

            return Ok(new { Message = "Add successfully" });
        }
    }
}
