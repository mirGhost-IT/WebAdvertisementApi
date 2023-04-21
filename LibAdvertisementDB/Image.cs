using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAdvertisementDB
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Img { get; set; }
        public Guid AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
    }
}
