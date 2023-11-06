using System.Threading;
using System.Threading.Tasks;

namespace Ball.Core.Binding
{
    public interface IInitializable
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}