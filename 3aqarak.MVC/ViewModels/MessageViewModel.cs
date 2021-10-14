using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.MVC.ViewModels
{
    public class MessageViewModel
    {
        public int PK_Messages_Id { get; set; }

        public int FK_Messages_Users_SenderId { get; set; }

        public int FK_Messages_Users_RecieverId { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeEnd { get; set; }

        public string MessageContent { get; set; }

        public bool IsRead { get; set; }

        public bool IsDone { get; set; }

        public bool IsCritical { get; set; }

        public string Title { get; set; }
        //add date srting to get data in Load data ********
        public string DateString { get; set; }
        public SelectList User { get; set; }
        public string DateTimeStartString { get; set; }

        public string DateTimeEndString { get; set; }

        public string UserName { get; set; }
    }
}