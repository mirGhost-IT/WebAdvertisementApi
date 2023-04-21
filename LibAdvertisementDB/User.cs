using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibAdvertisementDB
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public List<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
    }
}
