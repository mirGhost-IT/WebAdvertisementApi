using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAdvertisementApi.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebAdvertisementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EditAdvertisementController : ControllerBase
    {
        private readonly AdvertisementContext _db;
        private readonly ILogger<EditAdvertisementController> _logger;

        public EditAdvertisementController(ILogger<EditAdvertisementController> logger, AdvertisementContext db)
        {
            _logger = logger;
            _db = db;
        }
        /// <summary>
        /// Изменение объявления
        /// </summary>
        /// <param name="editAdvertisement">Модель объявления для изменения в формате JSON</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPut("Edit")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(EditAdvertisement editAdvertisement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime dateTimeUtc = editAdvertisement.Created.ToUniversalTime();
            editAdvertisement.Created = dateTimeUtc;

            dateTimeUtc = editAdvertisement.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            editAdvertisement.ExpirationDate = expirationDate;
            var adv = await _db.Advertisements
                .Include(i => i.User)
                .Include(i => i.Image)
                .FirstOrDefaultAsync(i=>i.Id == editAdvertisement.Id);

            _db.Advertisements.Remove(adv);

            var user = await _db.Users.FirstOrDefaultAsync(i => i.Id == editAdvertisement.UserId);

            Advertisement advertisement = new Advertisement
            {
                Text = editAdvertisement.Text,
                Number = editAdvertisement.Number,
                Rating = editAdvertisement.Rating,
                UserId = editAdvertisement.UserId,
                User = user,
                Created = editAdvertisement.Created,
                ExpirationDate = editAdvertisement.ExpirationDate
            };

            await _db.Advertisements.AddAsync(advertisement);

            LibAdvertisementDB.Image img = new LibAdvertisementDB.Image
            {
                Img = editAdvertisement.Img,
                Name = editAdvertisement.NameImg,
                AdvertisementId = advertisement.Id,
                Advertisement = advertisement
            };

            img.AdvertisementId = advertisement.Id;
            await _db.Images.AddAsync(img);

            await _db.SaveChangesAsync();

            return Ok(new { Message = "Edit successfully" });
        }
    }
}
