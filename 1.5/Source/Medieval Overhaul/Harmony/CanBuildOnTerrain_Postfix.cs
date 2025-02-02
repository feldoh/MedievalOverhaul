﻿using HarmonyLib;
using MedievalOverhaul;
using RimWorld;
using Verse;

namespace MedievalOverhaul
{
    [HarmonyPatch(typeof(GenConstruct), "CanBuildOnTerrain")]
    public static class CanBuildOnTerrain_Postfix
    {
        public static void Postfix(ref bool __result, BuildableDef entDef, IntVec3 c, Map map, Rot4 rot, Thing thingToIgnore = null, ThingDef stuffDef = null)
        {
            if (entDef == MedievalOverhaulDefOf.DankPyon_PlowedSoil && !c.GetTerrain(map).IsSoil)
            {
                __result = false;
            }
        }
    }
}