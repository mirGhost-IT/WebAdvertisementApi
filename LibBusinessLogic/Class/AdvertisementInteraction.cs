using LibAdvertisementDB;
using LibBusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBusinessLogic.Class
{
    public class AdvertisementInteraction : IAdvertisementInteraction
    {
        private readonly AdvertisementContext _db;
        public AdvertisementInteraction(AdvertisementContext db)
        {
            _db = db;
        }
        public async Task Add(Advertisement advertisement)
        {
            await _db.Advertisements.AddAsync(advertisement);
            await _db.SaveChangesAsync();
        }

        public async Task Edit(Advertisement OldAdvertisement, Advertisement NewAdvertisement)
        {
            _db.Advertisements.Remove(OldAdvertisement);
            await _db.Advertisements.AddAsync(NewAdvertisement);
            await _db.SaveChangesAsync();
        }

        public async Task Remove(Advertisement advertisement, string imagePhysicalPath)
        {     
            File.Delete(imagePhysicalPath);
            _db.Remove(advertisement);
            await _db.SaveChangesAsync();
        }
    }
}
