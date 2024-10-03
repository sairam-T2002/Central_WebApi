using Repository_DAL_.Model;

namespace Central_Service.Interface
{
    public interface IApiLog
    {
        void AddDbLog(List<ApiLog> logItem);
    }
}
