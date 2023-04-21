using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAdvertisementDB
{
    public class Image
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле Name обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле Img обязательно для заполнения")]

        public byte[] Img { get; set; }

        [Required(ErrorMessage = "Поле AdvertisementId обязательно для заполнения)]
        public Guid AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }
    }
}
