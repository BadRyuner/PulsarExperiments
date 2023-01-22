using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulsarExperiments.Features.Ships
{
	[HarmonyPatch] // max - 68 i32, free slots starts from 69 i32
	internal static class Patches
	{
		[HarmonyPatch(typeof(PLPersistantEncounterInstance), "GetPrefabNameForShipType")]
		internal static void Postfix(EShipType inShipType, ref string __result)
		{
			int type = (int)inShipType;

			if (type == UfoShip.UFOShipType)
				__result = "UFO";
			else if (type == UfoWithInteriorShip.ShipTypeInt)
				__result = "UFOWithInterior";
		}
	}
}
