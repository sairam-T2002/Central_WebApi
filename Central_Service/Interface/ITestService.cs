
namespace Central_Service.Interface
{
    public interface ITestService : IDisposable
    {
        Task<string> TestMethod();
    }
}
