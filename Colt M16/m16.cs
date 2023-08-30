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
            // Colt M16A1
            ModAPI.Register(
                new Modification() {
                    OriginalItem = ModAPI.FindSpawnable("M1 Garand"),
                    NameOverride = "Colt M16A1",
                    NameToOrderByOverride = "A",
                    DescriptionOverride = "The Colt M16A1. Features Semi-Auto and Burst Fire selective fire options. Has a scent of napalm in the morning.",
                    CategoryOverride = ModAPI.FindCategory("Firearms"),
                    ThumbnailOverride = ModAPI.LoadSprite("thumb/thumb_m16a1.png"),
                    AfterSpawn = (Instance) =>
                    {
                        // Barrel Sprite Rendering
                        var Barrel = new GameObject("ChildObject");
                        Barrel.transform.SetParent(Instance.transform);
                        Barrel.transform.localPosition = new Vector3(0f, 0f);
                        Barrel.transform.rotation = Instance.transform.rotation;
                        Barrel.transform.localScale = new Vector3(1f, 1f);

                        var BarrelSprite = Barrel.AddComponent<SpriteRenderer>();
                        BarrelSprite.sprite = ModAPI.LoadSprite("textures/m16_barrel.png", 22f);
                        BarrelSprite.sortingLayerName = "Default";
                        BarrelSprite.sortingOrder = 1;

                        
                        // Slide Sprite Rendering
                        var Slide = Instance.transform.Find("Slide");
                        Slide.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16_bolt_transparent.png", 22f);

                        var SlideObj = new GameObject("ChildObject");
                        SlideObj.transform.SetParent(Slide.transform);
                        SlideObj.transform.localPosition = new Vector3(0f, 0f);
                        SlideObj.transform.rotation = Slide.transform.rotation;
                        SlideObj.transform.localScale = new Vector3(1f, 1f);
                        
                        var SlideSprite = SlideObj.AddComponent<SpriteRenderer>();
                        SlideSprite.sprite = ModAPI.LoadSprite("textures/m16_bolt.png", 22f);
                        SlideSprite.sortingLayerName = "Default";
                        SlideSprite.sortingOrder = 2; 
                        
                        ModAPI.KeepExtraObjects();

                        // Gun's Collider
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16_collider.png", 22f);
                        foreach (var c in Instance.GetComponents<Collider2D>())
                        {
                            GameObject.Destroy(c);
                        }
                        Instance.FixColliders();

                        // Main Body Rendering
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16_body.png", 22f);
                        Instance.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                        Instance.GetComponent<SpriteRenderer>().sortingOrder = 3;
                        
                        // Adds Selective Fire toggling to gun
                        // Credits to megaturn and his Modern Firearms mod for the Fire Mode code
                        Instance.AddComponent<ModeToggle>();
                        var FireMode = Instance.GetComponent<ModeToggle>();

                        // Firearm Behaviour
                        var firearm = Instance.GetComponent<FirearmBehaviour>();
                        
                        firearm.BulletsPerShot = 1;
                        //firearm.Automatic = false;
                        firearm.AutomaticFireInterval = 0.075f;
                        firearm.barrelPosition = new Vector3(0.824f, 0.058f, 0.0f);

                        // Repositions points for attachments
                        Instance.transform.Find("BarrelAttachment").localPosition = new Vector3(0.16f, 0.047f, 0.0f);
                        Instance.transform.Find("ScopeAttachment").localPosition = new Vector3(-0.16f, 0.162f, 0.0f);

                        // Reference point for gun's barrel. Used to help positioning the gun's barrel
                        GameObject BarrelReferencePoint = GameObject.Instantiate(Instance.GetComponentInChildren<FirearmAttachmentPointBehaviour>().gameObject);
                        BarrelReferencePoint.transform.SetParent(Instance.transform, false);

                        BarrelReferencePoint.transform.localPosition = Instance.GetComponent<FirearmBehaviour>().barrelPosition;
                        BarrelReferencePoint.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        BarrelReferencePoint.GetComponent<FirearmAttachmentPointBehaviour>().AttachmentType = FirearmAttachmentType.AttachmentType.Other;
                        BarrelReferencePoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                        // Firearm Sounds
                        firearm.ShotSounds = new AudioClip[] {
                            ModAPI.LoadSound("sounds/m16_01.wav"),
                            ModAPI.LoadSound("sounds/m16_02.wav"),
                            ModAPI.LoadSound("sounds/m16_03.wav")
                        };

                        Cartridge customCartridge = ModAPI.FindCartridge("5.56x45mm");
                        customCartridge.name = "5.56x45mm NATO";
                        customCartridge.Damage *= 1.3f;
                        customCartridge.StartSpeed *= 1.5f;
                        customCartridge.Recoil *= 4.5f;
                        customCartridge.ImpactForce *= 0.8f;
                        firearm.Cartridge = customCartridge;
                        firearm.casingPosition = new Vector3(-0.158f, 0.07f, 0.0f);

                        // Reference point for gun casing. Same usage as gun barrel, but for the position of where casings come from
                        GameObject CasingReferencePoint = GameObject.Instantiate(Instance.GetComponentInChildren<FirearmAttachmentPointBehaviour>().gameObject);
                        CasingReferencePoint.transform.SetParent(Instance.transform, false);

                        CasingReferencePoint.transform.localPosition = Instance.GetComponent<FirearmBehaviour>().casingPosition;
                        CasingReferencePoint.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        CasingReferencePoint.GetComponent<FirearmAttachmentPointBehaviour>().AttachmentType = FirearmAttachmentType.AttachmentType.Other;
                        CasingReferencePoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            );

            // Colt M16A1 Grenadier
            ModAPI.Register(
                new Modification() {
                    OriginalItem = ModAPI.FindSpawnable("M1 Garand"),
                    NameOverride = "Colt M16A1 Grenadier",
                    NameToOrderByOverride = "A",
                    DescriptionOverride = "The Colt M16A1, grenadier variant. also comes with a little friend you can say hello to.",
                    CategoryOverride = ModAPI.FindCategory("Firearms"),
                    ThumbnailOverride = ModAPI.LoadSprite("thumb/thumb_m16a1g.png"),
                    AfterSpawn = (Instance) =>
                    {
                        // Barrel Sprite Rendering
                        var Barrel = new GameObject("ChildObject");
                        Barrel.transform.SetParent(Instance.transform);
                        Barrel.transform.localPosition = new Vector3(0f, 0f);
                        Barrel.transform.rotation = Instance.transform.rotation;
                        Barrel.transform.localScale = new Vector3(1f, 1f);

                        var BarrelSprite = Barrel.AddComponent<SpriteRenderer>();
                        BarrelSprite.sprite = ModAPI.LoadSprite("textures/m16g_barrel.png", 22f);
                        BarrelSprite.sortingLayerName = "Default";
                        BarrelSprite.sortingOrder = 1;

                        

                        // Slide Sprite Rendering
                        var Slide = Instance.transform.Find("Slide");
                        Slide.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16g_bolt_transparent.png", 22f);

                        var SlideObj = new GameObject("ChildObject");
                        SlideObj.transform.SetParent(Slide.transform);
                        SlideObj.transform.localPosition = new Vector3(0f, 0f);
                        SlideObj.transform.rotation = Slide.transform.rotation;
                        SlideObj.transform.localScale = new Vector3(1f, 1f);
                        
                        var SlideSprite = SlideObj.AddComponent<SpriteRenderer>();
                        SlideSprite.sprite = ModAPI.LoadSprite("textures/m16g_bolt.png", 22f);
                        SlideSprite.sortingLayerName = "Default";
                        SlideSprite.sortingOrder = 2; 
                        
                        ModAPI.KeepExtraObjects();

                        // Gun's Collider
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16g_collider.png", 22f);
                        foreach (var c in Instance.GetComponents<Collider2D>())
                        {
                            GameObject.Destroy(c);
                        }
                        Instance.FixColliders();

                        // Main Body Rendering
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16g_body.png", 22f);
                        Instance.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                        Instance.GetComponent<SpriteRenderer>().sortingOrder = 3;
                        
                        // Adds Selective Fire toggling to gun
                        // Credits to megaturn for the Fire Mode code
                        Instance.AddComponent<ModeToggle>();
                        var FireMode = Instance.GetComponent<ModeToggle>();

                        // Firearm Behaviour
                        var firearm = Instance.GetComponent<FirearmBehaviour>();
                        
                        firearm.BulletsPerShot = 1;
                        //firearm.Automatic = false;
                        firearm.AutomaticFireInterval = 0.075f;
                        firearm.barrelPosition = new Vector3(0.824f, 0.091f, 0.0f);

                        // Repositions points for attachments
                        Instance.transform.Find("BarrelAttachment").localPosition = new Vector3(0.16f, 0.047f, 0.0f);
                        Instance.transform.Find("ScopeAttachment").localPosition = new Vector3(-0.16f, 0.162f, 0.0f);

                        // Reference point for gun's barrel. Used to help positioning the gun's barrel
                        GameObject BarrelReferencePoint = GameObject.Instantiate(Instance.GetComponentInChildren<FirearmAttachmentPointBehaviour>().gameObject);
                        BarrelReferencePoint.transform.SetParent(Instance.transform, false);

                        BarrelReferencePoint.transform.localPosition = Instance.GetComponent<FirearmBehaviour>().barrelPosition;
                        BarrelReferencePoint.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        BarrelReferencePoint.GetComponent<FirearmAttachmentPointBehaviour>().AttachmentType = FirearmAttachmentType.AttachmentType.Other;
                        BarrelReferencePoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                        // Firearm Sounds
                        firearm.ShotSounds = new AudioClip[] {
                            ModAPI.LoadSound("sounds/m16_01.wav"),
                            ModAPI.LoadSound("sounds/m16_02.wav"),
                            ModAPI.LoadSound("sounds/m16_03.wav")
                        };

                        Cartridge customCartridge = ModAPI.FindCartridge("5.56x45mm");
                        customCartridge.name = "5.56x45mm NATO";
                        customCartridge.Damage *= 1.3f;
                        customCartridge.StartSpeed *= 1.5f;
                        customCartridge.Recoil *= 4.5f;
                        customCartridge.ImpactForce *= 0.8f;
                        firearm.Cartridge = customCartridge;
                        firearm.casingPosition = new Vector3(-0.158f, 0.101f, 0.0f);

                        // Reference point for gun casing. Same usage as gun barrel, but for the position of where casings come from
                        GameObject CasingReferencePoint = GameObject.Instantiate(Instance.GetComponentInChildren<FirearmAttachmentPointBehaviour>().gameObject);
                        CasingReferencePoint.transform.SetParent(Instance.transform, false);

                        CasingReferencePoint.transform.localPosition = Instance.GetComponent<FirearmBehaviour>().casingPosition;
                        CasingReferencePoint.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        CasingReferencePoint.GetComponent<FirearmAttachmentPointBehaviour>().AttachmentType = FirearmAttachmentType.AttachmentType.Other;
                        CasingReferencePoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            );

            // Colt M16A2 Grenadier
            ModAPI.Register(
                new Modification() {
                    OriginalItem = ModAPI.FindSpawnable("M1 Garand"),
                    NameOverride = "Colt M16A2",
                    NameToOrderByOverride = "A",
                    DescriptionOverride = "The Colt M16A2. Only fires Semi and 3-Rnd Burst.",
                    CategoryOverride = ModAPI.FindCategory("Firearms"),
                    ThumbnailOverride = ModAPI.LoadSprite("thumb/thumb_m16a2.png"),
                    AfterSpawn = (Instance) =>
                    {
                        // Barrel Sprite Rendering
                        var Barrel = new GameObject("ChildObject");
                        Barrel.transform.SetParent(Instance.transform);
                        Barrel.transform.localPosition = new Vector3(0f, 0f);
                        Barrel.transform.rotation = Instance.transform.rotation;
                        Barrel.transform.localScale = new Vector3(1f, 1f);

                        var BarrelSprite = Barrel.AddComponent<SpriteRenderer>();
                        BarrelSprite.sprite = ModAPI.LoadSprite("textures/m16a2_barrel.png", 22f);
                        BarrelSprite.sortingLayerName = "Default";
                        BarrelSprite.sortingOrder = 1;

                        

                        // Slide Sprite Rendering
                        var Slide = Instance.transform.Find("Slide");
                        Slide.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16g_bolt_transparent.png", 22f);

                        var SlideObj = new GameObject("ChildObject");
                        SlideObj.transform.SetParent(Slide.transform);
                        SlideObj.transform.localPosition = new Vector3(0f, 0f);
                        SlideObj.transform.rotation = Slide.transform.rotation;
                        SlideObj.transform.localScale = new Vector3(1f, 1f);
                        
                        var SlideSprite = SlideObj.AddComponent<SpriteRenderer>();
                        SlideSprite.sprite = ModAPI.LoadSprite("textures/m16g_bolt.png", 22f);
                        SlideSprite.sortingLayerName = "Default";
                        SlideSprite.sortingOrder = 2; 
                        
                        ModAPI.KeepExtraObjects();

                        // Gun's Collider
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16a2_collider.png", 22f);
                        foreach (var c in Instance.GetComponents<Collider2D>())
                        {
                            GameObject.Destroy(c);
                        }
                        Instance.FixColliders();

                        // Main Body Rendering
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("textures/m16a2_body.png", 22f);
                        Instance.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                        Instance.GetComponent<SpriteRenderer>().sortingOrder = 3;
                        
                        // Adds Selective Fire toggling to gun
                        // Credits to megaturn for the Fire Mode code
                        Instance.AddComponent<ModeToggle>();
                        var FireMode = Instance.GetComponent<ModeToggle>();

                        // Firearm Behaviour
                        var firearm = Instance.GetComponent<FirearmBehaviour>();
                        
                        firearm.BulletsPerShot = 1;
                        //firearm.Automatic = false;
                        firearm.AutomaticFireInterval = 0.075f;
                        firearm.barrelPosition = new Vector3(0.824f, 0.091f, 0.0f);

                        // Repositions points for attachments
                        Instance.transform.Find("BarrelAttachment").localPosition = new Vector3(0.16f, 0.047f, 0.0f);
                        Instance.transform.Find("ScopeAttachment").localPosition = new Vector3(-0.16f, 0.162f, 0.0f);

                        // Reference point for gun's barrel. Used to help positioning the gun's barrel
                        GameObject BarrelReferencePoint = GameObject.Instantiate(Instance.GetComponentInChildren<FirearmAttachmentPointBehaviour>().gameObject);
                        BarrelReferencePoint.transform.SetParent(Instance.transform, false);

                        BarrelReferencePoint.transform.localPosition = Instance.GetComponent<FirearmBehaviour>().barrelPosition;
                        BarrelReferencePoint.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        BarrelReferencePoint.GetComponent<FirearmAttachmentPointBehaviour>().AttachmentType = FirearmAttachmentType.AttachmentType.Other;
                        BarrelReferencePoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                        // Firearm Sounds
                        firearm.ShotSounds = new AudioClip[] {
                            ModAPI.LoadSound("sounds/m16_01.wav"),
                            ModAPI.LoadSound("sounds/m16_02.wav"),
                            ModAPI.LoadSound("sounds/m16_03.wav")
                        };

                        Cartridge customCartridge = ModAPI.FindCartridge("5.56x45mm");
                        customCartridge.name = "5.56x45mm NATO";
                        customCartridge.Damage *= 1.3f;
                        customCartridge.StartSpeed *= 1.5f;
                        customCartridge.Recoil *= 4.5f;
                        customCartridge.ImpactForce *= 0.8f;
                        firearm.Cartridge = customCartridge;
                        firearm.casingPosition = new Vector3(-0.158f, 0.101f, 0.0f);

                        // Reference point for gun casing. Same usage as gun barrel, but for the position of where casings come from
                        GameObject CasingReferencePoint = GameObject.Instantiate(Instance.GetComponentInChildren<FirearmAttachmentPointBehaviour>().gameObject);
                        CasingReferencePoint.transform.SetParent(Instance.transform, false);

                        CasingReferencePoint.transform.localPosition = Instance.GetComponent<FirearmBehaviour>().casingPosition;
                        CasingReferencePoint.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                        CasingReferencePoint.GetComponent<FirearmAttachmentPointBehaviour>().AttachmentType = FirearmAttachmentType.AttachmentType.Other;
                        CasingReferencePoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            );


        }
    }
}