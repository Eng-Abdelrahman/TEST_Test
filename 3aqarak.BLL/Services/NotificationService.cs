using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using AutoMapper;
using _3aqarak.BLL.Models;
using System.Data.Entity;
using _3aqarak.BLL.Domain;

namespace _3aqarak.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private INotificationDto _noteDto;
       
        public NotificationService(IUnitOfWork uow, INotificationDto noteDto)
        {
            _uow = uow;
            _noteDto = noteDto;
        }

        public async Task<INotificationDto> GetCallsNotifications()
        {
            _noteDto.Calls = Mapper.Map<List<tbl_PostbonedCalls>, List<PostponedCallDto>>((await _uow.PostponedRepo.FindAsync(c => !c.IsDone && DbFunctions.TruncateTime(c.DateTime) == DateTime.Today.Date)).ToList());
            return _noteDto;

        }
        /// <summary>
        /// function without Async 
        /// </summary>
        /// <returns></returns>
        public INotificationDto GetCallsNotificationsWithoutAsync()
        {
            _noteDto.Calls = Mapper.Map<List<tbl_PostbonedCalls>, List<PostponedCallDto>>( _uow.PostboneWithoutAsync.Find(c => !c.IsDone && DbFunctions.TruncateTime(c.DateTime) == DateTime.Today.Date).ToList());
            return _noteDto;

        }

        public async Task<INotificationDto> GetExpectedNotifications()
        {
            _noteDto.ExpectedContracts = Mapper.Map<List<tbl_ExpectedContracts>, List<ExpectedContractDto>>((await _uow.ExpectedRepo.FindAsync(c => !c.IsDeleted && !c.IsCancelled && !c.IsDone && DbFunctions.TruncateTime(c.ExpectDateTime) == DateTime.Today.Date || (c.PostponeDateTime != null && DbFunctions.TruncateTime(c.PostponeDateTime) == DateTime.Today.Date))).ToList());
            return _noteDto;
        }
        /// <summary>
        /// function Without Async
        /// </summary>
        /// <returns></returns>
        public INotificationDto GetExpectedNotificationsWithoutAsync()
        {
            _noteDto.ExpectedContracts = Mapper.Map<List<tbl_ExpectedContracts>, List<ExpectedContractDto>>( _uow.ExpectedContractWithoutAsync.Find(c => !c.IsDeleted && !c.IsCancelled && !c.IsDone && DbFunctions.TruncateTime(c.ExpectDateTime) == DateTime.Today.Date || (c.PostponeDateTime != null && DbFunctions.TruncateTime(c.PostponeDateTime) == DateTime.Today.Date)).ToList());
            return _noteDto;
        }
        public async Task<INotificationDto> GetFinishedRentalsNotifications()
        {
            _noteDto.EndedContracts = Mapper.Map<List<tbl_RentAgreementHeaders>, List<RentHeaderDto>>((await _uow.RentHeaderRepo.FindAsync(h => h.RentalEndDate == DbFunctions.TruncateTime(DateTime.Now) && !h.HasEnded && !h.IsDeleted)).ToList());
            return _noteDto;
        }
        /// <summary>
        /// function Without Async
        /// </summary>
        /// <returns></returns>
        public INotificationDto GetFinishedRentalsNotificationsWithoutAsync()
        {
            _noteDto.EndedContracts = Mapper.Map<List<tbl_RentAgreementHeaders>, List<RentHeaderDto>>( _uow.RentAgreementHeaderWithoutAsync.Find(h => h.RentalEndDate == DbFunctions.TruncateTime(DateTime.Now) && !h.HasEnded && !h.IsDeleted).ToList());
            return _noteDto;
        }

      

        public async Task<INotificationDto> GetPreviewNotifications()
        {
            var previews =await  _uow.PreviewCustomRepository.GetTodayPreviews();
            _noteDto.Previews = Mapper.Map<List<tbl_PreviewHeaders>, List<PreviewHeaderDto>>(previews);
            return _noteDto;
        }

        public INotificationDto GetPreviewNotificationsWithoutAsync()
        {
            var previews = _uow.PreviewCustomRepository.GetTodayPreviewsWithoutAsync();
            _noteDto.Previews = Mapper.Map<List<tbl_PreviewHeaders>, List<PreviewHeaderDto>>(previews);
            return _noteDto;
        }

        public async Task<INotificationDto> GetFinishedRentalsToCollectNotifications()
        {
            var rentalRecord =( await _uow.RentHeaderRepo.FindAsync(rent => !rent.IsDeleted && !rent.HasEnded &&
                (rent.DateNxtRent.Day <= DateTime.Today.Day
                && rent.DateNxtRent.Year <= DateTime.Today.Year
                && rent.DateNxtRent.Month <= DateTime.Today.Month)
                )).ToList();
            _noteDto.RentalsToCollect = Mapper.Map<List<tbl_RentAgreementHeaders>, List<RentHeaderDto>>(rentalRecord);
            return _noteDto;
        }

        public INotificationDto GetFinishedRentalsToCollectNotificationsWithoutAsync()
        {
            var rentalRecord = ( _uow.RentHeaderRepo.Find(rent => !rent.IsDeleted && !rent.HasEnded &&
                (rent.DateNxtRent.Day <= DateTime.Today.Day
                && rent.DateNxtRent.Year <= DateTime.Today.Year
                && rent.DateNxtRent.Month <= DateTime.Today.Month)
                )).ToList();
            _noteDto.RentalsToCollect = Mapper.Map<List<tbl_RentAgreementHeaders>, List<RentHeaderDto>>(rentalRecord);
            return _noteDto;
        }
        public async Task<INotificationDto> GetFinishedSaleToCollectNotifications()
        {
            var saleRecord =(await _uow.SaleHeaderRepo.FindAsync
                (sale => !sale.IsDeleted
                && sale.IsInstallable
                && (sale.DateOfNextInstall.Value.Day <= DateTime.Today.Day
                && sale.DateOfNextInstall.Value.Month <= DateTime.Today.Month
                && sale.DateOfNextInstall.Value.Year <= DateTime.Today.Year))).ToList();
            _noteDto.SaleToCollect = Mapper.Map<List<tbl_SaleAgreementHeaders>, List<SaleHeaderDto>>(saleRecord);
            return _noteDto;
        }

        public INotificationDto GetFinishedSaleToCollectNotificationsWithoutAsync()
        {
            var saleRecord = ( _uow.SaleHeaderRepo.Find
                (sale => !sale.IsDeleted
                && sale.IsInstallable
                && (sale.DateOfNextInstall.Value.Day <= DateTime.Today.Day
                && sale.DateOfNextInstall.Value.Month <= DateTime.Today.Month
                && sale.DateOfNextInstall.Value.Year <= DateTime.Today.Year))).ToList();
            _noteDto.SaleToCollect = Mapper.Map<List<tbl_SaleAgreementHeaders>, List<SaleHeaderDto>>(saleRecord);
            return _noteDto;
        }

        public async Task<INotificationDto> GetFellowupCallsNotifications()
        {
            _noteDto.FellowupCalls = Mapper.Map<List<tbl_FellowupCall>, List<FellowCallDto>>((await _uow.FellowupCallsRepo.FindAsync(c =>!c.IsDeleted && !c.IsDone && DbFunctions.TruncateTime(c.DateTime) == DateTime.Today.Date).ConfigureAwait(false)).ToList());
            return _noteDto;
        }

        public INotificationDto GetFellowupCallsNotificationsWithoutAsync()
        {
            _noteDto.FellowupCalls = Mapper.Map<List<tbl_FellowupCall>, List<FellowCallDto>>(_uow.FellowupCallWithoutAsync.Find(c => !c.IsDeleted && !c.IsDone && DbFunctions.TruncateTime(c.DateTime) == DateTime.Today.Date).ToList());
            return _noteDto;
        }
    }
}
