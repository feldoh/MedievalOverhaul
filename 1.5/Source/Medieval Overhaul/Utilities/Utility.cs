﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;
using RimWorld;

namespace MedievalOverhaul
{
    public static class Utility
    {
        public static SeperateHideList WhiteList = DefDatabase<SeperateHideList>.GetNamed("WhiteList");
        public static SeperateWoodList LogList = DefDatabase<SeperateWoodList>.GetNamed("LogList");
        public static HideGraphicList HideGraphicList = DefDatabase<HideGraphicList>.GetNamed("HideGraphicList");
        public static ModContentPack myContentPack = LoadedModManager.GetMod<MedievalOverhaulSettings>().Content;
        public static Thing FindSpecificFuel(Pawn pawn, ThingDef fuel)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(fuel), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: new Predicate<Thing>(validator));

            bool validator(Thing x) => !ForbidUtility.IsForbidden(x, pawn) && pawn.CanReserve((LocalTargetInfo)x);
        }

        public static Thing FindSpecificClosestFuel(Pawn pawn, List<ThingDef> fuelDefList)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.ClosestTouch, TraverseParms.For(pawn), validator: new Predicate<Thing>(validator));

            bool validator(Thing x) => !ForbidUtility.IsForbidden(x, pawn) && fuelDefList.Contains(x.def) && pawn.CanReserve((LocalTargetInfo)x);
        }
        public static bool FilterItemExists(ThingFilter filter, Pawn pawn)
        {
            foreach (var def in filter.AllowedThingDefs)
            {
                List<Thing> thingsOfDef = pawn.Map.listerThings.ThingsOfDef(def);
                if (thingsOfDef.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static string RemoveSubstring(ThingDef thingDef, string partToRemove)
        {
            string stringDefName = thingDef.defName;
            int index = stringDefName.IndexOf(partToRemove);
            if (index == -1)
            {
                return stringDefName;
            }
            else
            {
                string modifiedString = stringDefName.Replace(partToRemove, "");

                if (IsValidString(modifiedString))
                {
                    return modifiedString;
                }
                else
                {
                    return stringDefName;
                }
            }
        }
        static bool IsValidString(string input)
        {
            return !string.IsNullOrEmpty(input);
        }
    }
}
