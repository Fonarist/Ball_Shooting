using System.Threading;
using System.Threading.Tasks;
using Ball.Core.Binding;

namespace Ball.Core.Features
{
    public abstract class BaseMonoFeature : MonoBindable, IFeature
    {
        public virtual void Resolve() { }
        public virtual Task InitializeAsync(CancellationToken cancellationToken) { return Task.CompletedTask; }
        public virtual void Run() { }
    }
}
