
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class DataTableViewModel
    {

        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnDir { get; set; }
        public string SearchValue { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int RecordsTotal { get; set; }
        public int GradeId { get; set; }
        public int SemId { get; set; }
        public List<ClientViewModel> Clients { get; set; }
        public List<StaticViewModel> Templates { get; set; }
        public List<SpecialViewModel> Specials { get; set; }
        // add new proprety here
        public List<ContractArchiveViewModel> Contract { set; get; }
        //add also EventLog here ******
        public List<EventLogViewModel> EventLog { set; get; }



    }
}