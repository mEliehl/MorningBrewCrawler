using System.Threading.Tasks;

namespace Crawler.Commands
{
    public interface ICommand
    {
         
    }

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand Command);
    }
}