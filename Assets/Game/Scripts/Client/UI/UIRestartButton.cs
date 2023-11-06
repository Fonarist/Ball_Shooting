using System;
using System.Collections;
using System.Collections.Generic;
using Ball.Client.Placements;
using Ball.Core.Binding;
using Ball.Core.Placements;
using UnityEngine;
using UnityEngine.UI;

namespace Ball.Client.UI
{
    public class UIRestartButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(OnClickRestart);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClickRestart);
        }

        private void OnClickRestart()
        {
            var placementsModule = ServiceLocator.Resolve<IPlacementsModule>();
            placementsModule.InvokePlacement(new RestartLevelPlacement());
        }
    }
}
