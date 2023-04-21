using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAdvertisementDB
{
    public class User
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле Text обязательно для заполнения")]
        [StringLength(50, ErrorMessage = "Длина поля Name не должна превышать 50 символов")]
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public List<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
    }
}
