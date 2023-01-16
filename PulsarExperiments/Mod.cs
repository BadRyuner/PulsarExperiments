using System;
using PulsarModLoader;

namespace PulsarExperiments
{
	public class Mod : PulsarMod
	{
		public override string HarmonyIdentifier() => "BadExperiments";
		public override string Author => "Bad";
		public override string Name => "Experiments";
		public override string Version => "1";
		
		public Mod()
		{
			Prefabs.LoadPrefabs();
		}
	}
}
