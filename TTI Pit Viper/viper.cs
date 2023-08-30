using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Mod {
    public class Mod {
        public static void Main() {
            ModAPI.Register(
                new Modification() {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "TTI JW4 Pit Viper",
                    NameToOrderByOverride = "A",
                    DescriptionOverride = "John Wick's TTI Pit Viper as seen in John Wick 4",
                    CategoryOverride = ModAPI.FindCategory("Firearms"),
                    ThumbnailOverride = ModAPI.LoadSprite("thumb/thumbnail.png"),
                    AfterSpawn = (Instance) =>
                    {
                        ModAPI.KeepExtraObjects(); 
                        var childObject = new GameObject("ChildObject");
                        childObject.transform.SetParent(Instance.transform);
                        childObject.transform.localPosition = new Vector3(0f, 0f);
                        childObject.transform.rotation = Instance.transform.rotation;
                        childObject.transform.localScale = new Vector3(1f, 1f);

                        var Slide = Instance.transform.Find("Slide");
                        Slide.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/pitviper_slide.png", 23f);
                        Slide.GetComponent<SpriteRenderer>().sortingLayerName = "Top";

                        var firearm = Instance.GetComponent<FirearmBehaviour>();

                        Cartridge customCartridge = ModAPI.FindCartridge("9mm");
                        customCartridge.name = "9mm Pit Viper";
                        customCartridge.Damage *= 0.8f;
                        customCartridge.StartSpeed *= 1.5f; 
                        customCartridge.PenetrationRandomAngleMultiplier *= 0.5f;
                        customCartridge.Recoil *= 4f;
                        customCartridge.ImpactForce *= 0.7f;

                        firearm.Cartridge = customCartridge;

                        firearm.ShotSounds = new AudioClip[] {
                            ModAPI.LoadSound("sounds/viper_fire_01.wav")
                        };

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/viper_collider.png", 23f);
                        foreach (var c in Instance.GetComponents<Collider2D>())
                        {
                            GameObject.Destroy(c);
                        }
                        Instance.FixColliders();

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/pitviper.png", 23f);
                        Instance.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }
                }
            );
        }
    }
}