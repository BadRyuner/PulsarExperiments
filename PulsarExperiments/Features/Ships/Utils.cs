using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PulsarExperiments.Features.Ships
{
	public static class Utils
	{
		private static GameObject WDDrone = Resources.Load<GameObject>("NetworkPrefabs/WD_SecurityDrone_03");

		public static void FixShields(MeshRenderer r) =>
			r.material = WDDrone.transform.Find("Drone_4").Find("SheildBubble_OriginalDrone").GetComponent<MeshRenderer>().material;
	}
}
