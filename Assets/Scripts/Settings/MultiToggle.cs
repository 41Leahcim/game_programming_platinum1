using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Settings{
    [RequireComponent(typeof(Toggle))]
    public class MultiToggle : ToggleSetting{
        [SerializeField] private Toggle[] otherToggles;
        private bool[] previousStates;

        private void StoreOtherToggleValues(){
            for(int i = 0;i < previousStates.Length;i++){
                previousStates[i] = otherToggles[i].isOn;
            }
        }

        public override void Awake(){
            toggle = GetComponent<Toggle>();
            toggle.isOn = PlayerPrefs.GetInt(settingName, 0) == 1;
            toggle.onValueChanged.AddListener(ToggleOthers);
            previousStates = new bool[otherToggles.Length];
            StoreOtherToggleValues();
        }

        private void ToggleOthers(bool turnedOn){
            if(!turnedOn){
                StoreOtherToggleValues();
                foreach(Toggle toggle in otherToggles){
                    toggle.isOn = false;
                }
            }else{
                for(int i = 0;i < otherToggles.Length;i++){
                    otherToggles[i].isOn = previousStates[i];
                }
            }
        }
    }
}
