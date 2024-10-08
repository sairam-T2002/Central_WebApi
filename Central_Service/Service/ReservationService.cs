using Central_Service.Core;
using Central_Service.Interface;
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
    public class ReservationService : ServiceBase, IReservationService
    {
        public ReservationService( ILogger<AuthService> logger, IServiceProvider serviceProvider ) : base(logger, serviceProvider)
        {
        }

        
    }
}
