using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ball.Core.Binding;
using UnityEngine;

namespace Ball.Core.AsyncSequence
{
    public class AsyncActionSequence : ILateDisposable
    {
      private readonly List<IAsyncAction> _actions;
      public AsyncActionSequence()
      {
        _actions = new List<IAsyncAction>();
      }

      public AsyncActionSequence AddAction(IAsyncAction action)
      {
        _actions.Add(action);

        return this;
      }

      public Task<AsyncActionResultType> InvokeAsync()
      {
        return InvokeAsync(CancellationToken.None);
      }

      public async Task<AsyncActionResultType> InvokeAsync(CancellationToken cancellationToken, AsyncSequenceContext context = null)
      {
        var queueResult = await InvokeInternalAsync(cancellationToken, context).ConfigureAwait(true);
        LateDispose();

        return queueResult;
      }

      private async Task<AsyncActionResultType> InvokeInternalAsync(CancellationToken cancellationToken, AsyncSequenceContext context = null)
      {
        context ??= new AsyncSequenceContext();

        foreach (var action in _actions)
        {
          Debug.Log($"[{nameof(AsyncActionSequence)}] Invoke action {action.GetType().Name}");
          var actionResult = await action.InvokeAsync(this, context, cancellationToken).ConfigureAwait(true);

          if (actionResult.IsError)
          {
            Debug.LogError($"[{nameof(AsyncActionSequence)}] Action {action.GetType().Name} complete with error {actionResult.Error}");

            return AsyncActionResultType.Failed;
          }

          Debug.Log($"[{nameof(AsyncActionSequence)}] Action {action.GetType().Name} complete with result {actionResult.Value}");
          switch (actionResult.Value.ResultType)
          {
            case AsyncActionResultType.Cancelled:
              Debug.LogError($"[{nameof(AsyncActionSequence)}] {action.GetType()} was cancelled!");
              return AsyncActionResultType.Cancelled;
            case AsyncActionResultType.Failed:
              Debug.LogError($"[{nameof(AsyncActionSequence)}] {action.GetType()} completed but failed!");
              return AsyncActionResultType.Failed;
          }

          if (actionResult.Value.ResultSequence != null)
          {
            var subSequenceResult = await actionResult.Value.ResultSequence.InvokeAsync(cancellationToken, context).ConfigureAwait(true);
            if (subSequenceResult != AsyncActionResultType.Complete)
            {
              return subSequenceResult;
            }
          }
        }

        return AsyncActionResultType.Complete;
      }

      public void LateDispose()
      {
        _actions.Clear();
      }
    }
}