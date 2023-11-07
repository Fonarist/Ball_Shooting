using System;
using Ball.Core.Binding;
using Ball.Core.Features;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Ball.Client.UI
{
    public class ProgressFeature : BaseMonoFeature
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _textProg;
        [SerializeField] private GameObject _textHold;
        
        public override Type[] Contracts => new[] { typeof(ProgressFeature) };

        private void Start()
        {
            _textHold.SetActive(true);
            Reset();
        }

        public void ShowTextHold()
        {
            _textHold.SetActive(true);
        }

        public void Reset()
        {
            _textProg.text = "0%";
            _slider.value = 0;
        }

        public void UpdateProgress(float val)
        {
            if(_textHold.activeSelf)
                _textHold.SetActive(false);
            
            var percent = (int)(val * 100);
            _textProg.text = $"{percent}%";
            
            _slider.value = val;
        }
        
        public override void LateDispose() { }
    }
}
