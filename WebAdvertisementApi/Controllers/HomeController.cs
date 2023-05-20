using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Linq;
using System.Reflection;
using System.Text;
using WebAdvertisementApi.Models;
using static System.Net.Mime.MediaTypeNames;
using LibBusinessLogic.Interface;
using Microsoft.Extensions.Caching.Distributed;
using LibBusinessLogic.Class;

namespace WebAdvertisementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        IInfo _info;
        IAdvertisementInteraction _advertisementInteraction;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        AdvertisementContext _db;

        public HomeController(ILogger<HomeController> logger,
            IAdvertisementInteraction advertisementInteraction,
            IInfo info, 
            AdvertisementContext db, 
            IWebHostEnvironment environment)
        {
            _db = db;
            _advertisementInteraction = advertisementInteraction;
            _info = info;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Получить список объявлений
        /// </summary>
        /// <returns>Возвращет все объявления из бд</returns>
        [HttpGet("GetAdvertisements")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AllAdvertisements()
        {
            var adv = await _info.AllAdvertisements();
            return Ok(adv);
        }

        /// <summary>
        /// Получить список объявлений мультисортировкой 
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <param name="orderByQueryString">Список сортировок</param>
        /// <returns>Возврашает отсартированный список объявлений </returns>
        [HttpGet("GetMultiSort")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MultiSort(string? search, string? orderByQueryString, DateTime? startDate, DateTime? endDate)
        {
            var adv = await _db.Advertisements
            .Include(i => i.User)
            .MultiSort(search, orderByQueryString, startDate, endDate);
            if (adv.Count() == 0)
            {
                return NotFound();
            }

            return Ok(adv);
        }     


        /// <summary>
        /// Удаляет объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления</param>
        /// <returns>Строку подтверждения успеха</returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {

            var delete = await _info.InfoAdvertisement(id);
            if (delete == null)
            {
                return NotFound();
            }
            var imagePath = delete.ImageUrl;
            var imagePhysicalPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
            if (!System.IO.File.Exists(imagePhysicalPath))
            {
                return NotFound();
            }

            await _advertisementInteraction.Remove(delete, imagePhysicalPath);

            return Ok(new { Message = "Delete successfully" });
        }

        /// <summary>
        /// Удаляет объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления в формате JSON</param>
        /// <returns>Строку подтверждения успеха</returns>
        [HttpDelete("DeleteJSON")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteJSON([FromBody] Guid id) => await Delete(id);

        /// <summary>
        /// Изменение объявления
        /// </summary>
        /// <param name="editAdvertisement">Модель объявления для изменения в формате FromForm</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPut("Edit")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit([FromForm] EditAdvertisement editAdvertisement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(editAdvertisement.ImageUrl.FileName);
            string filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await editAdvertisement.ImageUrl.CopyToAsync(stream);
            }

            string imageUrl = "/images/" + fileName;

            DateTime dateTimeUtc = editAdvertisement.Created.ToUniversalTime();
            editAdvertisement.Created = dateTimeUtc;

            dateTimeUtc = editAdvertisement.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            editAdvertisement.ExpirationDate = expirationDate;

            Advertisement advertisement = await _info.InfoAdvertisement(editAdvertisement.Id);
            if (advertisement == null)
            {
                return NotFound();
            }

            advertisement.Text = editAdvertisement.Text;
            advertisement.ImageUrl = imageUrl;
            advertisement.Rating = editAdvertisement.Rating;
            advertisement.ExpirationDate = editAdvertisement.ExpirationDate;

            await _db.SaveChangesAsync();

            return Ok(new { Message = "Edit successfully" });
        }

        /// <summary>
        /// Добовляет новое объявление
        /// </summary>
        /// <param name="addAdvertisement">Модель объявления для добавления в формате FromForm</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add([FromForm] AddAdvertisement addAdvertisement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(addAdvertisement.ImageUrl.FileName);
            string filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await addAdvertisement.ImageUrl.CopyToAsync(stream);
            }

            string imageUrl = "/images/" + fileName;

            var user = await _info.GetUser(addAdvertisement.UserId);
            if (user == null)
            {
                return NotFound();
            }

            Advertisement advertisement = new Advertisement
            {
                Text = addAdvertisement.Text,
                Number = addAdvertisement.Number,
                Rating = addAdvertisement.Rating,
                UserId = addAdvertisement.UserId,
                ImageUrl = imageUrl,
                User = user
            };
            DateTime dateTimeUnspecified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            DateTime dateTimeUtc = dateTimeUnspecified.ToUniversalTime();
            advertisement.Created = dateTimeUtc;

            dateTimeUtc = addAdvertisement.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            advertisement.ExpirationDate = expirationDate;

            await _advertisementInteraction.Add(advertisement);

            return Ok(new { Message = "Add successfully" });
        }

        /// <summary>
        /// Получает 1 объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления</param>
        /// <returns>Возвращает 1 объявление по id из бд</returns>
        [HttpGet("GetInfo")]
        [ProducesResponseType(typeof(Advertisement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Info(Guid id)
        {
            var res = await _info.InfoAdvertisement(id);
            return Ok(res);
        }

        /// <summary>
        /// Получает 1 объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления в формате JSON</param>
        /// <returns>Возвращает 1 объявление по id из бд</returns>
        [HttpPost("GetInfoJSON")]
        [ProducesResponseType(typeof(Advertisement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InfoJSON([FromBody] Guid id) => await Info(id);
    }
}


