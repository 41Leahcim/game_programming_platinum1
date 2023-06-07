using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Settings{
    [RequireComponent(typeof(Slider))]
    public class SliderSetting : MonoBehaviour{
        [SerializeField] private string settingName;
        private Slider slider;

        void Awake(){
            slider = GetComponent<Slider>();
            slider.value = PlayerPrefs.GetFloat(settingName, 1.0f);
        }

        public void Save(){
            PlayerPrefs.SetFloat(settingName, slider.value);
        }
    }
}
