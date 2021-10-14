using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface IStaticService
    {
        Task<bool> DeleteTemplate(int id, int userId);
        Task<StaticDto> FindTemplateByID(int id);
        Task<SelectList> GetCatList();
        Task<SelectList> GetCatListById(int id);
        Task<List<StaticDto>> GetTemplates();
        Task<SelectList> GetTransList();
        Task<SelectList> GetTransListById(int id);
        Task<bool> SaveTemplate(StaticDto template, int userId);
        Task<bool> UpdateTemplate(StaticDto template, int userId);


    }
}
