using Ball.Core.Binding;
using Ball.Core.Placements;
using Ball.Client.Placements;
using UnityEngine;

namespace Ball.Client
{
  public class BallApplication : MonoBehaviour
  {
    private static BallApplication Instance;
    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
      }

      Launch();
    }
    
    private void Launch()
    {
      var placementsModule = ServiceLocator.Resolve<IPlacementsModule>();
      placementsModule.RegisterPlacement<ApplicationStartPlacement>(new ApplicationStartSequenceBuilder());
      placementsModule.RegisterPlacement<RestartLevelPlacement>(new RestartLevelSequenceBuilder());
      placementsModule.InvokePlacement(new ApplicationStartPlacement());
    }
  }
}
