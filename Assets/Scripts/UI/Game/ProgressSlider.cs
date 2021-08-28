using System;
using Attack;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class ProgressSlider : MonoBehaviour
    {
        public TargetEnum Target;
        private Slider _slider;

        void Start()
        {
            _slider = GetComponent<Slider>();
        }
        
        void Update()
        {
            _slider.value = Target switch
            {
                TargetEnum.Bullet => PlayerController.Instance.PercentToShot,
                TargetEnum.ElectricField => PlayerController.Instance.PercentToField,
                TargetEnum.Ray => PlayerController.Instance.PercentToRay,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public enum TargetEnum
        {
            Bullet,
            ElectricField,
            Ray
        }
    }
}
