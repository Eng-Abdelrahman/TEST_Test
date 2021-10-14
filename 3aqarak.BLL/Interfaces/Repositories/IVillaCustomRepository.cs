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
    public interface IVillaCustomRepository
    {
        Task<List<tbl_VillasDemands>> DemandsForpayment(tbl_VillasAvailables available);
        Task<List<tbl_VillasDemands>> DemandsForpaymentAndUsage(tbl_VillasAvailables available);
        //Task<List<tbl_VillasDemands>> DemandsForAnypaymentAnyUsage(tbl_VillasAvailables available);
        //Task<List<tbl_VillasDemands>> DemandsForAnypaymentAndUsage(tbl_VillasAvailables available);
        Task<List<tbl_VillasAvailables>> AvailableForPaymentAndFinish(tbl_VillasDemands demand, int[] codes, int[] finishIds);
        Task<List<tbl_VillasAvailables>> AvailableForPaymentFinishUsage(tbl_VillasDemands demand, int[] codes, int[] finishIds);
        Task<List<tbl_VillasAvailables>> AvailableMatches(tbl_VillasDemands demand, int[] codes);
        Task<List<tbl_VillasAvailables>> AvailableMatchesForUsage(tbl_VillasDemands demand, int[] codes);
        Task<List<tbl_VillasAvailables>> AvailableMatchesForFinish(tbl_VillasDemands demand, int[] codes, int[] finishIds);
        Task<List<tbl_VillasAvailables>> AvailableMatchesForFinishAndUsage(tbl_VillasDemands demand, int[] codes, int[] finishIds);
        Task<List<tbl_VillasAvailables>> AvailableMatchesForPayment(tbl_VillasDemands demand, int[] codes);
        Task<List<tbl_VillasAvailables>> AvailableMatchesForPaymentAndUsage(tbl_VillasDemands demand, int[] codes);
        Task<List<DemandsWithPreviews>> FilterDemandsHasPreviews(List<VillasDemandDto> empDemands, List<VillasDemandDto> colleguesDemands, VillasAvailableDto villasAvailableDto);

    }
}
