using LibAdvertisementDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBusinessLogic.Interface
{
    public interface IInfo
    {
        Task<List<Advertisement>> AllAdvertisements();
        MemoryStream ImageResize(string imagePhysicalPath, int width, int height);
        Task<Advertisement> InfoAdvertisement(Guid id);
        Task<User> GetUser(Guid id);
    }
}
