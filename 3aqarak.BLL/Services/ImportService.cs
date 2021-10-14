using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    public class ImportService:IImportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;

        public ImportService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }

    }
}
