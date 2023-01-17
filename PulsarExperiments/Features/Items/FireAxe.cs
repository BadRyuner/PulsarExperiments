using System;
using PulsarModLoader.Content.Items;
using UnityEngine;

namespace PulsarExperiments.Features.Items
{
	public class FireAxeMod : ItemMod
	{
		public override string Name => "FireAxe";
		public override PLPawnItem PLPawnItem => new FireAxeItem();

		public class FireAxeItem : PLPawnItem_Melee
		{
			public FireAxeItem() : base()
			{
				this.m_MarketPrice = 1000;
			}

			public override string GetItemName(bool skipLocalization = false) => "Fire Axe";

			protected override GameObject GetGunPrefab() => Prefabs.FireAxe;

			public override float StaminaLoss => 0.4f;

			public override float Damage() => 50f + 11f * base.Level;
		}
	}
}
