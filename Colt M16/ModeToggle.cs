using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Mod {
    public class ModeToggle : MonoBehaviour {

        public bool IsAuto; // Checks if Full Auto is enabled
        public bool IsBurst; // Checks if Burst Fire is enabled
        public float FireRate; // Holds value for Fire Rate
        public bool IsHeld;
        public int UseID = 1;
        public int FireID = 1;
        public FirearmBehaviour Fire;
        public AudioSource ModeClick;
        float Counter = 0f;

        void Awake() {
            ModeClick = gameObject.AddComponent<AudioSource>();
            ModeClick.clip = ModAPI.LoadSound("sounds/switchfire.wav");
        }

        void Start() {
            IsAuto = gameObject.GetComponent<FirearmBehaviour>().Automatic;
            FireRate = gameObject.GetComponent<FirearmBehaviour>().AutomaticFireInterval;
            gameObject.GetComponent<FirearmBehaviour>().IgnoreUse = true;

            Counter = FireRate;
            IsBurst = true;
            Buttons();
        }

        void Buttons() {
            this.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("sfm", "Selective Fire Toggle", "Set the fire mode to either Full Auto or Semi-Auto.", () => {
                SelectiveFire();
            }));
        }

        void OnMouseOver() {
            if(UseID == 1) {
                if(Input.GetKeyDown(KeyCode.V)) {
                    SelectiveFire();
                }
            }
            Counter += Time.deltaTime;
        }

        void Use() {
            if(!IsAuto && !IsBurst) {
                FireGun();
            }
            else if(!IsAuto && IsBurst) {
                StartCoroutine(BurstFire());
            }
        }

        void UseContinuous()
        {
            if (IsAuto && !IsBurst)
            {
                FireGun();
            }
        }

        IEnumerator BurstFire() {
            for(int i = 0; i < 3; i++) {
                FireGun();
                yield return new WaitForSeconds(FireRate+0.01f);
            }
        }

        void FireGun()
        {
            FirearmBehaviour Fire = this.gameObject.GetComponent<FirearmBehaviour>();
          
            if (Counter > FireRate) {
                Counter  = 0f;
                Fire.Shoot();
                Counter = 0f;
            }
        }
    
        void SelectiveFire() {
            switch(FireID) {
                case 1:
                    FireID = 1;
                    IsAuto = false;
                    IsBurst = false;                            // Semi-Automatic
                    ModAPI.Notify("Switched to Semi-Auto");
                    break;
                case 2:
                    FireID = 2;
                    IsAuto = false;
                    IsBurst = true;                             // 3 Round Burst Fire
                    ModAPI.Notify("Switched to Burst Fire");
                    break;
                case 3:
                    FireID = 3;
                    IsAuto = true;
                    IsBurst = false;                            // Full Auto. ill-advised inside buildings.
                    ModAPI.Notify("Switched to Full Auto");
                    break;
            }
            FireID++;
            if(FireID > 3) {
                FireID = 1; // if statement limits the FireID to specified number
            }
            ModeClick.Play();
        }
    }
}