using PulsarModLoader;
using PulsarModLoader.Content.Items;
using UnityEngine;

namespace PulsarExperiments.Features.Items
{
	public class PoorKatanaMod : ItemMod
	{
		public override string Name => "PoorKatana";
		public override PLPawnItem PLPawnItem => new PoorKatana();

		public class PoorKatana : PLPawnItem_Melee
		{
			public PoorKatana() : base()
			{
				this.m_MarketPrice = 1200;
			}

			public override string GetItemName(bool skipLocalization = false) => "Katana";

			protected override GameObject GetGunPrefab() => Prefabs.Katana;
		}
	}
}
