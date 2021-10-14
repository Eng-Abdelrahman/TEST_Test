using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3aqarak.MVC.ViewModels
{
    public class NotificationViewModel
    {
        public List<PreviewHeaderViewModel> Previews { get; set; }
        public List<ClientCallViewModel> Calls { get; set; }
        public List<ExpectedContractViewModel> ExpectedContracts { get; set; }
    }
}