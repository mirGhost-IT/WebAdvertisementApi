using LibAdvertisementDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBusinessLogic.Interface
{
    public interface IPagination
    {
        void EditCount(int count);
        void EditPage(int page);
        List<Advertisement> SelectionPage(List<Advertisement> advertisements);
    }
}
