using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    public class PreviewService : IPreviewService
    {
        private readonly IUnitOfWork _uow;
        private IConfirmation _conf;

        public PreviewService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }

        public async Task<IConfirmation> CreatePreview(int buyerId, int[] sellerId, int demandId, int[] availableIds, int userId, List<DateTime> dates, int demanCat, int[] availableCatIds)
        {
            var list = new List<combine>();
            for (int i = 0; i < sellerId.Length; i++)
            {
                list.Add(new combine
                {
                    availalableId = availableIds[i],
                    Date = dates[i].Date,
                    sellerId = sellerId[i],
                    DetailTime = DateTime.Today.Add(new TimeSpan(dates[i].Hour, dates[i].Minute, 00)),
                    availableCatId = availableCatIds[i],
                });
            }

            var distinctDates = dates.Select(d => d.Date).Distinct<DateTime>();

            foreach (var date in distinctDates)
            {
                var newHeader = new tbl_PreviewHeaders()
                {
                    CreatedAt = DateTime.UtcNow.AddMinutes(120),
                    ReviewDate = date,
                    FK_PreviewHeaders_Clients_BuyerId = buyerId,
                    DemandUnit_Id = demandId,
                    FK_PreviewHeaders_Users_CreatedBy = userId,
                    IsSuspended = true,
                    Category_Id = demanCat
                };
                _uow.PreviewRepo.Add(newHeader);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم الحفظ بنجاح!";
                    return _conf;
                }
                var objSameDate = list.Where(l => l.Date == date);

                foreach (var item in objSameDate)
                {
                    if (item.Date == newHeader.ReviewDate)
                    {
                        var newDetail = new tbl_PreviewDetails
                        {
                            AvailableUnits_Id = item.availalableId,
                            Category_Id=item.availableCatId,
                            Fk_PreviewDetails_Clients_SellerId = item.sellerId,
                            Fk_PreviewDetails_PreviewHeaders_Id = newHeader.PK_PreviewHeaders_Id,
                            FK_PreviewDetails_Users_ModidfiedBy = userId,
                            ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                            PreviewTime = item.DetailTime.ToString("hh:mm tt"),
                            PreviewHeaderDate = newHeader.ReviewDate,
                            IsNoDecision = true,
                        };
                        _uow.PreviewDetailRepo.Add(newDetail);
                    }
                }
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم الحفظ بنجاح!";
                    return _conf;
                }

            }
            _conf.Message = "تم الحفظ بنجاح!";
            return _conf;
        }

        public async Task<List<PreviewScreenDto>> GetPreviews(PreviewFilter filter)
        {
            var headers = new List<PreviewHeaderDto>();
            var details = new List<PreviewDetailsDto>();
            var previews = new List<PreviewScreenDto>();
            if (filter.sellerId == null && filter.toDate == null && filter.BuyerId == null && filter.fromDate == null)
            {
                headers = Mapper.Map<List<tbl_PreviewHeaders>, List<PreviewHeaderDto>>((await _uow.PreviewRepo.FindAsync(p => !p.IsDeleted && (p.ReviewDate >= DateTime.Today || (p.ReviewDate < DateTime.Today && p.IsSuspended)))).ToList());
                if (headers == null || !headers.Any())
                {
                    return null;
                }
                foreach (var header in headers)
                {
                    header.BuyerName =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == header.FK_PreviewHeaders_Clients_BuyerId)).FirstOrDefault().Name;
                    var user =(await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == header.FK_PreviewHeaders_Users_CreatedBy)).FirstOrDefault();
                    header.EmpName = user.FirstName + " " + user.LastName;
                    details = Mapper.Map<List<tbl_PreviewDetails>, List<PreviewDetailsDto>>((await _uow.PreviewDetailRepo.FindAsync(d => d.Fk_PreviewDetails_PreviewHeaders_Id == header.PK_PreviewHeaders_Id && !d.IsDeleted)).ToList());

                    if (details.FindAll(d => d.IsCancelled).Count == details.Count)
                    {
                        header.PreviewStatus = PreviewStatus.IsCancelled;
                    }
                    else if (details.FindAll(d => d.IsSucceded).Count == details.Count)
                    {
                        header.PreviewStatus = PreviewStatus.IsSucceded;
                    }
                    else if (details.FindAll(d => d.IsPostponed).Count == details.Count)
                    {
                        header.PreviewStatus = PreviewStatus.IsPostponed;
                    }
                    else if (details.FindAll(d => d.IsRejected).Count == details.Count)
                    {
                        header.PreviewStatus = PreviewStatus.IsRejected;
                    }
                    else if (details.FindAll(d => d.IsNoDecision).Count == details.Count)
                    {
                        header.PreviewStatus = PreviewStatus.IsSuspended;
                    }
                    else if (details.Exists(d => d.IsNoDecision))
                    {
                        header.PreviewStatus = PreviewStatus.IsSuspended;
                    }
                    else if (!details.Exists(d => d.IsCancelled) && !details.Exists(d => d.IsNoDecision) && !details.Exists(d => d.IsPostponed) && !details.Exists(d => d.IsRejected) && !details.Exists(d => d.IsSucceded))
                    {
                        header.PreviewStatus = PreviewStatus.IsUndefined;
                    }
                    var preview = new PreviewScreenDto()
                    {
                        //Details = details,
                        Header = header,
                    };
                    previews.Add(preview);
                }

            }
            return previews;

        }

        public async Task<PreviewScreenDto> GetPreviewDetails(int id)
        {
            var header = new PreviewHeaderDto();
            var details = new List<PreviewDetailsDto>();
            var preview = new PreviewScreenDto();
            {
                header = Mapper.Map<tbl_PreviewHeaders, PreviewHeaderDto>((await _uow.PreviewRepo.FindAsync(p => p.PK_PreviewHeaders_Id == id)).FirstOrDefault());
                if (header == null)
                {
                    return null;
                }
                header.BuyerName =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == header.FK_PreviewHeaders_Clients_BuyerId)).FirstOrDefault().Name;
                var user =(await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == header.FK_PreviewHeaders_Users_CreatedBy)).FirstOrDefault();
                header.EmpName = user.FirstName + " " + user.LastName;
                details = Mapper.Map<List<tbl_PreviewDetails>, List<PreviewDetailsDto>>((await _uow.PreviewDetailRepo.FindAsync(d => d.Fk_PreviewDetails_PreviewHeaders_Id == header.PK_PreviewHeaders_Id && !d.IsDeleted)).ToList());
                foreach (var detail in details)
                {
                    detail.SellerName =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == detail.Fk_PreviewDetails_Clients_SellerId)).FirstOrDefault().Name;
                    if (detail.IsCancelled)
                    {
                        detail.DetailStatus = PreviewStatus.IsCancelled;
                    }
                    else if (detail.IsSucceded)
                    {
                        detail.DetailStatus = PreviewStatus.IsSucceded;
                    }
                    else if (detail.IsNoDecision)
                    {
                        detail.DetailStatus = PreviewStatus.IsSuspended;
                    }
                    else if (detail.IsRejected)
                    {
                        detail.DetailStatus = PreviewStatus.IsRejected;
                    }
                    else if (detail.IsNoDecision)
                    {
                        detail.DetailStatus = PreviewStatus.IsSuspended;
                    }
                    else
                    {
                        detail.DetailStatus = PreviewStatus.IsUndefined;
                    }
                }
                preview.Header = header;
                preview.Details = details;
            };
            return preview;
        }

        public async Task<PreviewScreenDto> SetPreviewResults(int id)
        {
            var header = new PreviewHeaderDto();
            var details = new List<PreviewDetailsDto>();
            var preview = new PreviewScreenDto();
            if (id > 0)
            {
                header = Mapper.Map<tbl_PreviewHeaders, PreviewHeaderDto>((await _uow.PreviewRepo.FindAsync(p => p.PK_PreviewHeaders_Id == id)).FirstOrDefault());
                if (header == null)
                {
                    return null;
                }
                header.BuyerName =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == header.FK_PreviewHeaders_Clients_BuyerId)).FirstOrDefault().Name;
                header.PhoneNumber =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == header.FK_PreviewHeaders_Clients_BuyerId)).FirstOrDefault().Mobile;
                var user =(await _uow.UserRepo.FindAsync(u => u.PK_Users_Id == header.FK_PreviewHeaders_Users_CreatedBy)).FirstOrDefault();
                header.EmpName = user.FirstName + " " + user.LastName;
                details = Mapper.Map<List<tbl_PreviewDetails>, List<PreviewDetailsDto>>((await _uow.PreviewDetailRepo.FindAsync(d => !d.IsDeleted && d.Fk_PreviewDetails_PreviewHeaders_Id == header.PK_PreviewHeaders_Id && d.IsNoDecision)).ToList());
                foreach (var detail in details)
                {
                    detail.SellerName =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == detail.Fk_PreviewDetails_Clients_SellerId)).FirstOrDefault().Name;

                }

                preview.Header = header;
                preview.Details = details;
            };
            return preview;
        }

        public async Task<bool> SetPreviewDetSatus(int id, string status)
        {
            var detail =(await _uow.PreviewDetailRepo.FindAsync(p => p.PK_PreviewDetails_Id == id)).FirstOrDefault();
            if (detail != null)
            {
                detail.IsCancelled = false;
                detail.IsNoDecision = false;
                detail.IsRejected = false;
                detail.IsPostponed = false;
                detail.IsSucceded = false;
                if (status == PreviewStatus.IsSucceded)
                {
                    detail.IsSucceded = true;
                    await _uow.SaveAsync();
                }
                else if (status == PreviewStatus.IsRejected)
                {
                    detail.IsRejected = true;
                    await _uow.SaveAsync();
                }
                else if (status == PreviewStatus.IsCancelled)
                {
                    detail.IsCancelled = true;
                    await _uow.SaveAsync();
                }
                else if (status == PreviewStatus.IsPostponed)
                {
                    detail.IsPostponed = true;
                    await _uow.SaveAsync();
                }
                else if (status == PreviewStatus.IsSuspended)
                {
                    detail.IsNoDecision = true;
                    await _uow.SaveAsync();
                }
            }
            return await SetHeaderStatus(detail.Fk_PreviewDetails_PreviewHeaders_Id, detail.IsNoDecision);

        }

        public async Task<bool> PostponePreview(int preview, int detail, DateTime newDate, int userId)
        {
            var previewHeader =await GetPreviewDetails(preview);
            if (previewHeader != null)
            {
                int[] sellers = previewHeader.Details.Where(d => d.PK_PreviewDetails_Id == detail).Select(d => d.Fk_PreviewDetails_Clients_SellerId).ToArray();
                int[] availables = previewHeader.Details.Where(d => d.PK_PreviewDetails_Id == detail).Select(d => d.AvailableUnits_Id).ToArray();
                int[] availablCats = previewHeader.Details.Where(d => d.PK_PreviewDetails_Id == detail).Select(d => d.Category_Id).ToArray();

                var dates = new List<DateTime>();
                dates.Add(newDate);
                _conf = await CreatePreview(previewHeader.Header.FK_PreviewHeaders_Clients_BuyerId, sellers, previewHeader.Header.DemandUnit_Id, availables, userId, dates, previewHeader.Header.Category_Id,availablCats);
            }
            return _conf.Valid;
        }

        public async Task<bool> DeletePreview(int id, int userId)
        {

            var DBPreview =(await _uow.PreviewRepo.FindAsync(u => u.PK_PreviewHeaders_Id == id)).FirstOrDefault();
            if (DBPreview != null)
            {
                DBPreview.IsDeleted = true;
                _uow.PreviewRepo.Update(DBPreview);

            }
            var deleted = await _uow.SaveAsync() > 0;
            if (deleted)
            {
                var details = await _uow.PreviewDetailRepo.FindAsync(d => d.Fk_PreviewDetails_PreviewHeaders_Id == id);
                if (details != null || details.Any())
                {
                    foreach (var item in details)
                    {
                        item.IsDeleted = true;
                        item.FK_PreviewDetails_Users_ModidfiedBy = userId;
                    }
                }
                return await _uow.SaveAsync() > 0;
            }
            return false;

        }

        public async Task<PreviewHeaderDto> FindById(int id)
        {
            return Mapper.Map<tbl_PreviewHeaders, PreviewHeaderDto>((await _uow.PreviewRepo.FindAsync(p => p.PK_PreviewHeaders_Id == id && !p.IsDeleted)).FirstOrDefault());

        }

        public async Task<bool> IsPreviewExistsInSameTime(int[] availableIds, List<DateTime> dates,int [] availableCats)
        {
            bool Exists = false;
            List<tbl_PreviewDetails> previewDetailsList = new List<tbl_PreviewDetails>();
            List<string> times = new List<string>();
            foreach (var date in dates)
            {
                times.Add(date.ToString("hh:mm tt"));
            }
            if (availableIds.Length > 0)
            {
                for (int i=0;i<availableIds.Count();i++)
                {
                    foreach (var date in dates)
                    {
                        foreach (var time in times)
                        {
                            var id = availableIds[i];
                            var cat = availableCats[i];
                            var previewdetail =(await _uow.PreviewDetailRepo.FindAsync(p => (p.AvailableUnits_Id ==id &&p.Category_Id==cat) && p.PreviewHeaderDate == date.Date && p.PreviewTime == time && p.IsNoDecision)).FirstOrDefault();
                            if (previewdetail != null)
                            {
                                Exists = true;
                                previewDetailsList.Add(previewdetail);
                            }
                            else
                            {
                                Exists = false;
                            }
                        }
                    }
                }
            }
            if (previewDetailsList != null)
            {
                return Exists;
            }
            return Exists;
        }

        public async Task<bool> IsPostponePreviewExistsInSameTime(int oldPreviewDetail, DateTime postBoneDate)
        {
            var oldPreviewDetailObj =(await _uow.PreviewDetailRepo.FindAsync(d=>d.PK_PreviewDetails_Id==oldPreviewDetail)).FirstOrDefault();
            tbl_PreviewDetails previewDetail = new tbl_PreviewDetails();
            if (oldPreviewDetailObj!=null)
            {               
                string time = postBoneDate.ToString("hh:mm tt");
                previewDetail =(await _uow.PreviewDetailRepo.FindAsync(p => (p.AvailableUnits_Id == oldPreviewDetailObj.AvailableUnits_Id &&
                p.Category_Id == oldPreviewDetailObj.Category_Id) && 
                p.PreviewHeaderDate == postBoneDate.Date && p.PreviewTime == time && p.IsNoDecision&&!p.IsDeleted)).FirstOrDefault();
                if (previewDetail != null)
                {
                    return true;
                }
            }          
          
            return false;
        }

        public async Task<List<string>> GetReservationDates(DateTime date, int id,int catId)
        {
            //DateTime pDate = Convert.ToDateTime(date.ToString("yyyy-MM-dd 00:mm:ss.fffffff"));
            var previews =await _uow.PreviewDetailRepo.FindAsync(p => p.AvailableUnits_Id == id
            &&p.Category_Id==catId
            && p.PreviewHeaderDate.Day == date.Day
            && p.PreviewHeaderDate.Year == date.Year
            && p.PreviewHeaderDate.Month == date.Month
            && p.IsDeleted == false
            && p.IsNoDecision == true);
            List<string> previewDetailsDates = new List<string>();
            if (previews != null)
            {
                foreach (var preview in previews)
                {
                    previewDetailsDates.Add(preview.PreviewTime);
                }
            }
            return previewDetailsDates;
        }

        public async Task<bool> SetHeaderStatus(int id, bool status)
        {
            var header =(await _uow.PreviewRepo.FindAsync(p => p.PK_PreviewHeaders_Id == id)).FirstOrDefault();
            var details =await _uow.PreviewDetailRepo.FindAsync(p => p.Fk_PreviewDetails_PreviewHeaders_Id == id);
            if (details.Any(p => p.IsNoDecision == true))
            {
                return true;
            }
            header.IsSuspended = status;
            _uow.PreviewRepo.Update(header);
            return await _uow.SaveAsync() > 0;
        }
    }

    public struct combine
    {
        public int sellerId { get; set; }

        public int availalableId { get; set; }

        public DateTime Date { get; set; }

        public DateTime DetailTime { get; set; }

        public int availableCatId { get; set; }

    }



}
