using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Dto
{
    public class MessagesDto
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

    }
}
