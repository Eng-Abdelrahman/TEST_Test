using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IAppartmentCustomRepository
    {        //available Methods
        Task<List<tbl_DemandUnits>> DemandsForMultiUsageAndPayment(tbl_AvailableUnits available);
        Task<List<tbl_DemandUnits>> DemandsForUsageAndPayment(tbl_AvailableUnits available);
        //Task<List<tbl_DemandUnits>> DemandsForMultiUsageAnyPayment(tbl_AvailableUnits available);
        //Task<List<tbl_DemandUnits>> DemandsForUsageAnyPayment(tbl_AvailableUnits available);

        //Demand Methods
        Task<List<tbl_AvailableUnits>> AvailablesForPaymentFinish(tbl_DemandUnits demand, int[] codes, int[] finishIds);
        Task<List<tbl_AvailableUnits>> AvailablesForPaymentFinishUsage(tbl_DemandUnits demand, int[] codes, int[] finishIds);
        Task<List<tbl_AvailableUnits>> AvailableMatchesForMaster(tbl_DemandUnits demand, int[] codes);
        Task<List<tbl_AvailableUnits>> AvailableMatchesForUsage(tbl_DemandUnits demand, int[] codes);
        Task<List<tbl_AvailableUnits>> AvailableMatchesForFinish(tbl_DemandUnits demand, int[] codes, int[] finishIds);
        Task<List<tbl_AvailableUnits>> AvailableMatchesForFinishAndUsage(tbl_DemandUnits demand, int[] codes, int[] finishIds);
        Task<List<tbl_AvailableUnits>> AvailableMatchesForPayment(tbl_DemandUnits demand, int[] codes);
        Task<List<tbl_AvailableUnits>> AvailableMatchesForPaymentAndUsage(tbl_DemandUnits demand, int[] codes);
        Task<List<DemandsWithPreviews>> FilterDemandsHasPreviews(List<tbl_DemandUnits> empDemands,List<tbl_DemandUnits> colleguesDemands, AvailableDto available);
        Task<tbl_AvailableUnits> LoadRelatedUnitAsync(int id);


    }
}
