using System;
using Ball.Client.Placements;
using Ball.Core.Binding;
using Ball.Core.Features;
using Ball.Core.Placements;
using UnityEngine;
using UnityEngine.UI;

namespace Ball.Client.UI
{
    public class EndScreenFeature : BaseMonoFeature
    {
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _loseScreen;
        [SerializeField] private Button _buttonContinue;
        
        public override Type[] Contracts => new[] { typeof(EndScreenFeature) };

        public override void LateDispose() { }

        private void Awake()
        {
            _buttonContinue.onClick.AddListener(OnClickContinue);
        }

        private void OnDestroy()
        {
            _buttonContinue.onClick.RemoveListener(OnClickContinue);
        }

        public void Show(bool isWin)
        {
            if (isWin)
            {
                _winScreen.SetActive(true);
            }
            else
            {
                _loseScreen.SetActive(true);
            }
            
            _buttonContinue.gameObject.SetActive(true);
        }

        private void OnClickContinue()
        {
            var placementsModule = ServiceLocator.Resolve<IPlacementsModule>();
            placementsModule.InvokePlacement(new RestartLevelPlacement());
        }
    }
}
