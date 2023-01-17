using PulsarModLoader.Content.Items;
using System;
using System.Linq;
using UnityEngine;

namespace PulsarExperiments.Features.Items
{
	internal class EngTabletMod : ItemMod
	{
		public override string Name => "Engineer Tablet";
		public override PLPawnItem PLPawnItem => new EngTablet();

		public class EngTablet : PLPawnItem
		{
			public EngTablet()
				: base(EPawnItemType.E_LASERPISTOL)
			{
				Desc = "Remote reactor control for a lazy engineer";
				m_MarketPrice = 5500;
				base.UsesHeat = false;
				m_AnimID = 3;
			}

			public override string GetItemName(bool skipLocalization = false) => "Engineer Tablet";

			public override GameObject GetVisualPrefab() => Prefabs.EngTablet;

			// what if this can work in VR? Hmmmmm. I cant test this, sorry.
			public override void VRPositionUpdate()
			{
				PLCameraMode_Pilot plcameraMode_Pilot = PLCameraSystem.Instance.CurrentCameraMode as PLCameraMode_Pilot;
				if (PLSteamVR_AllPlatforms.XRSettingsenabled && (PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "LocalPawn" || (plcameraMode_Pilot != null && plcameraMode_Pilot.CameraMode == 3)) && MyItemInstance != null)
				{
					if (PLXMLOptionsIO.Instance.CurrentOptions.GetStringValueAsInt("LeftHandEquip") == 1 && PLInput.Instance.GetLeftHandPositionVRMotion() != Vector3.zero)
					{
						MyItemInstance.transform.position = PLCameraSystem.Instance.LeftHandItemTransform.position;
						MyItemInstance.transform.rotation = PLCameraSystem.Instance.LeftHandItemTransform.rotation;
						return;
					}
					if (PLInput.Instance.GetRightHandPositionVRMotion() != Vector3.zero)
					{
						MyItemInstance.transform.position = PLCameraSystem.Instance.RightHandItemTransform.position;
						MyItemInstance.transform.rotation = PLCameraSystem.Instance.RightHandItemTransform.rotation;
					}
				}
			}

			public override void OnActive()
			{
				base.OnActive();
				if (this.MyItemInstance != null)
				{
					if (this.MyItemInstance.transform.parent != base.GetTargetParentTransform())
					{
						this.MyItemInstance.transform.parent = base.GetTargetParentTransform();
						this.MyItemInstance.transform.localPosition = Vector3.zero;
						this.MyItemInstance.transform.localRotation = Quaternion.identity;
					}
					if (this.MyItemInstance.gameObject.activeSelf != !this.MySetupPawn.IsSprinting)
					{
						this.MyItemInstance.gameObject.SetActive(!this.MySetupPawn.IsSprinting);
					}
				}
			}

			public override void OnInActive()
			{
				base.OnInActive();
				if (MyItemInstance != null && MyItemInstance.gameObject.activeSelf)
				{
					MyItemInstance.gameObject.SetActive(false);
				}
			}

			public override void OnUpdate()
			{
				base.OnUpdate();
				if (this.MyItemInstance != null && this.MySetupPawn != null && this.MySetupPawn.MySkinnedMeshRenderer != null)
				{
					if (this.tabletChildTransforms == null || this.tabletChildTransforms.Length == 0 || this.tabletChildTransforms[0] == null)
					{
						this.tabletChildTransforms = this.MyItemInstance.gameObject.GetComponentsInChildren<Transform>();
					}
					foreach (Transform transform in this.tabletChildTransforms)
					{
						if (transform != null && transform.gameObject.layer != 8)
						{
							transform.gameObject.layer = this.MySetupPawn.MySkinnedMeshRenderer.gameObject.layer;
						}
					}

				}
			}

			public override void UnSetup()
			{
				base.UnSetup();
				if (MyItemInstance != null)
				{
					UnityEngine.Object.Destroy(MyItemInstance.gameObject);
					MyItemInstance = null;
					screen = null;
					EquipID = -1;
				}
			}

			public override void Setup(PLPawn inPawn, PLPawnInventory inInventory)
			{
				if (inPawn != null)
				{
					base.Setup(inPawn, inInventory);
					if (MyItemInstance == null)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GetVisualPrefab(), base.GetTargetParentTransform().position, base.GetTargetParentTransform().rotation);
						gameObject.transform.parent = base.GetTargetParentTransform();
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localRotation = Quaternion.identity;
						MyItemInstance = gameObject.GetComponent<PLPawnItemInstance>();
						MyItemInstance.gameObject.SetActive(false);
						screen = gameObject.GetComponentInChildren<PLClonedScreen>();
						PLShipInfo playership = PLNetworkManager.Instance.LocalPlayer.StartingShip;
						screen.MyScreenHubBase = playership.MyScreenBase;
						screen.MyTargetScreen = playership.MyScreenBase.AllScreens.First(s => s is PLEngineerReactorScreen); // can take any screen
					}
				}
			}

			public PLPawnItemInstance MyItemInstance;
			public PLClonedScreen screen;

			private Transform[] tabletChildTransforms;

			internal static void FixPrefabShader(GameObject g)
			{
				var my = g.GetComponentInChildren<MeshRenderer>();
				var game = PLGlobal.Instance.ScannerPrefab.transform.Find("Scanner_01").GetComponent<MeshRenderer>();
				my.materials = game.materials;
				my.transform.GetComponent<MeshFilter>().mesh = game.transform.GetComponent<MeshFilter>().mesh;
			}
		}
	}
}
