﻿using LibAdvertisementDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBusinessLogic.Interface
{
    public interface IAdvertisementInteraction
    {
        Task Add(Advertisement addAdvertisement);
        Task Remove(Advertisement addAdvertisement, string imagePhysicalPath);
    }
}
