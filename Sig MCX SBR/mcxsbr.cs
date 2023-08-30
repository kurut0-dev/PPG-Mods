using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace Mod {
    public class Mod {
        public static void Main() {
            ModAPI.Register(
                new Modification() {
                    OriginalItem = ModAPI.FindSpawnable("M1 Garand"),
                    NameOverride = "SIG Sauer MCX SBR 9\"",
                    NameToOrderByOverride = "A",
                    DescriptionOverride = "The SIG Sauer MCX (SBR Variant), or as it's called in MW19, the M13.",
                    CategoryOverride = ModAPI.FindCategory("Firearms"),
                    ThumbnailOverride = ModAPI.LoadSprite("thumb/thumbnail.png"),
                    AfterSpawn = (Instance) =>
                    {

                        // Barrel Sprite Rendering
                        var Barrel = new GameObject("ChildObject");
                        Barrel.transform.SetParent(Instance.transform);
                        Barrel.transform.localPosition = new Vector3(0f, 0f);
                        Barrel.transform.rotation = Instance.transform.rotation;
                        Barrel.transform.localScale = new Vector3(1f, 1f);

                        var BarrelSprite = Barrel.AddComponent<SpriteRenderer>();
                        BarrelSprite.sprite = ModAPI.LoadSprite("textures/mcxsbr_barrel.png", 22f);
                        BarrelSprite.sortingLayerName = "Default";
                        BarrelSprite.sortingOrder = 1;

                        
                        // Slide Sprite Rendering
                        var Slide = Instance.transform.Find("Slide");
                        Slide.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/mcxsbr_bolt_transparent.png", 22f);

                        var SlideObj = new GameObject("ChildObject");
                        SlideObj.transform.SetParent(Slide.transform);
                        SlideObj.transform.localPosition = new Vector3(0f, 0f);
                        SlideObj.transform.rotation = Slide.transform.rotation;
                        SlideObj.transform.localScale = new Vector3(1f, 1f);
                        
                        var SlideSprite = SlideObj.AddComponent<SpriteRenderer>();
                        SlideSprite.sprite = ModAPI.LoadSprite("textures/mcxsbr_bolt.png", 22f);
                        SlideSprite.sortingLayerName = "Default";
                        SlideSprite.sortingOrder = 2; 
                        
                        ModAPI.KeepExtraObjects();

                        // Gun's Collider
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/mcxsbr_collider.png", 22f);
                        foreach (var c in Instance.GetComponents<Collider2D>())
                        {
                            GameObject.Destroy(c);
                        }
                        Instance.FixColliders();

                        // Main Body Rendering
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/mcxsbr_body.png", 22f);
                        Instance.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                        Instance.GetComponent<SpriteRenderer>().sortingOrder = 3;
                        
                        // Adds Selective Fire toggling to gun
                        // Credits to megaturn for the Fire Mode code
                        Instance.AddComponent<ModeToggle>();
                        var FireMode = Instance.GetComponent<ModeToggle>();

                        // Firearm Behaviour
                        var firearm = Instance.GetComponent<FirearmBehaviour>();
                        
                        firearm.BulletsPerShot = 1;
                        firearm.Automatic = true;
                        firearm.AutomaticFireInterval = 0.075f;
                        firearm.barrelPosition = new Vector3(0.602f, 0.0865f, 0.0f);

                        // Repositions points for attachments
                        Instance.transform.Find("BarrelAttachment").localPosition = new Vector3(0.295f, 0.047f, 0.0f);
                        Instance.transform.Find("ScopeAttachment").localPosition = new Vector3(-0.01f, 0.162f, 0.0f);
                        
                        Vector3 SuppressorPosition = Instance.GetComponent<FirearmBehaviour>().barrelPosition;

                        //creates a new attachment point for the barrel
                        GameObject NewBarrelAttachmentPoint = GameObject.Instantiate(Instance.GetComponentInChildren<FirearmAttachmentPointBehaviour>().gameObject);
                        NewBarrelAttachmentPoint.transform.SetParent(Instance.transform, false);

                        //sets the position of the new attachment point
                        NewBarrelAttachmentPoint.transform.localPosition = SuppressorPosition;
                        NewBarrelAttachmentPoint.transform.localEulerAngles = new Vector3(0, 0, 90); NewBarrelAttachmentPoint.GetComponent<FirearmAttachmentPointBehaviour>().AttachmentType = FirearmAttachmentType.AttachmentType.Other;

                        // Firearm Sounds
                        firearm.ShotSounds = new AudioClip[] {
                            ModAPI.LoadSound("sounds/mcxsbr_01.wav"),
                            ModAPI.LoadSound("sounds/mcxsbr_02.wav"),
                            ModAPI.LoadSound("sounds/mcxsbr_03.wav")
                        };

                        Cartridge customCartridge = ModAPI.FindCartridge("5.56x45mm");
                        customCartridge.name = "5.56x45mm NATO";
                        customCartridge.Damage *= 1.3f;
                        customCartridge.StartSpeed *= 1.5f;
                        customCartridge.Recoil *= 4.5f;
                        customCartridge.ImpactForce *= 0.8f;
                        firearm.Cartridge = customCartridge;
                        firearm.casingPosition = new Vector3(0.05f, 0.1f, 0.0f);
                    }
                }
            );
        }
    }
}