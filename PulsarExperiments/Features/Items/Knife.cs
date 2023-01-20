using System;
using PulsarExperiments.ScaryThings;
using PulsarModLoader.Content.Items;
using UnityEngine;

namespace PulsarExperiments.Features.Items
{
	public class KnifeMod : ItemMod
	{
		public override string Name => "Knife";
		public override PLPawnItem PLPawnItem => new Knife();

		public class Knife : PLPawnItem_Melee
		{
			public Knife() : base()
			{
				this.m_MarketPrice = 700;
			}

			protected override GameObject GetGunPrefab() => Prefabs.Knife;

			public override float StaminaLoss => 0.2f;

			public override float Damage() => 20f + 5f * Level;

			public override string GetItemName(bool skipLocalization = false) => "Knife";

			public override void Setup(PLPawn inPawn, PLPawnInventory inInventory)
			{
				base.Setup(inPawn, inInventory);
				vec = MyGunInstance.gameObject.GetComponent<SetVector>();
			}

			public override void OnUpdate()
			{
				base.OnUpdate();
				if (attack.isPlaying)
				{
					MySetupPawn.MyIK.rightHandEffector_OriginalLocalPos = vec.Vec;
				}
			}

			public SetVector vec;
			public static Vector3 DefaultRightHandPos = new Vector3(0.09f, -0.2f, 0.2f);

			public override void OnInActive()
			{
				base.OnInActive();
				MySetupPawn.MyIK.rightHandEffector_OriginalLocalPos = DefaultRightHandPos; // reset
			}

			public override void UnSetup()
			{
				base.UnSetup();
				MySetupPawn.MyIK.rightHandEffector_OriginalLocalPos = DefaultRightHandPos; // reset
			}
		}
	}
}
