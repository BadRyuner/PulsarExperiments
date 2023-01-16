using System;
using PulsarModLoader.Content.Items;
using UnityEngine;

namespace PulsarExperiments.Features.Items
{
	internal class SpaceEagleMod : ItemMod
	{
		public override string Name => "SpaceEagle";
		public override PLPawnItem PLPawnItem => new SpaceEagle();

		public class SpaceEagle : PLPawnItem_PhasePistol
		{
			public SpaceEagle() : base()
			{
				m_MarketPrice = 1000;
				MinAutoFireDelay = 0.5f;
				AIEffectiveRange = 25f;
				Desc = "Unpopular AOG Weapon";
				Name = "Space Eagle";
				UsesAmmo = true;
				AmmoMax = 7;
				AmmoCurrent= 7;
			}

			protected override GameObject GetGunPrefab() => Prefabs.SpaceEagle;

			protected override float CalcDamageDone() => 38f + 8f * (float)base.Level;

			public override string GetItemName(bool skipLocalization = false) => "Space Eagle";

			public override void FireShot(Vector3 aimAtPoint, Vector3 destNormal, int newBoltID, Collider hitCollider)
			{
				if (MySetupPawn != null)
				{
					base.FireShot(aimAtPoint, destNormal, newBoltID, hitCollider);
					Heat += 0.3f;
					MySetupPawn.MyIK.ShotFeedbackAmt += 4f;
					MySetupPawn.CurrentAccuracyRating += 3f;
				}
			}
		}
	}
}
