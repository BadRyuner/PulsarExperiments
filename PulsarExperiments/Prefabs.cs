using System;
using System.IO;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace PulsarExperiments
{
	public static class Prefabs // PulsarExperiments.Prefabs
	{
		public static GameObject SpaceEagle;

		public static GameObject Katana;
		public static GameObject FireAxe;
		public static GameObject Knife;

		public static GameObject EngTablet;

		public static GameObject ScaryyyyUfo;
		public static GameObject UfoWithInterior;

		internal static AssetBundle bundle;

		public static void LoadPrefabs()
		{
			if (bundle != null)
				bundle.Unload(true);

			var assetbundlePath = Path.Combine(new FileInfo(typeof(Prefabs).Assembly.Location).Directory.FullName, "experiments.bundle");
			bundle = AssetBundle.LoadFromFile(assetbundlePath);

			SpaceEagle = bundle.LoadAsset<GameObject>("SpaceEaglePrefab");
			if (SpaceEagle == null)
				throw new Exception("Cant load SpaceEagle!");

			Katana = bundle.LoadAsset<GameObject>("KatanaPrefab");
			if (Katana == null)
				throw new Exception("Cant load Katana!");

			FireAxe = bundle.LoadAsset<GameObject>("FireAxePrefab");
			if (FireAxe == null)
				throw new Exception("Cant load FireAxe!");

			Knife = bundle.LoadAsset<GameObject>("knifePrefab");
			if (Knife == null)
				throw new Exception("Cant load Knife!");

			EngTablet = bundle.LoadAsset<GameObject>("EngTabletPrefab");
			if (EngTablet == null)
				throw new Exception("Cant load EngTablet!");
			Features.Items.EngTabletMod.EngTablet.FixPrefabShader(EngTablet);

			ScaryyyyUfo = bundle.LoadAsset<GameObject>("ufoPrefab");
			if (Katana == null)
				throw new Exception("Cant load UFO!");

			UfoWithInterior = bundle.LoadAsset<GameObject>("ufoIntPrefab");

			Features.Ships.Utils.FixShields(ScaryyyyUfo.transform.Find("Exterior").Find("ShieldBubble").GetComponent<MeshRenderer>());
			Features.Ships.Utils.FixShields(UfoWithInterior.transform.Find("Exterior").Find("ShieldBubble").GetComponent<MeshRenderer>());

			//foreach (var i in bundle.LoadAllAssets<Mesh>())
			//	PulsarModLoader.Utilities.Logger.Info($"{i.name}");
			Features.PawnAppearance.Patch.AddRobotFaces.Add(bundle.LoadAsset<Mesh>("Pyro"));
			Features.PawnAppearance.Patch.AddRobotFaces.Add(bundle.LoadAsset<Mesh>("sphere"));

			PhotonNetwork.PrefabCache.Add("NetworkPrefabs/UFO", ScaryyyyUfo);
			PhotonNetwork.PrefabCache.Add("NetworkPrefabs/UFOWithInterior", UfoWithInterior);
		}
	}

	[HarmonyPatch]
	internal static class FixPrefabCache
	{
		public static MethodBase TargetMethod()
		{
			var type = AccessTools.TypeByName("NetworkingPeer");
			return AccessTools.Method(type, "DoInstantiate");
		}

		public static void Prefix(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, ref GameObject resourceGameObject)
		{
			if (resourceGameObject == null)
			{
				string text = (string)evData[0];
				PhotonNetwork.PrefabCache.TryGetValue(text, out resourceGameObject);
			}
		}
	}
}
