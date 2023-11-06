using System;
using System.Threading;
using System.Threading.Tasks;
using Ball.Core.AsyncSequence;
using UnityEngine.SceneManagement;

namespace Ball.Client.AsyncActions
{
  public class LoadSceneAction : BaseAsyncAction
  {
    private readonly string _sceneName;

    public LoadSceneAction(string sceneName) =>
      _sceneName = sceneName;

    public override async Task<Result<AsyncActionResult>> InvokeAsync(
      AsyncActionSequence sequence,
      AsyncSequenceContext context,
      CancellationToken cancellationToken)
    {
      SceneManager.LoadScene(_sceneName);

      await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(true);

      return ResultFromType(AsyncActionResultType.Complete);
    }
  }
}
