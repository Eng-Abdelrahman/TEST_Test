using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface ISaleContractService
    {
        Task<SelectList> GetCats();
        Task<List<SaleHeaderDto>> GetContracts(DateTime? from, DateTime? to,int catId);
        Task<SaleHeaderDto> FindHeaderByID(int id);
        Task<SaleDetailsDto> FindDetailByID(int id);
        Task<bool> UpdateContract(SaleHeaderDto Contract, SaleDetailsDto details, int userId);
        Task<IConfirmation> SaveContract(SaleHeaderDto Contract, SaleDetailsDto details, int userId);
        Task<bool> DeleteContract(int id, int userId);
        Task<SelectList> GetPaymentBasis();
        Task<SelectList> GetPaymentBasisId(int id);
        //hussainy
        IConfirmation SavePhotos(HttpFileCollectionBase files);
        IConfirmation ValidatePhotos(HttpFileCollectionBase file);
        Task<bool> DeleteRelatedCompCommission(int id);
        Task<bool> DeleteRelatedEmpCommission(int id);
        Task<bool> UpdateNextIntstall(SaleHeaderDto nextSaleFee, int nextAmount, DateTime newDate, int userId,int nextInstallValue);
    }
}
