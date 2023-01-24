using System;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Util;

namespace PulsarExperiments.ScaryThings
{
	[HarmonyPatch(typeof(PLPathfinder), "LoadGraph")]
	public class PLShipAStarConnectionForBundles
	{
		private static bool Prefix(PLPathfinder __instance, string targetPath, ref List<uint> graphIDs, PLTeleportationLocationInstance tli, int optionalTerrainGraphID)
		{
			if (targetPath.Contains("redirme"))
			{
				PulsarModLoader.Utilities.Logger.Info($"[PLShipAStarConnectionForBundles] Redir for {targetPath}");
				//TextAsset textAsset = Resources.Load(targetPath) as TextAsset;

				TextAsset textAsset = Prefabs.NavForUFO;

				List<NavGraph> list = new List<NavGraph>(__instance.AStarPath.astarData.graphs);
				if (textAsset != null)
				{
					byte[] bytes = textAsset.bytes;
					if (bytes != null)
					{
						try
						{
							__instance.AStarPath.astarData.DeserializeGraphsAdditive(bytes);
						}
						catch (Exception ex)
						{
							PulsarModLoader.Utilities.Logger.Info($"[PLShipAStarConnectionForBundles] OOOPS");
							Debug.LogError("error DeserializeGraphsAdditive: " + targetPath + "    " + ex.Message);
						}
					}
				}
				int num = 0;
				foreach (NavGraph navGraph in __instance.AStarPath.astarData.graphs) // eh, recastgraphs only in paid version of Astar ><
				{
					RecastGraph recastGraph = navGraph as RecastGraph;
					if (recastGraph != null && !list.Contains(navGraph))
					{
						uint pge_IDCounter = __instance.PGE_IDCounter;
						__instance.PGE_IDCounter = pge_IDCounter + 1U;
						uint num2 = pge_IDCounter;
						PLPathfinderGraphEntity plpathfinderGraphEntity = new PLPathfinderGraphEntity();
						plpathfinderGraphEntity.ID = num2;
						plpathfinderGraphEntity.LastUsedFrame = Time.frameCount;
						plpathfinderGraphEntity.Graph = recastGraph;
						plpathfinderGraphEntity.TLI = tli;
						plpathfinderGraphEntity.TerrainMode = num == optionalTerrainGraphID;
						plpathfinderGraphEntity.GraphBounds = recastGraph.forcedBounds;
						plpathfinderGraphEntity.Handler = new TileHandler(plpathfinderGraphEntity.Graph);
						plpathfinderGraphEntity.Handler.CreateTileTypesFromGraph();
						__instance.AllPGEs.Add(plpathfinderGraphEntity);
						plpathfinderGraphEntity.LargestAreaIndex = __instance.GetLargestAreaIndex(navGraph);
						graphIDs.Add(num2);
						num++;
					}
				}
				return false; // override
			}
			return true;
		}
	}
}
