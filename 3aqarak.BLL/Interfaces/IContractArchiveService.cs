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
    public interface IContractArchiveService
    {
        Task<List<ContractArchiveDto>> GetContractArchives();
        Task<ContractArchiveDto> FindContractArchiveByID(int id);
       // SelectList GetContractById(int id);
        Task<bool> UpdateContractArchive(ContractArchiveDto ContractArchive, int userId);
        Task<bool> SaveContractArchive(ContractArchiveDto ContractArchive, int userId);
        Task<bool> DeleteContractArchive(int id, int userId);
        IConfirmation SavePhotos(HttpFileCollectionBase files);
        IConfirmation DeletePhotos(HttpFileCollectionBase files);
        IConfirmation ValidatePhotos(HttpFileCollectionBase file);
        Task<bool> CheckContractId(string contractId, int id);
    }
}
