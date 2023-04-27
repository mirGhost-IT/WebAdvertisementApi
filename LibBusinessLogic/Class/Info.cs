using LibAdvertisementDB;
using LibBusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBusinessLogic.Class
{
    public class Info : IInfo
    {
        private readonly AdvertisementContext _db;
        public Info(AdvertisementContext db)
        {
            _db = db;
        }
        public async Task<List<Advertisement>> AllAdvertisements()
        {
            var adv = await _db.Advertisements
            .Include(i => i.User)
            .ToListAsync();
            return adv;
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(i => i.Id == id);
            return user;
        }

        public MemoryStream ImageResize(string imagePhysicalPath, int width, int height)
        {
            using (var image = SixLabors.ImageSharp.Image.Load(imagePhysicalPath))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max
                }));

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new JpegEncoder());
                    return ms;
                }
            }
        }

        public async Task<Advertisement> InfoAdvertisement(Guid id)
        {
            var res = await _db.Advertisements
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);
            return res;
        }
    }
}
