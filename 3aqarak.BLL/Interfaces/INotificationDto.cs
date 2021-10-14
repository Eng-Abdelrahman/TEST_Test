using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface INotificationDto
    {
        List<PostponedCallDto> Calls { get; set; }
        List<FellowCallDto> FellowupCalls { get; set; }
        List<ExpectedContractDto> ExpectedContracts { get; set; }
        List<PreviewHeaderDto> Previews { get; set; }
        List<RentHeaderDto> EndedContracts { get; set; }
        List<RentHeaderDto> RentalsToCollect { get; set; }
        List<SaleHeaderDto> SaleToCollect { get; set; }
    }
}
