using Central_Service.Core;
using Central_Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository_DAL_.Model;
using Repository_DAL_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Service
{
    public class ApiLogService: ServiceBase, IApiLog
    {
        private readonly IRepository<ApiLog> _logService;
        public ApiLogService( ILogger<AppDataService> logger, IServiceProvider serviceProvider, IRepository<ApiLog> logService ) : base(logger, serviceProvider)
        {
            _logService = logService;
        }

        public async void AddDbLog( List<ApiLog> logItem )
        {
            try
            {
                await _logService.AddRange(logItem);
            }
            catch(Exception ex )
            {
                await _logService.Add(new ApiLog {log_origin = $"ApiLogService.{nameof(AddDbLog)}",log = "",Exception = ex.Message, DateTime = DateTime.UtcNow });
            }
            
        }
    }
}
