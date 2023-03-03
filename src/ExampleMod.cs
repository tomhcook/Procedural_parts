using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using KSP.Game;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using SpaceWarp.API.Mods;
using SpaceWarp.API.AssetBundles;
using SpaceWarp.API.Configuration;
using SpaceWarp.API.Managers;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using KSP.OAB;

using HarmonyLib;
using KSP.Api;
using KSP.Api.CoreTypes;
using KSP.UI.Binding;
using SpaceWarp.API.Mods;

namespace Procedrual_parts
{

    [MainMod]
    public class Procedrualparts : Mod
    {
        #region Fields

        // Main.
        public List<ObjectAssemblyCategoryButton> categoryButtons;
        private static GameState[] validScenes = new[] { GameState.FlightView, GameState.Map3DView };
        private static bool ValidScene => validScenes.Contains(GameManager.Instance.Game.GlobalGameState.GetState());
        private  bool launched = false;
        private static GameObject appButton;
        public GameObject Newfilter;
        public GameObject ProcedrualTab;
        public static Procedrualparts instance;


        // OnInitialized is called after a mod has been initialized and its assets have been loaded

        #endregion

        public void Start()
        {
            launched = false;
        }

        public void Update()
        {
            if(launched == false)
            {
                byte[] texture = File.ReadAllBytes("../assets/Placeholder.png");
                VabTab(texture);
                FilterTab();
                Logger.Info("unused");
                //VabTab(texture);
            }

            //
            //Logger.Info(parts.activeInHierarchy.ToString());
            //

        }
        public void FilterTab( )
        {
            GameObject filter = GameObject.Find("FILTER TYPE");
            Newfilter = Instantiate(filter.gameObject, filter.transform.parent, true);
        }
        public void VabTab(byte[] texture)
        {
            

            GameObject PartTab = GameObject.Find("GRP-Part-Categories");
            
            Texture2D tex = new Texture2D(24, 24, TextureFormat.ARGB32, false);
            if (PartTab.activeInHierarchy)
            {
                launched = true;
                Logger.Info(launched.ToString());
                Logger.Info(PartTab.name);
                ImageConversion.LoadImage(tex, texture);
                PartTab.transform.GetChild(12).gameObject.SetActive(true);
                ProcedrualTab = Instantiate(PartTab.transform.GetChild(12).gameObject);
                ProcedrualTab.GetChild("icon_science").gameObject.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, 24, 24), new Vector2(0.5f, 0.5f));
                ProcedrualTab.transform.parent = PartTab.transform;
                CopyComponent(PartTab.transform.GetChild(12).gameObject.GetComponent<ObjectAssemblyCategoryButton>(), ProcedrualTab);   
                CopyComponent(PartTab.transform.GetChild(12).gameObject.GetComponent<ToggleExtended>(), ProcedrualTab);
                CopyComponent(PartTab.transform.GetChild(12).gameObject.GetComponent<ToggleExtendedVisualizer>(), ProcedrualTab);
                
                
               
                PartTab.transform.GetChild(12).gameObject.SetActive(false);
            }

        }

        public void PartGen()
        {
            
        }
        // public void CompFilter()
        // {
        //     
        //     System.Type type = Newfilter.GetComponent<KSP.OAB.AssemblyFilterContainer>().GetType();
        //     FieldInfo[] fields = type.GetFields();
        //     
        //     Logger.Info("this is the fields");
        //     foreach(FieldInfo field in fields)
        //     {
        //         Logger.Info(field.Name);
        //         Logger.Info(field.Attributes.ToString());
        //     }
        //     
        // }

        Component CopyComponent(Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                
                field.SetValue(copy, field.GetValue(original));
            }

            return copy;
        }
    }
}
