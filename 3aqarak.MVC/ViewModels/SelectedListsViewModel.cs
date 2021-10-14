using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class SelectedListsViewModel
    {
        public SelectList RegionsFrom { get; set; }

        public MultiSelectList Views { get; set; }

        public MultiSelectList Finishings { get; set; }

        public SelectList Payments { get; set; }

        public SelectList Usages { get; set; }

        public SelectList Transactions { get; set; }
    }
}