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
    public interface IRentContractService
    {
        Task<bool> DeleteContract(int id, int userId);
        Task<bool> DeleteRelatedCompCommission(int id);
        Task<bool> DeleteRelatedEmpCommission(int id);
        Task<bool> EndRentContract(RentHeaderDto header);
        Task<RentDetailsDto> FindDetailByID(int id);
        Task<RentHeaderDto> FindHeaderByID(int id);
        Task<SelectList> GetCats();
        Task<List<RentHeaderDto>> GetContracts(DateTime? from, DateTime? to, int catId);
        Task<SelectList> GetPaymentBasis();
        Task<SelectList> GetPaymentBasisId(int id);
        Task<IConfirmation> SaveContract(RentHeaderDto contract, RentDetailsDto details, int userId);
        IConfirmation SavePhotos(HttpFileCollectionBase files);
        Task<bool> UpdateContract(RentHeaderDto contract, RentDetailsDto details, int userId);
        Task<bool> UpdateNextRent(RentHeaderDto nextRent, DateTime newDate, int userId);
        IConfirmation ValidatePhotos(HttpFileCollectionBase files);
    }
}
