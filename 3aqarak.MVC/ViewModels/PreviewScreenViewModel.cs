using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class PreviewScreenViewModel
    {
        public PreviewHeaderViewModel Header { get; set; }

        public List<PreviewDetailViewModel> Details { get; set; }

    }
}