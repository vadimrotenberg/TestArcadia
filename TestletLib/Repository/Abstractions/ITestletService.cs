using TestletLib.Models;

namespace TestletLib.Repository.Abstractions
{
    public interface ITestletService
    {
        Testlet BuildTestlet(string testLetId);
    }
}
