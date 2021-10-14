using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.Areas.Dashboard.ViewModels
{
    public class EventLogViewModel
    {
        public int PK_Event_Id { get; set; }

        public string FK_Event_Users_Id { get; set; }

        public string UserName { get; set; }

        public string TableName { get; set; }

        public string EventType { get; set; }

        public DateTime Date { get; set; }
        //add date srting to get data in Load data ********
        public string DateString { get; set; }
        public string Description { get; set; }
    }
}