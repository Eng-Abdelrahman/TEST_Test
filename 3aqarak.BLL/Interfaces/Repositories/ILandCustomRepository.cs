using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces.Repositories
{
    public interface ILandCustomRepository
    {
        Task<List<tbl_LandsDemands>> AvailablesForMasters(tbl_AvailableLands available);
        Task<List<tbl_LandsDemands>> AvailablesForPayment(tbl_AvailableLands available);
        Task<List<DemandsWithPreviews>> FilterDemandsHasPreviews(List<LandsDemandsDto> empDemands, List<LandsDemandsDto> colleguesDemands, AvailableLandsDto available);
    }
}
