using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ball.Core.Modules
{
  public abstract class BaseModule : IModule
  {
    protected virtual string Tag => GetType().Name;
    public virtual Type[] Contracts => new[] { GetType() };

    public virtual void LateDispose() { }
    public virtual void Resolve() { }
    public virtual Task InitializeAsync(CancellationToken cancellationToken) { return Task.CompletedTask; }
    public virtual void Run() { }
  }
}