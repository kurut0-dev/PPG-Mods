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
                    NameOverride = "HK P30L",
                    NameToOrderByOverride = "A",
                    DescriptionOverride = "John Wick's HK P30L as seen in the first John Wick",
                    CategoryOverride = ModAPI.FindCategory("Firearms"),
                    ThumbnailOverride = ModAPI.LoadSprite("thumb/thumbnail.png"),
                    AfterSpawn = (Instance) =>
                    {
                        ModAPI.KeepExtraObjects();

                        // Barrel Sprite Rendering
                        var Barrel = new GameObject("ChildObject");
                        Barrel.transform.SetParent(Instance.transform);
                        Barrel.transform.localPosition = new Vector3(0f, 0f);
                        Barrel.transform.rotation = Instance.transform.rotation;
                        Barrel.transform.localScale = new Vector3(1f, 1f);

                        var BarrelSprite = Barrel.AddComponent<SpriteRenderer>();
                        BarrelSprite.sprite = ModAPI.LoadSprite("textures/p30l_barrel.png", 20f);
                        BarrelSprite.sortingLayerName = "Default";
                        BarrelSprite.sortingOrder = 1;

                        // Slide Sprite Rendering
                        var Slide = Instance.transform.Find("Slide");
                        Slide.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/p30l_slide_transparent.png", 20f);

                        var SlideObj = new GameObject("ChildObject");
                        SlideObj.transform.SetParent(Slide.transform);
                        SlideObj.transform.localPosition = new Vector3(0f, 0f);
                        SlideObj.transform.rotation = Slide.transform.rotation;
                        SlideObj.transform.localScale = new Vector3(1f, 1f);
                        
                        var SlideSprite = SlideObj.AddComponent<SpriteRenderer>();
                        SlideSprite.sprite = ModAPI.LoadSprite("textures/p30l_slide.png", 20f);
                        SlideSprite.sortingLayerName = "Default";
                        SlideSprite.sortingOrder = 2;

                        // Firearm Behaviour
                        var firearm = Instance.GetComponent<FirearmBehaviour>();

                        Cartridge customCartridge = ModAPI.FindCartridge("9mm");
                        customCartridge.name = "9x19mm Parabellum";
                        customCartridge.Damage *= 0.8f;
                        customCartridge.StartSpeed *= 1.5f; 
                        customCartridge.PenetrationRandomAngleMultiplier *= 0.5f;
                        customCartridge.Recoil *= 4f;
                        customCartridge.ImpactForce *= 0.7f;

                        firearm.Cartridge = customCartridge;

                        // Firearm Sounds
                        firearm.ShotSounds = new AudioClip[] {
                            ModAPI.LoadSound("sounds/p30l_01.wav"),
                            ModAPI.LoadSound("sounds/p30l_02.wav"),
                            ModAPI.LoadSound("sounds/p30l_03.wav")
                        };

                        // Gun's Collider
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/p30l_collider.png", 20f);
                        foreach (var c in Instance.GetComponents<Collider2D>())
                        {
                            GameObject.Destroy(c);
                        }
                        Instance.FixColliders();

                        // Main Body Rendering
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/p30l_body.png", 20f);
                        Instance.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                        Instance.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    }
                }
            );
        }
    }
}