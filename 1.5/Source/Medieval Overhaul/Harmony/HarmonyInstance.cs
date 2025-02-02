﻿using HarmonyLib;
using ProcessorFramework;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace MedievalOverhaul
{
    [StaticConstructorOnStartup]
    public static class HarmonyInstance
    {
        public static Dictionary<HediffDef, StatDef> statMultipliers = new Dictionary<HediffDef, StatDef>();

        //public static Harmony harmony;
        static HarmonyInstance()
        {
            foreach (var stat in DefDatabase<StatDef>.AllDefs)
            {
                var extension = stat.GetModExtension<HediffFallRateMultiplier>();
                if (extension != null && extension.hediffDef != null)
                {
                    statMultipliers[extension.hediffDef] = stat;
                }
            }
            foreach (var def in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (def.IsChunk() && def.projectileWhenLoaded is null)
                {
                    def.projectileWhenLoaded = MedievalOverhaulDefOf.DankPyon_Artillery_Boulder;
                }
            }
        }

        public static bool IsChunk(this ThingDef def)
        {
            if (!def.thingCategories.NullOrEmpty())
            {
                if (!def.thingCategories.Contains(ThingCategoryDefOf.Chunks))
                {
                    return def.thingCategories.Contains(ThingCategoryDefOf.StoneChunks);
                }
            }
            return false;
        }
    }

    [StaticConstructorOnStartup]
    public static class ArtillerySearchGroup
    {
        private static readonly Dictionary<ThingDef, ThingRequestGroup> registeredArtillery = new Dictionary<ThingDef, ThingRequestGroup>();
        static ArtillerySearchGroup()
        {
            RegisterArtillery(MedievalOverhaulDefOf.DankPyon_Artillery_Trebuchet, ThingRequestGroup.Chunk);
        }

        public static bool RegisterArtillery(ThingDef def, ThingRequestGroup ammoGroup)
        {
            if (!registeredArtillery.ContainsKey(def))
            {
                registeredArtillery.Add(def, ammoGroup);
                return true;
            }
            return false;
        }

        public static bool TryGetArtillery(this Thing thing, out ThingRequestGroup group) => registeredArtillery.TryGetValue(thing.def, out group);
    }

    public class HediffFallRateMultiplier : DefModExtension
    {
        public HediffDef hediffDef;
    }

    public class Breakdown
    {
        public Thing FindComponent(Pawn pawn, Thing thing)
        {
            if (thing.def.techLevel == TechLevel.Neolithic || thing.def.techLevel == TechLevel.Medieval)
            {
                return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(MedievalOverhaulDefOf.DankPyon_ComponentBasic), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
            }
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.ComponentIndustrial), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
        }
    }

    [HarmonyPatch(typeof(HediffComp_SeverityPerDay), "SeverityChangePerDay")]
    [HarmonyPatch(typeof(HediffComp_Immunizable), "SeverityChangePerDay")]
    public class HediffComp_Immunizable_Patch
    {
        private static void Postfix(HediffComp_SeverityPerDay __instance, ref float __result)
        {
            if (HarmonyInstance.statMultipliers.TryGetValue(__instance.Def, out var stat))
            {
                __result *= __instance.Pawn.GetStatValue(stat);
            }
        }
    }

    //[HarmonyPatch(typeof(ResearchProjectDef), "CanBeResearchedAt")]
    //public static class ResearchProjectDef_CanBeResearchedAt_Patch
    //{
    //    public static void Postfix(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus, ResearchProjectDef __instance, ref bool __result)
    //    {
    //        if (__result)
    //        {
    //            var fuelComp = bench.GetComp<CompRefuelable>();
    //            if (fuelComp != null && !fuelComp.HasFuel)
    //            {
    //                __result = false;
    //            }
    //        }
    //    }
    //}

}
