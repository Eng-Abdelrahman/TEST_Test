using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _3aqarak.BLL.Interfaces
{
    public interface IPreviewService
    {
        Task<IConfirmation> CreatePreview(int buyerId, int[] sellerId, int demandId, int[] availableIds, int userId, List<DateTime> dates, int demanCat, int[] availableCatIds);
        Task<List<PreviewScreenDto>> GetPreviews(PreviewFilter filter = null);
        Task<PreviewScreenDto> GetPreviewDetails(int id);
        Task<PreviewScreenDto> SetPreviewResults(int id);
        Task<bool> SetPreviewDetSatus(int id, string status);
        Task<bool> PostponePreview(int preview, int detail, DateTime newDate, int userId);
        Task<bool> DeletePreview(int id, int userId);
        Task<PreviewHeaderDto> FindById(int id);
        Task<bool> IsPreviewExistsInSameTime(int[] availableIds, List<DateTime> dates, int[] availableCats);
        Task<bool> IsPostponePreviewExistsInSameTime(int previewDetail, DateTime dates);
        Task<List<string>> GetReservationDates(DateTime date, int id, int catId);
        Task<bool> SetHeaderStatus(int id, bool status);
    }
}
