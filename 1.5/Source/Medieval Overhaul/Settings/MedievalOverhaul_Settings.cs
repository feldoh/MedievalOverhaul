﻿using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Verse;

namespace MedievalOverhaul
{
    public class MedievalOverhaul_Settings : ModSettings
    {
        public static Dictionary<string, bool> settingMode = new();

        // Debug Mode
        public bool debugMode = false;

        // Production Chains
        public bool leatherChain = true;
        public bool woodChain = true;
        public bool clothChain = true;

        // Map Generation Stuff
        public bool industrialJunk = true;
        //public bool exostriderRemains = true;
        public bool hornetNest = false;
        public bool vanillaMine = false;

        // Misc
        public bool refuelableTorch = false;
        public bool boomalopeTar = true;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref leatherChain, "leatherChain", true);
            Scribe_Values.Look(ref woodChain, "woodChain", true);
            Scribe_Values.Look(ref clothChain, "clothChain", true);
            Scribe_Values.Look(ref industrialJunk, "industrialJunk", true);
            //Scribe_Values.Look(ref exostriderRemains, "exostriderRemains", true);
            Scribe_Values.Look(ref hornetNest, "hornetNest", false);
            Scribe_Values.Look(ref vanillaMine, "vanillaMine", false);
            Scribe_Values.Look(ref refuelableTorch, "refuelableTorch", false);
            Scribe_Values.Look(ref boomalopeTar, "boomalopeTar", true);
            Scribe_Values.Look(ref debugMode, "debugMode", false);
            Scribe_Collections.Look(ref settingMode, "settingMode", LookMode.Value, LookMode.Value);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                settingMode ??= new Dictionary<string, bool>();
            }

        }
        public IEnumerable<string> toggleSettings
        {
            get
            {
                return GetType().GetFields().Where(p => p.FieldType == typeof(bool) && (bool)p.GetValue(this)).Select(p => p.Name);
            }
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(rect);
            listingStandard.Label((string)"DankPyon_Settings_ProductionChain".Translate());
            listingStandard.CheckboxLabeled((string)"DankPyon_Settings_LeatherChain".Translate(), ref this.leatherChain);
            listingStandard.CheckboxLabeled((string)"DankPyon_Settings_WoodChain".Translate(), ref this.woodChain);
            listingStandard.CheckboxLabeled((string)"DankPyon_Settings_ClothChain".Translate(), ref this.clothChain);
            listingStandard.Gap();
            listingStandard.GapLine();
            listingStandard.Gap();
            listingStandard.Label((string)"DankPyon_Settings_MapGen".Translate());
            listingStandard.CheckboxLabeled((string)"DankPyon_Settings_IndustrialJunk".Translate(), ref this.industrialJunk);
            //listingStandard.CheckboxLabeled((string)"DankPyon_Settings_ExostriderRemains".Translate(), ref this.exostriderRemains);
            listingStandard.CheckboxLabeled((string)"DankPyon_Settings_HornetNest".Translate(), ref this.hornetNest);
            listingStandard.CheckboxLabeled((string)"DankPyon_Settings_VanillaMine".Translate(), ref this.vanillaMine);
            listingStandard.Gap();
            listingStandard.GapLine();
            listingStandard.Gap();
            listingStandard.Label((string)"DankPyon_Settings_MiscOption".Translate());
            listingStandard.CheckboxLabeled((string)"DankPyon_Settings_RefuelableTorch".Translate(), ref this.refuelableTorch);
            listingStandard.CheckboxLabeled((string)"DankPyon_Settings_BoomalopeTar".Translate(), ref this.boomalopeTar);
            listingStandard.End();
        }
    }
}
