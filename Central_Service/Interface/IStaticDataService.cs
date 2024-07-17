using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Central_Service.Model;

namespace Central_Service.Interface
{
    public interface IStaticDataService
    {
        Task<StaticDataRet_DTO> ServeStaticData( string imagePath );
    }
}
