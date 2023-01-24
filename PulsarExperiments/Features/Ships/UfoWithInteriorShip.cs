using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PulsarExperiments.Features.Ships
{
	public class UfoWithInteriorShip : PLShipInfo
	{
		public const int ShipTypeInt = 70;

		protected override void Start()
		{
			base.Start();
			var screenshader = new Material(Shader.Find("Custom/ScreenShader"));
			foreach(var screen in InteriorDynamic.GetComponentsInChildren<PLUIScreen>())
				screen.GetComponent<MeshRenderer>().material = screenshader;
		}

		public override void ShipFinalCalculateStats(ref PLShipStats inStats)
		{
			base.ShipFinalCalculateStats(ref inStats);
		}

		public override void SetupShipStats(bool previewStats = false, bool startingPlayerShip = false)
		{
			base.SetupShipStats(previewStats, startingPlayerShip);
			this.ShipTypeID = (EShipType)ShipTypeInt;
			this.MyStats.Mass = 500f;
			this.ReverseThrustEnabled = true;
			this.ThrusterSFXEventName = "intrepid_external_thruster_large";
			if (this.MyScreenBase != null)
			{
				this.MyScreenBase.ScreenThemeAtlas = PLGlobal.Instance.CurvedThemeAtlas;
			}
			if (startingPlayerShip)
			{
				this.NumberOfFuelCapsules = 20;
			}
			else
			{
				this.NumberOfFuelCapsules = 5;
			}
			this.GX_ID = "Tier Unknown UFO";
			this.FactionID = 0;
			this.ShipExplosionID = 14;
			this.AngDragMultiplier = 1.3f;

			this.MyStats.SetSlotLimit(ESlotType.E_COMP_HULL, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_CPU, 2);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_REACTOR, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_SENS, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_SHLD, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_TELE, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_WARP, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_MAINTURRET, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_THRUSTER, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_INERTIA_THRUSTER, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_MANEUVER_THRUSTER, 2);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_CARGO, 2);

			//this.MyStats.SetSlotLimit(ESlotType.E_COMP_O2, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_HULLPLATING, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_CAPTAINS_CHAIR, 1);
			this.MyStats.SetSlotLimit(ESlotType.E_COMP_SENSORDISH, 1);

			base.ShipNameValue = "UFO";
			if (PhotonNetwork.isMasterClient)
			{
				PLRand shipDeterministicRand = PLShipInfoBase.GetShipDeterministicRand(this.PersistantShipInfo, 0);
				this.MyStats.AddShipComponent(new PLHullPlating(EHullPlatingType.E_HULLPLATING_CCGE, 0), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLCaptainsChair(ECaptainsChairType.E_COLONIAL_CLASSIC, 0, 0), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLSensorDish(ESensorDishType.E_NORMAL, 0), -1, ESlotType.E_COMP_NONE);
				float num = Mathf.Clamp01(shipDeterministicRand.NextFloat() * 0.5f + PLServer.Instance.ChaosLevel * 0.12f);
				if (num < 0.3f)
				{
					this.MyStats.AddShipComponent(new PLMegaTurret(base.GetChaosBoost(shipDeterministicRand.Next() % 50)), -1, ESlotType.E_COMP_NONE);
				}
				else if (num < 0.5f)
				{
					this.MyStats.AddShipComponent(new PLMegaTurret_RapidFire(base.GetChaosBoost(shipDeterministicRand.Next() % 50)), -1, ESlotType.E_COMP_NONE);
				}
				else if (num < 0.9f)
				{
					this.MyStats.AddShipComponent(new PLMegaTurretCU(base.GetChaosBoost(shipDeterministicRand.Next() % 50)), -1, ESlotType.E_COMP_NONE);
				}
				else
				{
					this.MyStats.AddShipComponent(new PLMegaTurretCU_2(base.GetChaosBoost(shipDeterministicRand.Next() % 50)), -1, ESlotType.E_COMP_NONE);
				}
				this.MyStats.AddShipComponent(new PLSensor_EM(3 + base.GetChaosBoost(shipDeterministicRand.Next() % 50)), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLCPU(ECPUClass.E_CPUTYPE_CYBER_DEF, 2 + base.GetChaosBoost(shipDeterministicRand.Next() % 50)), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLHull(EHullType.E_NANO_MACHINES, Mathf.Max(1, base.GetChaosBoost(shipDeterministicRand.Next() % 50))), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLWarpDrive(EWarpDriveType.E_WARPDR_CU_STANDARD_JUMP_MODULE, 0, 0), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLThruster(EThrusterType.E_THRUSTER_NORMAL, 0), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLManeuverThruster(EManeuverThrusterType.E_NORMAL, 0), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLManeuverThruster(EManeuverThrusterType.E_NORMAL, 0), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLInertiaThruster(EInertiaThrusterType.E_NORMAL, 0), -1, ESlotType.E_COMP_NONE);
				this.MyStats.AddShipComponent(new PLReactor(EReactorType.E_REAC_WD_NULL_POINT_REACTOR_B, Mathf.Max(3, base.GetChaosBoost(shipDeterministicRand.Next() % 50))), -1, ESlotType.E_COMP_NONE);
				//this.MyStats.AddShipComponent(new PLShieldGenerator(EShieldGeneratorType.E_SG_WD_PARTICLE_SHIELD_UP, base.GetChaosBoost(shipDeterministicRand.Next() % 50)), -1, ESlotType.E_COMP_NONE);

				base.AddRandomCPU(shipDeterministicRand.Next() % 50, 1);
				base.AddRandomCPU(shipDeterministicRand.Next() % 50, 1);
			}
		}

		public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			base.OnPhotonSerializeView(stream, info);
		}
	}
}
