﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Helpers
{
    public class DemandsWithPreviews
    {
        public int HeaderId { get; set; }
        public DateTime PreviewDate { get; set; }
        public int DemandId { get; set; }
        public int BuyerId { get; set; }
        public int AvailableId { get; set; }
        public int Seller { get; set; }
        public int DetailId { get; set; }
    }
}