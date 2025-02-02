﻿using MedievalOverhaul;
using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;
using System.Diagnostics;
using System.Linq;

namespace MedievalOverhaul
{
	public class RecipeWorkerCounter_MakeWoodPlanks : RecipeWorkerCounter
	{
		public override bool CanCountProducts(Bill_Production bill)
		{
			return true;
		}

		public override int CountProducts(Bill_Production bill)
		{
			int countProductNum = 0;
			for (int i = 0; i < TimberUtility.AllPlanks.Count; i++)
			{
				IEnumerable <Thing> thing = bill.Map.listerThings.ThingsOfDef(TimberUtility.AllPlanks[i]);
				foreach (var item in thing)
				{
					//Thing things = item as Thing;
					countProductNum += item.stackCount;
				}
			}
			return countProductNum;
		}

		public override string ProductsDescription(Bill_Production bill)
		{
			return MedievalOverhaulDefOf.DankPyon_Wood.label;
		}

		public override bool CanPossiblyStore(Bill_Production bill, ISlotGroup slotGroup)
		{
            foreach (ThingDef allowedThingDef in bill.ingredientFilter.AllowedThingDefs)
			{
                if (!allowedThingDef.butcherProducts.NullOrEmpty())
				{
                    ThingDef thingDef = allowedThingDef.butcherProducts[0].thingDef;
					if (!slotGroup.Settings.AllowedToAccept(thingDef))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
