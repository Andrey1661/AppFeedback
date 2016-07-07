using System.Threading.Tasks;

namespace AppFeedBack.Commands.Interfaces
{
    public interface ICommand
    {
        Task Execute();
    }
}
