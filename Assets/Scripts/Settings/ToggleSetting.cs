using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Settings{
    [RequireComponent(typeof(Toggle))]
    public class ToggleSetting : MonoBehaviour{
        [SerializeField] protected string settingName;
        protected Toggle toggle;

        public virtual void Awake(){
            toggle = GetComponent<Toggle>();
            toggle.isOn = PlayerPrefs.GetInt(settingName, 0) == 1;
        }

        public void Save(){
            PlayerPrefs.SetInt(settingName, toggle.isOn? 1 : 0);
        }
    }
}