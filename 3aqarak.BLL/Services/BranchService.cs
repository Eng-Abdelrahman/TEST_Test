using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using AutoMapper;
using _3aqarak.BLL.Models;

namespace _3aqarak.BLL.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _uow;
        private IConfirmation _conf;

        public BranchService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }

        public async Task<IConfirmation> CheckIsMainBranch(BranchDto branch)
        {
            if (branch.PK_Branch_Id == 0)
            {
                var exists =(await _uow.BranchRepo.FindAsync(b => b.IsMainBranch && !b.IsDeleted)).Any();
                if (exists)
                {
                    _conf.Valid = false;
                    _conf.Message = "لايمكن تكرار الفرع الرئيسي مرتين";
                }
                else
                {
                    _conf.Valid = true;
                }
            }
            else
            {
                var DBbranch =(await _uow.BranchRepo.FindAsync(b =>b.PK_Branch_Id == branch.PK_Branch_Id)).FirstOrDefault();
                if (DBbranch != null)
                {
                    var exists = (await _uow.BranchRepo.FindAsync(b => b.IsMainBranch && !b.IsDeleted && b.PK_Branch_Id != branch.PK_Branch_Id)).Any();
                    if (exists)
                    {
                        _conf.Valid = false;
                        _conf.Message = "لايمكن تكرار الفرع الرئيسي مرتين";
                    }
                    else
                    {
                        _conf.Valid = true;
                    }
                }
                else
                {
                    _conf.Valid = false;
                    _conf.Message = "البيان غير موجود في قاعدة البيانات";
                }
            }
            return _conf;
        }

        public async Task<bool> DeleteBranch(int id, int userId)
        {
            var DBBranch =(await _uow.BranchRepo.FindAsync(u => u.PK_Branch_Id == id)).FirstOrDefault();
            if (DBBranch != null)
            {
                DBBranch.IsDeleted = true;
                DBBranch.FK_Branches_Users_ModidfiedBy = userId;
                _uow.BranchRepo.Update(DBBranch);

            }
            return await _uow.SaveAsync() > 0;
        }       

        public async Task<BranchDto> FindByID(int id)
        {
            var Branch =(await _uow.BranchRepo.FindAsync(u => u.PK_Branch_Id == id)).FirstOrDefault();
            return (Branch != null) ? Mapper.Map<tbl_Branches, BranchDto>(Branch) : new BranchDto();
        }
        /// <summary>
        /// Add function without Async 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BranchDto FindByIDWithoutAsync(int id)
        {
            var Branch =  _uow.BranchesCustRepoWithoutAsync.Find(u => u.PK_Branch_Id == id).FirstOrDefault();
            return (Branch != null) ? Mapper.Map<tbl_Branches, BranchDto>(Branch) : new BranchDto();
        }


        public async Task<List<BranchDto>> GetBranches()
        {
            var Branches = await _uow.BranchRepo.FindAsync(u => u.IsDeleted == false);
            if (Branches.Any() && Branches != null)
            {
                return Mapper.Map<List<tbl_Branches>, List<BranchDto>>(Branches.ToList());
            }
            return new List<BranchDto>();
        }

        public async Task<bool> SaveBranch(BranchDto Branch, int userId)
        {
            if (Branch.PK_Branch_Id == 0)
            {
                var newBranch = Mapper.Map<BranchDto, tbl_Branches>(Branch);
                newBranch.FK_Branches_Users_CreatedBy = userId;
                newBranch.FK_Branches_Users_ModidfiedBy = userId;
                newBranch.CreatedAt = DateTime.UtcNow.AddMinutes(120);
                newBranch.ModifiedAt = DateTime.UtcNow.AddMinutes(120);            
                _uow.BranchRepo.Add(newBranch);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateBranch(BranchDto Branch, int userId)
        {
            var DBBranch =(await _uow.BranchRepo.FindAsync(u => u.PK_Branch_Id== Branch.PK_Branch_Id)).FirstOrDefault();
            if (DBBranch != null)
            {
                DBBranch.Name = Branch.Name;
                DBBranch.FK_Branches_Users_ModidfiedBy = userId;
                DBBranch.FK_Branches_Users_MgrId = Branch.FK_Branches_Users_MgrId;
                DBBranch.Address = Branch.Address;
                DBBranch.PhoneNumber = Branch.PhoneNumber;
                DBBranch.IsMainBranch = Branch.IsMainBranch;                
                _uow.BranchRepo.Update(DBBranch);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
