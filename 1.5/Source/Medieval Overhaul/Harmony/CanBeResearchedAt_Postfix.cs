﻿using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MedievalOverhaul.Medieval_Overhaul.Harmony
{
    [HarmonyPatch(typeof(ResearchProjectDef), "CanBeResearchedAt")]
    public static class CanBeResearchedAt_Postfix
    {
        public static void Postfix(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus, ResearchProjectDef __instance, ref bool __result)
        {
            if (__result)
            {
                var fuelComp = bench.GetComp<CompRefuelable>();
                if (fuelComp != null && !fuelComp.HasFuel)
                {
                    __result = false;
                }
            }
        }
    }
}
