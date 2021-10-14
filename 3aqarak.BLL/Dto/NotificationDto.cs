using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Dto
{
    public class NotificationDto : INotificationDto
    {
        public List<PreviewHeaderDto> Previews { get; set; }
        public List<PostponedCallDto> Calls { get; set; }
        public List<ExpectedContractDto> ExpectedContracts { get; set; }
        public List<RentHeaderDto> EndedContracts { get; set; }
        public List<RentHeaderDto> RentalsToCollect { get; set; }
        public List<SaleHeaderDto> SaleToCollect { get; set; }
        public List<FellowCallDto> FellowupCalls { get; set; }

        public NotificationDto()
        {
            Previews = new List<PreviewHeaderDto>();
            Calls = new List<PostponedCallDto>();
            ExpectedContracts = new List<ExpectedContractDto>();
            EndedContracts = new List<RentHeaderDto>();
            RentalsToCollect = new List<RentHeaderDto>();
            SaleToCollect = new List<SaleHeaderDto>();
            FellowupCalls = new List<FellowCallDto>();
        }
    }
}
