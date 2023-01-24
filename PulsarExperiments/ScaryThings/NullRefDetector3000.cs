using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace PulsarExperiments.ScaryThings
{
	/*
	[HarmonyPatch(typeof(PLGameStatic), "Update")]
	internal static class NullFieldDetector3000
	{
		static bool Prefix(PLGameStatic __instance)
		{
			string state = "1";
			try
			{
				__instance.Update_DroppedItemVisuals();
				bool flag = false;
				if (PLEncounterManager.Instance != null && PLCameraSystem.Instance != null)
				{
					if (PLNetworkManager.Instance.CurrentGame != null)
					{
						if (PLEncounterManager.Instance.PlayerShip != null && PLCameraSystem.Instance.SunShaftCasterTransform != null && PLNetworkManager.Instance.CurrentGame.DirectionalLight_Exterior != null)
						{
							PLCameraSystem.Instance.SunShaftCasterTransform.transform.position = -PLNetworkManager.Instance.CurrentGame.DirectionalLight_Exterior.transform.forward * 500000f;
						}
						if (PLNetworkManager.Instance.CurrentGame.DirectionalLight_Exterior != null && PLEncounterManager.Instance.GetCPEI() != null && PLEncounterManager.Instance.GetCPEI().GameInitWithHubID)
						{
							Quaternion quaternion = PLNetworkManager.Instance.CurrentGame.DirectionalLight_ExteriorRotation;
							Color color = PLNetworkManager.Instance.CurrentGame.DirectionalLight_ExteriorColor;
							if (PLEncounterManager.Instance.PlayerShip != null && PLEncounterManager.Instance.PlayerShip.InWarp)
							{
								quaternion = Quaternion.AngleAxis(180f, PLEncounterManager.Instance.PlayerShip.Exterior.transform.up) * PLEncounterManager.Instance.PlayerShip.Exterior.transform.rotation;
								color = Color.white * 0.2f;
							}
							PLNetworkManager.Instance.CurrentGame.DirectionalLight_Exterior.transform.rotation = quaternion;
							PLNetworkManager.Instance.CurrentGame.DirectionalLight_Exterior.color = color;
						}
					}
					if (__instance.ShipSparks != null)
					{
						int stringValueAsInt = PLXMLOptionsIO.Instance.CurrentOptions.GetStringValueAsInt("ShadowQuality");
						ParticleSystem.LightsModule lights = __instance.ShipSparks.lights;
						lights.maxLights = stringValueAsInt * 10;
						lights.enabled = stringValueAsInt >= 2;
					}
					if (__instance.MainProFlare == null && PLEncounterManager.Instance.GetCPEI() != null && PLEncounterManager.Instance.GetCPEI().ShouldSpawnDefaultFlare())
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.DefaultStarFlare);
						__instance.MainProFlare = gameObject.GetComponent<ProFlare>();
						gameObject.layer = 17;
					}
					bool flag2 = true;
					PLSectorInfo currentSector = PLServer.GetCurrentSector();
					if (currentSector != null)
					{
						flag2 &= currentSector.VisualIndication != ESectorVisualIndication.NEBULA;
						flag2 &= currentSector.VisualIndication != ESectorVisualIndication.GREEN_NEBULA;
						flag2 &= currentSector.MySPI.Faction != 4;
					}
					PLGameShip plgameShip = (PLNetworkManager.Instance.CurrentGame as PLGameShip);
					if (plgameShip != null && plgameShip.DirectionalLight_Exterior != null)
					{
						ProFlare proFlare;
						if (plgameShip.SpaceStarFlare != null)
						{
							__instance.MainProFlare.enabled = false;
							plgameShip.SpaceStarFlare.enabled = plgameShip.DirectionalLight_Exterior.enabled && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp;
							proFlare = plgameShip.SpaceStarFlare;
							if (plgameShip.ShouldTintStarFlare)
							{
								plgameShip.SpaceStarFlare.GlobalTintColor = PLInGameUI.FromAlpha(PLNetworkManager.Instance.CurrentGame.DirectionalLight_ExteriorColor, 1f);
							}
						}
						else
						{
							if (__instance.MainProFlare != null)
							{
								__instance.MainProFlare.enabled = flag2 && plgameShip.DirectionalLight_Exterior.enabled && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp;
							}
							proFlare = __instance.MainProFlare;
						}
						bool flag3 = false;
						bool flag4 = false;
						if (proFlare != null)
						{
							if (PLNetworkManager.Instance.ViewedPawn != null && PLNetworkManager.Instance.ViewedPawn.CurrentShip == null)
							{
								proFlare.enabled = false;
								PLGamePlanet plgamePlanet = plgameShip as PLGamePlanet;
								if (plgamePlanet != null)
								{
									flag3 = !plgamePlanet.IsNighttime && PLNetworkManager.Instance.ViewedPawn.MyInterior == null;
									flag4 = plgamePlanet.IsNighttime && PLNetworkManager.Instance.ViewedPawn.MyInterior == null;
								}
							}
							else
							{
								proFlare.enabled = flag2 && plgameShip.DirectionalLight_Exterior.enabled && PLEncounterManager.Instance.PlayerShip != null && !PLEncounterManager.Instance.PlayerShip.InWarp;
								proFlare.transform.position = plgameShip.DirectionalLight_Exterior.transform.forward * -10000f;
								if (PLCameraSystem.Instance.ProFlaresBatch.GameCamera != PLCameraSystem.Instance.CurrentSubSystem.MainCameras[0])
								{
									PLCameraSystem.Instance.ProFlaresBatch.SwitchCamera(PLCameraSystem.Instance.CurrentSubSystem.MainCameras[0]);
								}
								proFlare.RaycastPhysics = true;
								proFlare.useDistanceFade = false;
								proFlare.useDistanceScale = false;
								proFlare.mask = LayerMask.GetMask(new string[] { "Exterior_RayCast" }) | 1;
							}
						}
						if (plgameShip.SurfaceDayFlare != null)
						{
							plgameShip.SurfaceDayFlare.enabled = flag3;
							if (flag3)
							{
								plgameShip.SurfaceDayFlare.RaycastPhysics = true;
								plgameShip.SurfaceDayFlare.useDistanceFade = false;
								plgameShip.SurfaceDayFlare.useDistanceScale = false;
								plgameShip.SurfaceDayFlare.mask = LayerMask.GetMask(new string[] { "Planet", "PlanetFar" });
							}
						}
						if (plgameShip.SurfaceNightFlare != null)
						{
							plgameShip.SurfaceNightFlare.enabled = flag4;
							if (flag4)
							{
								plgameShip.SurfaceNightFlare.RaycastPhysics = true;
								plgameShip.SurfaceNightFlare.useDistanceFade = false;
								plgameShip.SurfaceNightFlare.useDistanceScale = false;
								plgameShip.SurfaceNightFlare.mask = LayerMask.GetMask(new string[] { "Planet", "PlanetFar" });
							}
						}
					}
					else if (__instance.MainProFlare != null)
					{
						__instance.MainProFlare.enabled = false;
					}
					__instance.AllPawns.RemoveAll((PLPawn item) => item == null || item.gameObject == null);
					__instance.AllPawnBases.RemoveAll((PLPawnBase item) => item == null || item.gameObject == null);
					__instance.AllCreatures.RemoveAll((PLCreature item) => item == null || item.gameObject == null);
					__instance.AllQLI.RemoveAll((QualityLightInfo item) => item == null || item.light == null);
					__instance.AllDLI.RemoveAll((DisableLightInfo item) => item == null || item.light == null);
					__instance.AllDOI.RemoveAll((DisableObjectInfo item) => item == null || item.myRenderer == null);
					__instance.AllCombatTargets.RemoveAll((PLCombatTarget item) => item == null || item.gameObject == null);
					__instance.AllGroundTurrets.RemoveAll((PLGroundTurret item) => item == null || item.gameObject == null);
					__instance.AllBGShips.RemoveAll((PLBGShip item) => item == null || item.gameObject == null);
					__instance.m_AmmoRefills.RemoveAll((PLAmmoRefill item) => item == null || item.gameObject == null);
					int stringValueAsInt2 = PLXMLOptionsIO.Instance.CurrentOptions.GetStringValueAsInt("QualityLevel");
					int stringValueAsInt3 = PLXMLOptionsIO.Instance.CurrentOptions.GetStringValueAsInt("ShadowQuality");
					bool flag5 = false;
					if (PLCameraSystem.Instance.CurrentCameraMode != null)
					{
						flag5 = PLCameraSystem.Instance.CurrentCameraMode.ShouldShowInterior();
					}
					bool flag6 = false;
					if (PLNetworkManager.Instance.ViewedPawn != null && PLNetworkManager.Instance.ViewedPawn.GetPlayer() != null)
					{
						flag6 = PLNetworkManager.Instance.ViewedPawn.GetPlayer().OnPlanet;
					}
					bool flag7 = false;
					if (UnityEngine.Random.Range(0, 1000) == 0 || stringValueAsInt2 != __instance.cachedQLIQualityLevel || __instance.cachedQLICount != __instance.AllQLI.Count || stringValueAsInt3 != __instance.cachedQLIShadowQualityLevel || flag6 != __instance.cachedQLIViewedPawnOnPlanet || __instance.cachedQLIViewedPawn != PLNetworkManager.Instance.ViewedPawn)
					{
						flag7 = true;
						for (int i = 0; i < __instance.AllQLI.Count; i++)
						{
							QualityLightInfo qualityLightInfo = __instance.AllQLI[i];
							int num = qualityLightInfo.shadowMinLevel;
							if (qualityLightInfo.light.type == LightType.Point)
							{
								num += 2;
								num = Mathf.Clamp(num, 0, 4);
							}
							bool flag8 = true;
							if (stringValueAsInt2 < 3 && PLNetworkManager.Instance.ViewedPawn != null)
							{
								float num2 = 10f + qualityLightInfo.light.range * (float)(2 + stringValueAsInt3);
								if (PLNetworkManager.Instance.ViewedPawn.GetPlayer() != null && PLNetworkManager.Instance.ViewedPawn.GetPlayer().OnPlanet)
								{
									num2 *= 2f;
								}
								flag8 = Vector3.SqrMagnitude(qualityLightInfo.light.transform.position - PLNetworkManager.Instance.ViewedPawn.transform.position) < num2 * num2;
							}
							if (stringValueAsInt3 < num - 1 || !flag8)
							{
								if (qualityLightInfo.light.shadows != LightShadows.None)
								{
									qualityLightInfo.light.shadows = LightShadows.None;
								}
							}
							else if (qualityLightInfo.light.shadows != qualityLightInfo.originalShadows)
							{
								qualityLightInfo.light.shadows = qualityLightInfo.originalShadows;
							}
						}
					}
					bool flag9 = false;
					if ((UnityEngine.Random.Range(0, 1000) == 0 || stringValueAsInt2 != __instance.cachedDOIQualityLevel || __instance.cachedDOICount != __instance.AllDOI.Count || flag6 != __instance.cachedDOIViewedPawnOnPlanet || __instance.cachedDOIViewedPawn != PLNetworkManager.Instance.ViewedPawn) && PLNetworkManager.Instance.ViewedPawn != null && PLNetworkManager.Instance.ViewedPawn.GetPlayer() != null)
					{
						flag9 = true;
						for (int j = 0; j < __instance.AllDOI.Count; j++)
						{
							DisableObjectInfo disableObjectInfo = __instance.AllDOI[j];
							bool flag10 = PLNetworkManager.Instance.ViewedPawn.GetPlayer().OnPlanet ^ disableObjectInfo.isOnShip;
							disableObjectInfo.myRenderer.enabled = flag10 && stringValueAsInt2 > disableObjectInfo.minQualityLevel;
						}
					}
					bool flag11 = false;
					if ((UnityEngine.Random.Range(0, 1000) == 0 || stringValueAsInt2 != __instance.cachedDLIQualityLevel || stringValueAsInt3 != __instance.cachedDLIShadowQualityLevel || __instance.cachedDLICount != __instance.AllDLI.Count || flag5 != __instance.cachedDLIShowInterior || flag6 != __instance.cachedDLIViewedPawnOnPlanet || Vector3.SqrMagnitude(__instance.cachedDLICameraPos - PLCameraSystem.GetCenterEyeAnchor().position) > 1f || __instance.cachedDLIViewedPawn != PLNetworkManager.Instance.ViewedPawn) && PLNetworkManager.Instance != null && PLNetworkManager.Instance.ViewedPawn != null && PLNetworkManager.Instance.ViewedPawn.GetPlayer() != null)
					{
						flag11 = true;
						for (int k = 0; k < __instance.AllDLI.Count; k++)
						{
							DisableLightInfo disableLightInfo = __instance.AllDLI[k];
							bool flag12 = flag6 ^ disableLightInfo.isOnShip;
							if (stringValueAsInt2 > disableLightInfo.minQualityLevel && flag5)
							{
								bool flag13 = true;
								if (stringValueAsInt2 < 3)
								{
									float num3 = 11f + disableLightInfo.light.range * (float)(3 + stringValueAsInt2);
									if (!disableLightInfo.isOnShip)
									{
										num3 *= 2f;
									}
									flag13 = Vector3.SqrMagnitude(disableLightInfo.light.transform.position - PLCameraSystem.GetCenterEyeAnchor().position) < num3 * num3;
								}
								disableLightInfo.light.enabled = flag12 && flag13;
							}
							else
							{
								disableLightInfo.light.enabled = false;
							}
						}
					}
					if (flag11)
					{
						__instance.cachedDLIShowInterior = flag5;
						__instance.cachedDLICount = __instance.AllDLI.Count;
						__instance.cachedDLICameraPos = PLCameraSystem.GetCenterEyeAnchor().position;
						__instance.cachedDLIQualityLevel = stringValueAsInt2;
						__instance.cachedDLIShadowQualityLevel = stringValueAsInt3;
						__instance.cachedDLIViewedPawnOnPlanet = flag6;
						__instance.cachedDLIViewedPawn = PLNetworkManager.Instance.ViewedPawn;
					}
					if (flag9)
					{
						__instance.cachedDOICount = __instance.AllDOI.Count;
						__instance.cachedDOIQualityLevel = stringValueAsInt2;
						__instance.cachedDOIViewedPawnOnPlanet = flag6;
						__instance.cachedDOIViewedPawn = PLNetworkManager.Instance.ViewedPawn;
					}
					if (flag7)
					{
						__instance.cachedQLICount = __instance.AllQLI.Count;
						__instance.cachedQLIQualityLevel = stringValueAsInt2;
						__instance.cachedQLIShadowQualityLevel = stringValueAsInt3;
						__instance.cachedQLIViewedPawnOnPlanet = flag6;
						__instance.cachedQLIViewedPawn = PLNetworkManager.Instance.ViewedPawn;
					}
				}
				state = "2";
				if (PLNetworkManager.Instance.ViewedPawn != null && PLNetworkManager.Instance.ViewedPawn.CurrentShip == null && PLNetworkManager.Instance.ViewedPawn.MyCurrentTLI != null)
				{
					PLMusic.SetState("Location", "planet");
				}
				else if (PLNetworkManager.Instance.ViewedPawn != null && PLNetworkManager.Instance.ViewedPawn.CurrentShip != null && PLCameraSystem.Instance.CurrentCameraMode != null && PLCameraSystem.Instance.CurrentCameraMode.ShouldShowInterior())
				{
					PLMusic.SetState("Location", "internal");
				}
				else
				{
					PLMusic.SetState("Location", "external");
				}
				state = "3";
				PLShipInfo plshipInfo = ((PLNetworkManager.Instance.MyLocalPawn != null) ? PLNetworkManager.Instance.MyLocalPawn.CurrentShip : null);
				if (plshipInfo == null && PLAcademyShipInfo.Instance != null)
				{
					plshipInfo = PLAcademyShipInfo.Instance;
				}
				state = "4";
				if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.LocalPlayer != null)
				{
					float num4 = 4f;
					float num5 = 4f;
					float num6 = 4f;
					float num7 = 4f;
					float num8 = 4f;
					float num9 = 4f;
					float num10 = 4f;
					float num11 = 9f;
					float num12 = 3f;
					float num13 = 9f;
					float num14 = 9f;
					float num15 = 4f;
					float num16 = 16f;
					float num17 = 13f;
					float num18 = 9f;
					float num19 = 9f;
					float num20 = 9f;
					float num21 = 6f;
					PLInteriorDoor plinteriorDoor = null;
					PLPickupObject plpickupObject = null;
					PLPickupComponent plpickupComponent = null;
					PLPickupRandomComponent plpickupRandomComponent = null;
					PLExosuitVisualAsset plexosuitVisualAsset = null;
					PLNeuralRewriter plneuralRewriter = null;
					PLAppearanceStation plappearanceStation = null;
					PLFluffyOven plfluffyOven = null;
					DroppedItemVisual droppedItemVisual = null;
					PLDialogueActorInstance pldialogueActorInstance = null;
					PLUIScreen pluiscreen = null;
					PLPawn plpawn = null;
					PLLockedSeamlessDoor pllockedSeamlessDoor = null;
					Transform transform = null;
					PLLiarsDiceGame plliarsDiceGame = null;
					PLAmmoRefill plammoRefill = null;
					PLSylvassiCypher_ControlStation plsylvassiCypher_ControlStation = null;
					PLBatteryInput plbatteryInput = null;
					PLHullHealSwitch plhullHealSwitch = null;
					PLLCChair pllcchair = null;
					PLAbyssMissileRefillModule plabyssMissileRefillModule = null;
					float num22 = 9f;
					if (PLNetworkManager.Instance.MyLocalPawn != null && PLAbyssShipInfo.Instance != null && PLAbyssShipInfo.Instance.MissileRefillModule != null)
					{
						float sqrMagnitude = (PLAbyssShipInfo.Instance.MissileRefillModule.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
						if (sqrMagnitude < num22)
						{
							plabyssMissileRefillModule = PLAbyssShipInfo.Instance.MissileRefillModule;
						}
					}
					RaycastHit raycastHit = default(RaycastHit);
					Vector3 forward = PLCameraSystem.GetCenterEyeAnchor().forward;
					if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.RaceID != 2)
					{
						for (int l = 0; l < PLExosuitVisualAsset.AllEVAs.Count; l++)
						{
							PLExosuitVisualAsset plexosuitVisualAsset2 = PLExosuitVisualAsset.AllEVAs[l];
							if (plexosuitVisualAsset2 != null && plexosuitVisualAsset2.MyShipInfo == PLNetworkManager.Instance.MyLocalPawn.CurrentShip && (plexosuitVisualAsset2.MyShipInfo == null || plexosuitVisualAsset2.MyShipInfo.GetIsPlayerShip()))
							{
								float sqrMagnitude2 = (plexosuitVisualAsset2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
								if (sqrMagnitude2 < num7)
								{
									num7 = sqrMagnitude2;
									plexosuitVisualAsset = plexosuitVisualAsset2;
								}
							}
						}
					}
					state = "5";
					if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip.GetIsPlayerShip())
					{
						transform = PLNetworkManager.Instance.MyLocalPawn.CurrentShip.WeapUpgradeUIWorldRoot;
						num14 = Vector3.SqrMagnitude(PLNetworkManager.Instance.MyLocalPawn.transform.position - transform.position);
					}
					foreach (PLPawn plpawn2 in __instance.AllPawns)
					{
						if (plpawn2 != null && plpawn2.TeamID == 0 && plpawn2.photonView != null && plpawn2.GetPlayer() != null && plpawn2.GetPlayer() != PLNetworkManager.Instance.LocalPlayer && plpawn2.GetPlayer().RaceID == 0 && plpawn2.IsDead && plpawn2.GetPlayer().CanBeRevived)
						{
							float sqrMagnitude3 = (plpawn2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
							if (sqrMagnitude3 < num13)
							{
								num13 = sqrMagnitude3;
								plpawn = plpawn2;
							}
						}
					}
					state = "6";
					if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.MyLocalPawn.GetPlayer() != null && PLNetworkManager.Instance.MyLocalPawn.GetPlayer().OnPlanet)
					{
						foreach (PLSylvassiCypher_ControlStation plsylvassiCypher_ControlStation2 in PLSylvassiCypher_ControlStation.All)
						{
							if (plsylvassiCypher_ControlStation2 != null)
							{
								float sqrMagnitude4 = (plsylvassiCypher_ControlStation2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
								if (sqrMagnitude4 < num19)
								{
									num19 = sqrMagnitude4;
									plsylvassiCypher_ControlStation = plsylvassiCypher_ControlStation2;
								}
							}
						}
					}
					state = "7";
					if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.MyLocalPawn.GetPlayer() != null)
					{
						foreach (PLBatteryInput plbatteryInput2 in PLBatteryInput.All)
						{
							if (plbatteryInput2 != null && !plbatteryInput2.CurrentStatus)
							{
								float sqrMagnitude5 = (plbatteryInput2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
								if (sqrMagnitude5 < num20)
								{
									num20 = sqrMagnitude5;
									plbatteryInput = plbatteryInput2;
								}
							}
						}
					}
					state = "8";
					if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.LocalPlayer != null && PLLCChair.Instance != null && (PLLCChair.Instance.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude < 4f)
					{
						pllcchair = PLLCChair.Instance.Check("PLLCChair.Instance");
					}
					state = "9";
					if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.MyLocalPawn.GetPlayer() != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip.HullHealSwitch != null)
					{
						float sqrMagnitude6 = (PLNetworkManager.Instance.MyLocalPawn.CurrentShip.HullHealSwitch.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
						if (sqrMagnitude6 < num21)
						{
							num21 = sqrMagnitude6;
							plhullHealSwitch = PLNetworkManager.Instance.MyLocalPawn.CurrentShip.HullHealSwitch;
						}
					}
					state = "10";
					if (PLNetworkManager.Instance.MyLocalPawn != null && plshipInfo != null && (plshipInfo.GetIsPlayerShip() || plshipInfo.ShipTypeID == EShipType.E_ACADEMY))
					{
						for (int m = 0; m < plshipInfo.MyScreenBase.AllScreens.Count; m++)
						{
							PLUIScreen pluiscreen2 = plshipInfo.MyScreenBase.AllScreens[m];
							if (pluiscreen2 != null && pluiscreen2.GetTargetScreenID() == 10)
							{
								float sqrMagnitude7 = (pluiscreen2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
								if (sqrMagnitude7 < num12)
								{
									num12 = sqrMagnitude7;
									pluiscreen = pluiscreen2;
								}
							}
						}
						for (int n = 0; n < PLClonedScreen.AllClonedScreens.Count; n++)
						{
							PLClonedScreen plclonedScreen = PLClonedScreen.AllClonedScreens[n];
							if (plclonedScreen != null && plclonedScreen.GetTargetScreenID() == 10 && plclonedScreen.MyScreenHubBase == plshipInfo.MyScreenBase)
							{
								float sqrMagnitude8 = (plclonedScreen.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
								if (sqrMagnitude8 < num12)
								{
									num12 = sqrMagnitude8;
									pluiscreen = plclonedScreen;
								}
							}
						}
					}
					state = "11";
					if (PLNetworkManager.Instance.MyLocalPawn != null)
					{
						foreach (PLFluffyOven plfluffyOven2 in __instance.AllOvens)
						{
							if (plfluffyOven2 != null)
							{
								float sqrMagnitude9 = (plfluffyOven2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
								if (sqrMagnitude9 < num7)
								{
									plfluffyOven = plfluffyOven2;
								}
							}
						}
						foreach (DroppedItemVisual droppedItemVisual2 in __instance.Displayed_DroppedItemVisuals)
						{
							if (droppedItemVisual2 != null && droppedItemVisual2.Visual != null)
							{
								float sqrMagnitude10 = (droppedItemVisual2.Visual.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
								Vector3 normalized = (droppedItemVisual2.Visual.transform.position - PLCameraSystem.GetCenterEyeAnchor().position).normalized;
								if (sqrMagnitude10 < num8 && Vector3.Dot(normalized, forward) > 0.985f)
								{
									num8 = sqrMagnitude10;
									droppedItemVisual = droppedItemVisual2;
								}
							}
						}
					}
					state = "12";
					for (int num23 = 0; num23 < __instance.m_NeuralRewriters.Count; num23++)
					{
						PLNeuralRewriter plneuralRewriter2 = __instance.m_NeuralRewriters[num23];
						if (plneuralRewriter2 != null && plneuralRewriter2.gameObject.activeInHierarchy)
						{
							float sqrMagnitude11 = (plneuralRewriter2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
							if (sqrMagnitude11 < num9)
							{
								num9 = sqrMagnitude11;
								plneuralRewriter = plneuralRewriter2;
							}
						}
					}
					state = "13";
					if (PLNetworkManager.Instance.MainMenu.GetActiveMenuCount() == 0)
					{
						for (int num24 = 0; num24 < __instance.m_AppearanceStations.Count; num24++)
						{
							PLAppearanceStation plappearanceStation2 = __instance.m_AppearanceStations[num24];
							if (plappearanceStation2 != null && plappearanceStation2.gameObject.activeInHierarchy)
							{
								float sqrMagnitude12 = (plappearanceStation2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
								if (sqrMagnitude12 < num10)
								{
									num10 = sqrMagnitude12;
									plappearanceStation = plappearanceStation2;
								}
							}
						}
					}
					state = "14";
					for (int num25 = 0; num25 < __instance.m_AllPickupObjects.Count; num25++)
					{
						PLPickupObject plpickupObject2 = __instance.m_AllPickupObjects[num25];
						if (plpickupObject2 != null && plpickupObject2.PickupID != -1 && plpickupObject2.gameObject.activeInHierarchy)
						{
							float sqrMagnitude13 = (plpickupObject2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
							Vector3 normalized2 = (plpickupObject2.transform.position - PLCameraSystem.GetCenterEyeAnchor().position).normalized;
							if (sqrMagnitude13 < num4 && Vector3.Dot(normalized2, forward) > 0.985f)
							{
								num4 = sqrMagnitude13;
								plpickupObject = plpickupObject2;
							}
						}
					}
					state = "15";
					for (int num26 = 0; num26 < __instance.m_AllPickupComponents.Count; num26++)
					{
						PLPickupComponent plpickupComponent2 = __instance.m_AllPickupComponents[num26];
						if (plpickupComponent2 != null && plpickupComponent2.PickupID != -1 && plpickupComponent2.gameObject.activeInHierarchy)
						{
							float num27 = (plpickupComponent2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
							num27 /= plpickupComponent2.PickupRangeModifier;
							if (num27 < num5)
							{
								num5 = num27;
								plpickupComponent = plpickupComponent2;
							}
						}
					}
					state = "16";
					for (int num28 = 0; num28 < __instance.m_AllPickupRandomComponents.Count; num28++)
					{
						PLPickupRandomComponent plpickupRandomComponent2 = __instance.m_AllPickupRandomComponents[num28];
						if (plpickupRandomComponent2 != null && plpickupRandomComponent2.PickupID != -1 && plpickupRandomComponent2.gameObject.activeInHierarchy && plpickupRandomComponent2.RandCompSetup)
						{
							float sqrMagnitude14 = (plpickupRandomComponent2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
							if (sqrMagnitude14 < num6)
							{
								num6 = sqrMagnitude14;
								plpickupRandomComponent = plpickupRandomComponent2;
							}
						}
					}
					state = "17";
					for (int num29 = 0; num29 < __instance.m_AmmoRefills.Count; num29++)
					{
						PLAmmoRefill plammoRefill2 = __instance.m_AmmoRefills[num29];
						if (plammoRefill2 != null && plammoRefill2.ClipAvailable && plammoRefill2.MyTLI != null && plammoRefill2.MyTLI.MyShipInfo != null && plammoRefill2.MyTLI.MyShipInfo == plshipInfo)
						{
							float sqrMagnitude15 = (plammoRefill2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
							Vector3 normalized3 = (plammoRefill2.GetAmmoRefillClip_WorldPos() - PLNetworkManager.Instance.MyLocalPawn.PawnCamera.transform.position).normalized;
							if (sqrMagnitude15 < num18 && Vector3.Dot(PLNetworkManager.Instance.MyLocalPawn.PawnCamera.transform.forward, normalized3) > 0.9f)
							{
								num18 = sqrMagnitude15;
								plammoRefill = plammoRefill2;
							}
						}
					}
					state = "18";
					for (int num30 = 0; num30 < PLInteriorDoor.AllInteriorDoors.Count; num30++)
					{
						PLInteriorDoor plinteriorDoor2 = PLInteriorDoor.AllInteriorDoors[num30];
						if (plinteriorDoor2 != null)
						{
							float sqrMagnitude16 = (plinteriorDoor2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
							Vector3 normalized4 = (plinteriorDoor2.transform.position - PLNetworkManager.Instance.MyLocalPawn.PawnCamera.transform.position).normalized;
							if (sqrMagnitude16 < num15)
							{
								num15 = sqrMagnitude16;
								plinteriorDoor = plinteriorDoor2;
							}
						}
					}
					state = "19";
					if (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.CurrentlyInLiarsDiceGame == null)
					{
						for (int num31 = 0; num31 < PLLiarsDiceGame.AllGames.Count; num31++)
						{
							PLLiarsDiceGame plliarsDiceGame2 = PLLiarsDiceGame.AllGames[num31];
							if (plliarsDiceGame2 != null && plliarsDiceGame2.LocalPlayerCanUse && plliarsDiceGame2.LocalPlayerCanJoinRightNow())
							{
								Vector3 vector = plliarsDiceGame2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position;
								vector.y *= 1.4f;
								float sqrMagnitude17 = vector.sqrMagnitude;
								if (sqrMagnitude17 < num17)
								{
									num17 = sqrMagnitude17;
									plliarsDiceGame = plliarsDiceGame2;
								}
							}
						}
					}
					state = "20";
					for (int num32 = 0; num32 < PLLockedSeamlessDoor.AllDoors.Count; num32++)
					{
						PLLockedSeamlessDoor pllockedSeamlessDoor2 = PLLockedSeamlessDoor.AllDoors[num32];
						if (pllockedSeamlessDoor2 != null && !pllockedSeamlessDoor2.IsOpen())
						{
							float sqrMagnitude18 = (pllockedSeamlessDoor2.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position).sqrMagnitude;
							Vector3 normalized5 = (pllockedSeamlessDoor2.transform.position - PLNetworkManager.Instance.MyLocalPawn.PawnCamera.transform.position).normalized;
							if (sqrMagnitude18 < num16)
							{
								num16 = sqrMagnitude18;
								pllockedSeamlessDoor = pllockedSeamlessDoor2;
							}
						}
					}
					state = "21";
					CargoObjectDisplay cargoObjectDisplay = null;
					if (plshipInfo != null)
					{
						PLShipStats myStats = plshipInfo.MyStats;
						List<PLShipComponent> componentsOfType = myStats.GetComponentsOfType(ESlotType.E_COMP_CARGO, true);
						componentsOfType.AddRange(myStats.GetComponentsOfType(ESlotType.E_COMP_HIDDENCARGO, true));
						float num33 = 4.5f;
						for (int num34 = 0; num34 < componentsOfType.Count; num34++)
						{
							PLShipComponent plshipComponent = componentsOfType[num34];
							CargoObjectDisplay cargoObjectDisplay2;
							if (plshipComponent.VisualSlotType == ESlotType.E_COMP_HIDDENCARGO)
							{
								cargoObjectDisplay2 = plshipInfo.GetHiddenCODDisplayingCargoAtID(plshipComponent, plshipComponent.SortID);
							}
							else
							{
								cargoObjectDisplay2 = plshipInfo.GetCODDisplayingCargoAtID(plshipComponent, plshipComponent.SortID);
							}
							if (cargoObjectDisplay2 != null && cargoObjectDisplay2.RootObj != null && cargoObjectDisplay2.DisplayedItem != null && !cargoObjectDisplay2.DisplayedItem.IsFlaggedForSelfDestruction())
							{
								float num35 = Vector3.SqrMagnitude(cargoObjectDisplay2.RootObj.transform.position - PLNetworkManager.Instance.MyLocalPawn.transform.position);
								if (num35 < num33)
								{
									cargoObjectDisplay = cargoObjectDisplay2;
									num33 = num35;
								}
							}
						}
					}
					state = "22";
					PLCaptainsChair plcaptainsChair = null;
					if (plshipInfo != null && plshipInfo.CaptainsChairPivot != null)
					{
						PLCaptainsChair shipComponent = plshipInfo.MyStats.GetShipComponent<PLCaptainsChair>(ESlotType.E_COMP_CAPTAINS_CHAIR, false);
						if (shipComponent != null && shipComponent.MyInstance != null && Vector3.SqrMagnitude(PLNetworkManager.Instance.MyLocalPawn.transform.position - shipComponent.MyInstance.transform.position) < 4f)
						{
							plcaptainsChair = shipComponent;
						}
					}
					state = "23";
					bool flag14 = false;
					if (PLEncounterManager.Instance.PlayerShip != null)
					{
						PLSlot slot = PLEncounterManager.Instance.PlayerShip.MyStats.GetSlot(ESlotType.E_COMP_CARGO);
						if (slot != null && slot.Count < slot.MaxItems)
						{
							flag14 = true;
						}
					}
					state = "24";
					float num36 = float.MaxValue;
					if (PLNetworkManager.Instance.MyLocalPawn != null && PLLCChair.Instance != null && PLLCChair.Instance.InjectionPayloadVisual != null)
					{
						num36 = Vector3.SqrMagnitude(PLNetworkManager.Instance.MyLocalPawn.transform.position - PLLCChair.Instance.InjectionPayloadVisual.transform.position);
					}
					float num37 = float.MaxValue;
					if (PLNetworkManager.Instance.MyLocalPawn != null && plshipInfo != null && plshipInfo.ResearchLockerWorldRoot != null && (plshipInfo == PLEncounterManager.Instance.PlayerShip || plshipInfo.ShipTypeID == EShipType.E_ACADEMY))
					{
						num37 = Vector3.SqrMagnitude(PLNetworkManager.Instance.MyLocalPawn.transform.position - plshipInfo.ResearchLockerWorldRoot.position);
					}
					float num38 = float.MaxValue;
					if (PLNetworkManager.Instance.MyLocalPawn != null && plshipInfo != null && plshipInfo.ShipLogStation != null && (plshipInfo.ShipTypeID == EShipType.E_ACADEMY || plshipInfo == PLEncounterManager.Instance.PlayerShip))
					{
						num38 = Vector3.SqrMagnitude(PLNetworkManager.Instance.MyLocalPawn.transform.position - plshipInfo.ShipLogStation.position);
					}
					float num39 = float.MaxValue;
					if (PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip != null && PLNetworkManager.Instance.MyLocalPawn.CurrentShip == PLEncounterManager.Instance.PlayerShip)
					{
						foreach (PLLocker pllocker in PLNetworkManager.Instance.MyLocalPawn.CurrentShip.GetLockers())
						{
							if (pllocker != null && pllocker.Base != null)
							{
								float num41 = Vector3.SqrMagnitude(PLNetworkManager.Instance.MyLocalPawn.transform.position - pllocker.Base.transform.position);
								if (num41 < num39)
								{
									num39 = num41;
								}
							}
						}
					}
					state = "25";
					bool flag15 = false;
					if (PLTabMenu.Instance.TargetContainer != null && (Mathf.Min(new float[] { num37, num39, num36 }) > 16f || PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station) || PLInput.Instance.GetButtonDown(PLInputBase.EInputActionName.prev_menu) || PLInput.Instance.GetButtonDown(PLInputBase.EInputActionName.menu) || PLInput.Instance.GetButtonDown(PLInputBase.EInputActionName.tabmenu)))
					{
						PLTabMenu.Instance.TargetContainer = null;
						flag15 = true;
					}
					if (PLNetworkManager.Instance.MyLocalPawn != null && (PLNetworkManager.Instance.MyLocalPawn.CurrentShip == null || PLAbyssShipInfo.Instance != null))
					{
						for (int num42 = 0; num42 < PLDialogueActorInstance.AllDialogueActorInstances.Count; num42++)
						{
							PLDialogueActorInstance pldialogueActorInstance2 = PLDialogueActorInstance.AllDialogueActorInstances[num42];
							if (pldialogueActorInstance2 != null && pldialogueActorInstance2.enabled && pldialogueActorInstance2.gameObject.activeInHierarchy && pldialogueActorInstance2.ActorTypeData != null)
							{
								float sqrMagnitude19 = (PLNetworkManager.Instance.MyLocalPawn.transform.position - pldialogueActorInstance2.transform.position).sqrMagnitude;
								if (sqrMagnitude19 < pldialogueActorInstance2.MaxRange * pldialogueActorInstance2.MaxRange && sqrMagnitude19 < num11)
								{
									num11 = sqrMagnitude19;
									pldialogueActorInstance = pldialogueActorInstance2;
								}
							}
						}
					}
					string text = "";
					for (int num43 = PLNetworkManager.Instance.LocalPlayer.GetMaxScrapProcessingAttempts() - 1; num43 >= 0; num43--)
					{
						if (num43 < PLNetworkManager.Instance.LocalPlayer.ScrapProcessingAttemptsLeft)
						{
							text += "[-] ";
						}
						else
						{
							text += "<color=red>[-]</color> ";
						}
					}
					state = "26";
					PLLocker pllocker2 = null;
					if (num39 < 12f && PLTabMenu.Instance.TargetContainer == null && !flag15 && Physics.Raycast(new Ray(PLCameraSystem.Instance.CurrentSubSystem.LocalPawnCameras[0].transform.position, PLCameraSystem.Instance.CurrentSubSystem.LocalPawnCameras[0].transform.forward), out raycastHit, 8f, 2048))
					{
						foreach (PLLocker pllocker3 in PLNetworkManager.Instance.MyLocalPawn.CurrentShip.GetLockers())
						{
							if (raycastHit.collider == pllocker3.Base)
							{
								pllocker2 = pllocker3;
								pllocker2.LastIsCursorOverTime = Time.frameCount;
								break;
							}
						}
					}
					state = "27";
					bool flag16 = false;
					if (!PLNetworkManager.Instance.IsTyping && PLNetworkManager.Instance.MyLocalPawn != null && PLNetworkManager.Instance.MyLocalPawn.GetPlayer() != null && PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory != null && PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory.ActiveItem != null && PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory.ActiveItem.UsesAmmo && PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory.ActiveItem.AmmoMax > PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory.ActiveItem.AmmoCurrent)
					{
						if (UnityEngine.Random.value < 0.01f || __instance.shouldUpdateClips)
						{
							__instance.shouldUpdateClips = false;
							__instance.CurrentPlayer_Clips.Clear();
							__instance.CurrentPlayer_Clips.AddRange(PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory.GetPawnItemsOfType<PLPawnItem_AmmoClip>());
						}
						if (__instance.CurrentPlayer_Clips.Count > 0)
						{
							flag16 = true;
						}
					}
					state = "28";
					if (!(PLTabMenu.Instance.TargetContainer != null) && !flag15)
					{
						if (pllcchair != null)
						{
							if (pllcchair.PlayerIDInChair == -1)
							{
								if (PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0)
								{
									PLGlobal.Instance.SetBottomInfo(PLLocalize.Localize("Sit In Chair", false), "", "activate_station", "");
									if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
									{
										if (PLLCChairUI.Instance != null)
										{
											PLLCChairUI.Instance.OnLocalPlayerEntered();
										}
										pllcchair.LocallyChangedTime_PlayerIDInChair = Time.time;
										pllcchair.photonView.RPC("SetPlayerIDInChair", PhotonTargets.All, new object[] { PLNetworkManager.Instance.LocalPlayer.GetPlayerID() });
									}
								}
								else
								{
									PLGlobal.Instance.SetBottomInfo(PLLocalize.Localize("Only the Captain can use __instance", false), "", "", "");
								}
							}
							else if (pllcchair.PlayerIDInChair == PLNetworkManager.Instance.LocalPlayerID)
							{
								PLGlobal.Instance.SetBottomInfo(PLLocalize.Localize("Leave Chair", false), "", "activate_station", "");
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
								{
									pllcchair.photonView.RPC("SetPlayerIDInChair", PhotonTargets.All, new object[] { -1 });
									pllcchair.LocallyChangedTime_PlayerIDInChair = Time.time;
								}
							}
						}
						else if (plabyssMissileRefillModule != null)
						{
							float num44 = 2f;
							int num45 = 40;
							if (PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0)
							{
								if (PLServer.Instance != null && PLServer.Instance.CurrentUpgradeMats > 0)
								{
									if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > 0.1f)
									{
										StringBuilder stringBuilder = new StringBuilder();
										stringBuilder.Append(PLLocalize.Localize("ACTIVATING TORPEDO REFILL", false));
										stringBuilder.Append("\n");
										stringBuilder.Append("[");
										float num46 = Mathf.Clamp01(PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) / num44);
										for (int num47 = 0; num47 < num45; num47++)
										{
											float num48 = (float)num47 / (float)num45;
											if (num46 >= num48)
											{
												stringBuilder.Append("|");
											}
											else
											{
												stringBuilder.Append(" ");
											}
										}
										stringBuilder.Append("]");
										PLGlobal.Instance.SetBottomInfo("", stringBuilder.ToString(), "", "");
									}
									else
									{
										PLGlobal.Instance.SetBottomInfo(PLLocalize.Localize("Uses 5 processed scrap to refill torpedoes", false), PLLocalize.Localize("(hold) Refill Torpedos", false), "", "activate_station");
									}
									if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > num44)
									{
										PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.activate_station);
										plabyssMissileRefillModule.OptionalShipInfo.photonView.RPC("RefillTorpedoes", PhotonTargets.All, Array.Empty<object>());
									}
								}
								else
								{
									PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("No processed scrap to consume", false), "", "");
								}
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Must be the Captain to use __instance system", false), "", "");
							}
						}
						else if (plhullHealSwitch != null && plhullHealSwitch.OptionalShipInfo != null)
						{
							float num49 = 2f;
							int num50 = 40;
							if (PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0)
							{
								if (plhullHealSwitch.OptionalShipInfo.MyHull == null || plhullHealSwitch.OptionalShipInfo.MyHull.Current >= plhullHealSwitch.OptionalShipInfo.MyStats.HullMax)
								{
									PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Hull does not need repair", false), "", "");
								}
								else if (PLServer.Instance != null && PLServer.Instance.CurrentUpgradeMats > 0)
								{
									if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > 0.1f)
									{
										StringBuilder stringBuilder2 = new StringBuilder();
										stringBuilder2.Append(PLLocalize.Localize("ACTIVATING HULL REINFORCMENT", false));
										stringBuilder2.Append("\n");
										stringBuilder2.Append("[");
										float num51 = Mathf.Clamp01(PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) / num49);
										for (int num52 = 0; num52 < num50; num52++)
										{
											float num53 = (float)num52 / (float)num50;
											if (num51 >= num53)
											{
												stringBuilder2.Append("|");
											}
											else
											{
												stringBuilder2.Append(" ");
											}
										}
										stringBuilder2.Append("]");
										PLGlobal.Instance.SetBottomInfo("", stringBuilder2.ToString(), "", "");
										plhullHealSwitch.LitUpAmt_SideLights = num51;
									}
									else
									{
										PLGlobal.Instance.SetBottomInfo(PLLocalize.Localize("Uses 1 processed scrap to repair the hull (+200 hull / scrap)", false), PLLocalize.Localize("(hold) Reinforce Hull", false), "", "activate_station");
									}
									if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > num49)
									{
										PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.activate_station);
										plhullHealSwitch.OptionalShipInfo.photonView.RPC("HullHealSwitchPulled", PhotonTargets.All, Array.Empty<object>());
									}
								}
								else
								{
									PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("No processed scrap to consume", false), "", "");
								}
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Must be the Captain to use __instance system", false), "", "");
							}
						}
						else if (plbatteryInput != null)
						{
							PLGlobal.Instance.SetBottomInfo("Insert Battery", "", "activate_station", "");
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station) && PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.MyInventory != null && PLNetworkManager.Instance.LocalPlayer.MyInventory.GetPawnItemOfType<PLPawnItem_Battery>() != null)
							{
								PLNetworkManager.Instance.LocalPlayer.photonView.RPC("Request_UseBatteryInput", PhotonTargets.All, new object[] { plbatteryInput.photonView.viewID });
							}
						}
						else if (PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress != null)
						{
							if (PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress.DisplayedItem != null)
							{
								string text2 = "";
								StringBuilder stringBuilder3 = new StringBuilder();
								stringBuilder3.Append(PLLocalize.Localize("Processing ", false));
								stringBuilder3.Append(PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress.DisplayedItem.Name);
								stringBuilder3.Append("\n");
								float num54 = Mathf.Clamp01(PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.process_scrap) / __instance.ScrapMiniGameTime);
								int num55 = 13;
								for (int num56 = 0; num56 < 26; num56++)
								{
									float num57 = (float)num56 / 26f;
									if (num56 == num55)
									{
										if (Time.time % 0.25f < 0.125f)
										{
											stringBuilder3.Append("<color=#00FF00>|</color>");
										}
										else
										{
											stringBuilder3.Append("<color=#AAAAAA>|</color>");
										}
									}
									else if (num54 >= num57)
									{
										stringBuilder3.Append("|");
									}
									else
									{
										stringBuilder3.Append("<color=#000000>|</color>");
									}
								}
								PLGlobal.Instance.SetBottomInfo(text2, stringBuilder3.ToString() + "\n" + text, "", "");
								float heldDownTime = PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.process_scrap);
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.process_scrap) && Mathf.Abs(heldDownTime - __instance.ScrapMiniGameTime * 0.5f) < 0.13f && Time.time - __instance.LastAttemptToTransferNeutralCargoTime > 1f)
								{
									__instance.LastAttemptToTransferNeutralCargoTime = Time.time;
									PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.process_scrap);
									PLInGameUI.Instance.ProcessedScrapSuccess_Label.text = "+" + (PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress.DisplayedItem.Level + 1).ToString();
									PLInGameUI.Instance.LastSuccessfulProcessedScrapTime = Time.time;
									PLNetworkManager.Instance.LocalPlayer.photonView.RPC("AttemptToProcessScrapCargo", PhotonTargets.MasterClient, new object[]
									{
							plshipInfo.ShipID,
							PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress.DisplayedItem.NetID
									});
									__instance.ScrapMiniGameTime = UnityEngine.Random.Range(1.5f, 2.5f);
									PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress = null;
									PLMusic.PostEvent("play_ship_generic_internal_computer_ui_complete", PLNetworkManager.Instance.MyLocalPawn.gameObject);
								}
								else if ((heldDownTime > __instance.ScrapMiniGameTime || PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.process_scrap) || !PLInput.Instance.GetButton(PLInputBase.EInputActionName.process_scrap)) && heldDownTime > 0.5f)
								{
									PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.process_scrap);
									PLPlayer localPlayer = PLNetworkManager.Instance.LocalPlayer;
									int num40 = localPlayer.ScrapProcessingAttemptsLeft;
									localPlayer.ScrapProcessingAttemptsLeft = num40 - 1;
									PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress = null;
									PLTabMenu.Instance.TimedErrorMsg = PLLocalize.Localize("Scrap Processing Failed!", false);
									PLMusic.PostEvent("play_ship_generic_internal_computer_ui_error", PLNetworkManager.Instance.MyLocalPawn.gameObject);
								}
								else if (!PLInput.Instance.GetButton(PLInputBase.EInputActionName.process_scrap) || heldDownTime < 0.09f || PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress.DisplayedItem == null || PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress.DisplayedItem.IsFlaggedForSelfDestruction())
								{
									PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.process_scrap);
									PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress = null;
								}
							}
							else
							{
								PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.process_scrap);
								PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress = null;
							}
						}
						else if (PLTabMenu.Instance.DialogueMenu.CurrentActorInstance != null)
						{
							if ((PLNetworkManager.Instance.MyLocalPawn.transform.position - PLTabMenu.Instance.DialogueMenu.CurrentActorInstance.transform.position).sqrMagnitude > PLTabMenu.Instance.DialogueMenu.CurrentActorInstance.MaxRange * PLTabMenu.Instance.DialogueMenu.CurrentActorInstance.MaxRange)
							{
								PLTabMenu.Instance.DialogueMenu.CurrentActorInstance = null;
								PLMusic.PostEvent("play_sx_ui_dialoguebox_close", __instance.gameObject);
							}
						}
						else if (plpawn != null && plpawn.GetPlayer() != null)
						{
							float num58 = 2f;
							int num59 = 40;
							if (Time.time - __instance.LastAttemptToResetTalentPointsTime > 5f)
							{
								if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > 0.1f)
								{
									StringBuilder stringBuilder4 = new StringBuilder();
									stringBuilder4.Append(PLLocalize.Localize("REVIVING ", false));
									stringBuilder4.Append(plpawn.GetPlayer().GetPlayerName(false));
									stringBuilder4.Append("\n");
									stringBuilder4.Append("[");
									float num60 = Mathf.Clamp01(PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) / num58);
									for (int num61 = 0; num61 < num59; num61++)
									{
										float num62 = (float)num61 / (float)num59;
										if (num60 >= num62)
										{
											stringBuilder4.Append("|");
										}
										else
										{
											stringBuilder4.Append(" ");
										}
									}
									stringBuilder4.Append("]");
									PLGlobal.Instance.SetBottomInfo("", stringBuilder4.ToString(), "", "");
								}
								else
								{
									PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("(hold) Revive ", false) + plpawn.GetPlayer().GetPlayerName(false), "", "activate_station");
								}
							}
							else
							{
								PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.activate_station);
							}
							if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > num58)
							{
								__instance.LastAttemptToRevivePlayerTime = Time.time;
								PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.activate_station);
								if (plpawn.GetPlayer().IsBot)
								{
									plpawn.photonView.RPC("Revive", PhotonTargets.MasterClient, new object[] { PLNetworkManager.Instance.LocalPlayerID });
									if (!PhotonNetwork.isMasterClient)
									{
										plpawn.GetPlayer().CanBeRevived = false;
									}
								}
								else if (plpawn.GetPlayer().GetPhotonPlayer() != null)
								{
									plpawn.photonView.RPC("Revive", plpawn.GetPlayer().GetPhotonPlayer(), new object[] { PLNetworkManager.Instance.LocalPlayerID });
									plpawn.GetPlayer().CanBeRevived = false;
								}
							}
						}
						else if (plsylvassiCypher_ControlStation != null)
						{
							PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Activate Controls", false), "", "activate_station");
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
							{
								plsylvassiCypher_ControlStation.OnActivate();
							}
						}
						else if (pluiscreen != null && plshipInfo != null && plshipInfo.SensorDishControllerPlayerID != PLNetworkManager.Instance.LocalPlayerID && plshipInfo.MySensorDish != null && (plshipInfo.GetIsPlayerShip() || plshipInfo.ShipTypeID == EShipType.E_ACADEMY))
						{
							if (Time.unscaledTime - plshipInfo.SensorDish_ControllerLocalChangeTime > 0.2f)
							{
								PLPlayer playerFromPlayerID = PLServer.Instance.GetPlayerFromPlayerID(plshipInfo.SensorDishControllerPlayerID);
								if (playerFromPlayerID == null || playerFromPlayerID.IsBot)
								{
									if (PLNetworkManager.Instance.LocalPlayer.Talents[51] > 0 || plshipInfo.ShipTypeID == EShipType.E_ACADEMY)
									{
										string text3 = "";
										if (playerFromPlayerID != null)
										{
											text3 = "in use by <" + PLReadableStringManager.Instance.GetFormattedResultFromInputString(playerFromPlayerID.GetPlayerName(false)) + ">";
										}
										PLGlobal.Instance.SetBottomInfo(text3, PLLocalize.Localize("Operate Sensor Dish ", false), "", "activate_station");
										if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
										{
											plshipInfo.SensorDishControllerPlayerID = PLNetworkManager.Instance.LocalPlayerID;
											plshipInfo.photonView.RPC("RequestSensorDishController", PhotonTargets.MasterClient, new object[] { PLNetworkManager.Instance.LocalPlayerID });
											PLCameraSystem.Instance.ChangeCameraMode(new PLCameraMode_SensorDish(plshipInfo));
											plshipInfo.SensorDish_ControllerLocalChangeTime = Time.unscaledTime;
										}
									}
									else
									{
										PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("You need the <Sensor Dish Certification> talent to use __instance station", false), "", "");
									}
								}
								else
								{
									PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Sensor Dish in use by <", false) + playerFromPlayerID.GetPlayerName(false) + ">", "", "");
								}
							}
						}
						else if (plpickupRandomComponent != null && !plpickupRandomComponent.PickedUp)
						{
							if (PLNetworkManager.Instance.IsInternalBuild)
							{
								PLGlobal.Instance.SetBottomInfo("", string.Concat(new string[]
								{
						PLLocalize.Localize("Pickup ", false),
						plpickupRandomComponent.GetCompName(),
						" (",
						plpickupRandomComponent.PickupID.ToString(),
						")"
								}), "", "pickup_item");
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Pickup ", false) + plpickupRandomComponent.GetCompName(), "", "pickup_item");
							}
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.pickup_item) && flag14)
							{
								PLNetworkManager.Instance.LocalPlayer.photonView.RPC("AttemptToPickupRandomComponentAtID", PhotonTargets.MasterClient, new object[] { plpickupRandomComponent.PickupID });
								PLNetworkManager.Instance.MyLocalPawn.photonView.RPC("Anim_Pickup", PhotonTargets.Others, Array.Empty<object>());
								PLMusic.PostEvent("play_sx_player_item_pickup_large", PLNetworkManager.Instance.MyLocalPawn.gameObject);
							}
						}
						else if (plammoRefill != null)
						{
							PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Pickup Ammo Clip", false), "", "pickup_item");
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.pickup_item))
							{
								int num63 = Array.IndexOf<PLAmmoRefill>(plammoRefill.MyTLI.MyShipInfo.MyAmmoRefills, plammoRefill);
								if (num63 >= 0 && num63 < plammoRefill.MyTLI.MyShipInfo.MyAmmoRefills.Length)
								{
									plammoRefill.MyTLI.MyShipInfo.photonView.RPC("PlayerRequestsAmmoClip", PhotonTargets.MasterClient, new object[] { num63 });
									if (!PhotonNetwork.isMasterClient)
									{
										plammoRefill.ClipAvailable = false;
									}
								}
							}
						}
						else if (plpickupComponent != null && !plpickupComponent.PickedUp)
						{
							bool flag17 = false;
							if (plpickupComponent.OptionalRace_IfPrize != null)
							{
								flag17 = (PLServer.Instance.RacesWonBitfield & (1 << plpickupComponent.OptionalRace_IfPrize.RaceID)) == 0;
							}
							if (flag17)
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Win Race To Claim Prize: ", false) + plpickupComponent.GetCompName(), "", "");
							}
							else
							{
								if (PLNetworkManager.Instance.IsInternalBuild)
								{
									PLGlobal.Instance.SetBottomInfo("", string.Concat(new string[]
									{
							PLLocalize.Localize("Pickup", false),
							" ",
							plpickupComponent.GetCompName(),
							" (",
							plpickupComponent.PickupID.ToString(),
							")"
									}), "", "pickup_item");
								}
								else
								{
									PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Pickup", false) + " " + plpickupComponent.GetCompName(), "", "pickup_item");
								}
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.pickup_item) && flag14)
								{
									PLNetworkManager.Instance.LocalPlayer.photonView.RPC("AttemptToPickupComponentAtID", PhotonTargets.MasterClient, new object[] { plpickupComponent.PickupID });
									PLNetworkManager.Instance.MyLocalPawn.photonView.RPC("Anim_Pickup", PhotonTargets.Others, Array.Empty<object>());
									PLMusic.PostEvent("play_sx_player_item_pickup_large", PLNetworkManager.Instance.MyLocalPawn.gameObject);
								}
							}
						}
						else if (plpickupObject != null && !plpickupObject.PickedUp)
						{
							if (PLNetworkManager.Instance.IsInternalBuild)
							{
								PLGlobal.Instance.SetBottomInfo("", string.Concat(new string[]
								{
						PLLocalize.Localize("Pickup", false),
						" ",
						plpickupObject.GetItemName(false),
						" (",
						plpickupObject.PickupID.ToString(),
						")"
								}), "", "pickup_item");
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Pickup", false) + " " + plpickupObject.GetItemName(false), "", "pickup_item");
							}
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.pickup_item))
							{
								PLMusic.PostEvent("play_sx_player_item_pickup", PLNetworkManager.Instance.MyLocalPawn.gameObject);
								PLNetworkManager.Instance.MyLocalPawn.photonView.RPC("Anim_Pickup", PhotonTargets.Others, Array.Empty<object>());
								PLNetworkManager.Instance.LocalPlayer.photonView.RPC("AttemptToPickupObjectAtID", PhotonTargets.MasterClient, new object[] { plpickupObject.PickupID });
							}
						}
						else if (droppedItemVisual != null)
						{
							PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Pickup", false) + " " + droppedItemVisual.GetPawnItemName(), "", "pickup_item");
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.pickup_item))
							{
								PLNetworkManager.Instance.LocalPlayer.photonView.RPC("AttemptToPickupDroppedPlayerPawnItem", PhotonTargets.MasterClient, new object[] { droppedItemVisual.DroppedItem.DroppedItemID.GetDecrypted() });
								PLNetworkManager.Instance.MyLocalPawn.photonView.RPC("Anim_Pickup", PhotonTargets.Others, Array.Empty<object>());
								PLMusic.PostEvent("play_sx_player_item_pickup_large", PLNetworkManager.Instance.MyLocalPawn.gameObject);
							}
						}
						else if (pldialogueActorInstance != null)
						{
							if (PLTabMenu.Instance.DialogueMenu.CurrentActorInstance == null && PLTabMenu.Instance.DialogueMenu.TimeNotDisplayed > 0.1f)
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize(pldialogueActorInstance.InteractionText, false) + " " + PLLocalize.Localize(pldialogueActorInstance.DisplayName, false), "", "talk_to_npc");
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.talk_to_npc))
								{
									if (pldialogueActorInstance.ActorTypeData != null)
									{
										pldialogueActorInstance.BeginDialogue();
									}
									PLTabMenu.Instance.DialogueMenu.CurrentActorInstance = pldialogueActorInstance;
									PLTabMenu.Instance.OnToggleTabMenu();
									PLMusic.PostEvent("play_sx_ui_dialoguebox_open", __instance.gameObject);
									PLServer.Instance.photonView.RPC("TalkToNPCOfActorType", PhotonTargets.MasterClient, new object[] { pldialogueActorInstance.ActorName });
								}
							}
						}
						else if (num38 < 6f)
						{
							PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Open Ship Log", false), "", "activate_station");
							if (PLInput.Instance.GetButtonDown(PLInputBase.EInputActionName.activate_station))
							{
								PLTabMenu.Instance.TabMenuActive = true;
								PLTabMenu.Instance.CurrentTabIndex = 0;
								PLTabMenu.Instance.SetCrewPage(3);
							}
						}
						else if (num36 < 9f && PLTabMenu.Instance.TargetContainer == null && !flag15)
						{
							PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Open Hatch", false), "", "activate_station");
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
							{
								PLTabMenu.Instance.TargetContainer = PLLCChair.Instance.InjectionPayloadContainer;
								PLTabMenu.Instance.OnToggleTabMenu();
							}
						}
						else if (num37 < 12f && PLTabMenu.Instance.TargetContainer == null && !flag15 && plshipInfo.ResearchLockerCollider.Raycast(new Ray(PLCameraSystem.Instance.CurrentSubSystem.LocalPawnCameras[0].transform.position, PLCameraSystem.Instance.CurrentSubSystem.LocalPawnCameras[0].transform.forward), out raycastHit, 8f))
						{
							plshipInfo.LastIsCursorOverAtomizerTime = Time.frameCount;
							PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Access Atomizer", false), "", "activate_station");
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
							{
								PLTabMenu.Instance.TargetContainer = PLServer.Instance.ResearchLockerInventory;
								PLTabMenu.Instance.OnToggleTabMenu();
							}
						}
						else if (pllocker2 != null)
						{
							PLPlayer cachedFriendlyPlayerOfClass = PLServer.Instance.GetCachedFriendlyPlayerOfClass(pllocker2.ClassID);
							if (cachedFriendlyPlayerOfClass != null)
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Access", false) + " " + PLReadableStringManager.Instance.GetFormattedResultFromInputString(cachedFriendlyPlayerOfClass.GetPlayerName(false)) + "'s Locker", "", "activate_station");
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo("", string.Concat(new string[]
								{
						PLLocalize.Localize("Access", false),
						" ",
						PLGlobal.Instance.ClassNames[pllocker2.ClassID],
						" ",
						PLLocalize.Localize("Locker", false)
								}), "", "activate_station");
							}
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
							{
								PLTabMenu.Instance.TargetContainer = PLServer.Instance.ClassInfos[pllocker2.ClassID].ClassLockerInventory;
								PLTabMenu.Instance.OnToggleTabMenu();
								PLMusic.PostEvent("play_sx_ship_generic_internal_locker_open", PLNetworkManager.Instance.MyLocalPawn.gameObject);
							}
						}
						else if (plexosuitVisualAsset != null)
						{
							if (PLNetworkManager.Instance.MyLocalPawn.GetExosuitIsActive())
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Take off exosuit", false), "", "toggle_exosuit");
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Put on exosuit", false), "", "toggle_exosuit");
							}
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.toggle_exosuit) && PLLoader.Instance.IsLoaded)
							{
								PLLoader.Instance.LoadThis(new PLLoadRequest(new CustomRequestExecuteFuntion(__instance.LoadingScreenExosuitExecute), 0, 0));
							}
						}
						else if (plfluffyOven != null && plfluffyOven.CookIsActive())
						{
							if (plfluffyOven.GetCookAlpha() > 0.01f && plfluffyOven.GetCurrentProducingBiscuitOwner() == PLNetworkManager.Instance.LocalPlayer && plfluffyOven.GetInternalItemInfo() != null)
							{
								PLGlobal.Instance.SetBottomInfo("", string.Concat(new string[]
								{
						PLLocalize.Localize("Take", false),
						" ",
						plfluffyOven.GetInternalItemInfo().GetItemName(false),
						"   (",
						plfluffyOven.GetBiscuitCookedLevel_String(),
						")"
								}), "", "activate_station");
							}
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station) && PLLoader.Instance.IsLoaded)
							{
								plfluffyOven.LocalPlayerTakeBiscuit();
							}
						}
						else if (plcaptainsChair != null)
						{
							if (plshipInfo.CaptainsChairPlayerID == PLNetworkManager.Instance.LocalPlayer.GetPlayerID())
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Leave Captain's Chair", false), "", "activate_station");
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
								{
									plshipInfo.photonView.RPC("AttemptToSitInCaptainsChair", PhotonTargets.MasterClient, new object[] { -1 });
								}
							}
							else if (plshipInfo.CaptainsChairPlayerID == -1 || (PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetClassID() == 0))
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Sit in Captain's Chair", false), "", "activate_station");
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
								{
									plshipInfo.photonView.RPC("AttemptToSitInCaptainsChair", PhotonTargets.MasterClient, new object[] { PLNetworkManager.Instance.LocalPlayer.GetPlayerID() });
								}
							}
						}
						else if (cargoObjectDisplay != null)
						{
							string text4 = "";
							if (plshipInfo != null && (plshipInfo.GetIsPlayerShip() || plshipInfo.ShipTypeID == EShipType.E_ACADEMY) && cargoObjectDisplay.DisplayedItem.ActualSlotType == ESlotType.E_COMP_SCRAP && (PLNetworkManager.Instance.LocalPlayer.Talents[55] > 0 || PLNetworkManager.Instance.LocalPlayer.Talents[54] > 0 || plshipInfo.ShipTypeID == EShipType.E_ACADEMY))
							{
								if (PLNetworkManager.Instance.LocalPlayer.ScrapProcessingAttemptsLeft > 0)
								{
									if (PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress == null && PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.process_scrap) > 0.1f)
									{
										PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress = cargoObjectDisplay;
									}
									PLGlobal.Instance.SetBottomInfo(text4, string.Concat(new string[]
									{
							PLLocalize.Localize("(hold) ", false),
							PLLocalize.Localize("Process", false),
							" ",
							cargoObjectDisplay.DisplayedItem.Name,
							" (+",
							(cargoObjectDisplay.DisplayedItem.Level + 1).ToString(),
							")\n",
							text
									}), "", "process_scrap");
								}
								else
								{
									PLNetworkManager.Instance.LocalPlayer.ScrapProcessing_COD_InProgress = null;
								}
							}
							else if (PLNetworkManager.Instance.MyLocalPawn.CurrentShip != null && (PLNetworkManager.Instance.MyLocalPawn.CurrentShip.TeamID == -1 || PLNetworkManager.Instance.MyLocalPawn.CurrentShip.Abandoned))
							{
								if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.pickup_item) > 0.1f)
								{
									StringBuilder stringBuilder5 = new StringBuilder();
									stringBuilder5.Append("Transferring ");
									stringBuilder5.Append(cargoObjectDisplay.DisplayedItem.Name);
									stringBuilder5.Append("\n");
									stringBuilder5.Append("[");
									float num64 = Mathf.Clamp01(PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.pickup_item) / __instance.CargoTransferHoldTime);
									for (int num65 = 0; num65 < 26; num65++)
									{
										float num66 = (float)num65 / 26f;
										if (num64 >= num66)
										{
											stringBuilder5.Append("|");
										}
										else
										{
											stringBuilder5.Append(" ");
										}
									}
									stringBuilder5.Append("]");
									PLGlobal.Instance.SetBottomInfo(text4, stringBuilder5.ToString(), "", "");
								}
								else
								{
									PLGlobal.Instance.SetBottomInfo(text4, PLLocalize.Localize("(hold) ", false) + PLLocalize.Localize("Transfer", false) + " " + cargoObjectDisplay.DisplayedItem.Name, "", "pickup_item");
								}
								if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.pickup_item) > __instance.CargoTransferHoldTime && Time.time - __instance.LastAttemptToTransferNeutralCargoTime > 2f)
								{
									__instance.LastAttemptToTransferNeutralCargoTime = Time.time;
									PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.pickup_item);
									PLNetworkManager.Instance.LocalPlayer.photonView.RPC("AttemptToTransferNeutralCargo", PhotonTargets.MasterClient, new object[]
									{
							PLNetworkManager.Instance.MyLocalPawn.CurrentShip.ShipID,
							cargoObjectDisplay.DisplayedItem.NetID
									});
								}
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo(text4, cargoObjectDisplay.DisplayedItem.Name, "", "");
							}
						}
						else if (plinteriorDoor != null)
						{
							bool flag18 = PLServer.AnyPlayerHasItemOfName(plinteriorDoor.RequiredItem);
							if (plinteriorDoor.RequiredItem == "" || flag18)
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Enter", false) + " " + PLLocalize.Localize(plinteriorDoor.VisibleName, false), "", "enter_exit");
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.enter_exit))
								{
									plinteriorDoor.OnLocalPlayerEnter();
								}
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Locked - Requires <", false) + PLLocalize.Localize(plinteriorDoor.RequiredItem, false) + ">", "", "");
							}
						}
						else if (pllockedSeamlessDoor != null)
						{
							bool flag19 = PLServer.AnyPlayerHasItemOfName(pllockedSeamlessDoor.RequiredItem);
							if (pllockedSeamlessDoor.RequiredItem == "" || flag19)
							{
								if (!pllockedSeamlessDoor.IsLockedDueToPIN() && !pllockedSeamlessDoor.ControlledFromScriptOnly)
								{
									PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Enter", false) + " " + PLLocalize.Localize(pllockedSeamlessDoor.VisibleName, false), "", "enter_exit");
									if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.enter_exit))
									{
										pllockedSeamlessDoor.OnLocalPlayerEnter();
									}
								}
								else
								{
									pllockedSeamlessDoor.SetBottomInfoStringsWhenLocked();
								}
							}
							else
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Locked - Requires <", false) + PLLocalize.Localize(pllockedSeamlessDoor.RequiredItem, false) + ">", "", "");
							}
						}
						else if (plappearanceStation != null)
						{
							PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Customize Appearance", false), "", "activate_station");
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
							{
								PLNetworkManager.Instance.MainMenu.AddActiveMenu(new PLCustomPawnMenu(PLNetworkManager.Instance.LocalPlayer));
							}
						}
						else if (plneuralRewriter != null)
						{
							float num67 = 6f;
							int num68 = 40;
							if (Time.time - __instance.LastAttemptToResetTalentPointsTime > 5f)
							{
								if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > 0.1f)
								{
									StringBuilder stringBuilder6 = new StringBuilder();
									stringBuilder6.Append(PLLocalize.Localize("REWRITING NEURAL CONNECTIONS", false));
									stringBuilder6.Append("\n");
									stringBuilder6.Append("[");
									float num69 = Mathf.Clamp01(PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) / num67);
									for (int num70 = 0; num70 < num68; num70++)
									{
										float num71 = (float)num70 / (float)num68;
										if (num69 >= num71)
										{
											stringBuilder6.Append("|");
										}
										else
										{
											stringBuilder6.Append(" ");
										}
									}
									stringBuilder6.Append("]");
									PLGlobal.Instance.SetBottomInfo("", stringBuilder6.ToString(), "", "");
								}
								else
								{
									PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("(hold) ", false) + PLLocalize.Localize("Reset Talent Points", false), "", "activate_station");
								}
							}
							else
							{
								PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.activate_station);
							}
							if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > num67)
							{
								__instance.LastAttemptToResetTalentPointsTime = Time.time;
								PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.activate_station);
								if (PLNetworkManager.Instance.LocalPlayerID != -1)
								{
									PLServer.Instance.photonView.RpcSecure("ResetTalentPointsOfPlayer", PhotonTargets.All, true, new object[] { PLNetworkManager.Instance.LocalPlayerID });
								}
							}
						}
						else if (transform != null && num14 < 4f)
						{
							if (PLNetworkManager.Instance.MyLocalPawn.CurrentShip.WeapUpgrade_PreviewItem != null)
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Take", false) + " " + PLNetworkManager.Instance.MyLocalPawn.CurrentShip.WeapUpgrade_BaseItem.GetItemName(false), "", "activate_station");
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
								{
									PLNetworkManager.Instance.MyLocalPawn.CurrentShip.photonView.RPC("Request_WeapUpgrade_TakeItem", PhotonTargets.MasterClient, new object[] { PLNetworkManager.Instance.LocalPlayerID.GetDecrypted() });
								}
							}
							else if (PLNetworkManager.Instance.LocalPlayer.MyInventory.ActiveItem != null && PLNetworkManager.Instance.LocalPlayer.MyInventory.ActiveItem.PawnItemType != EPawnItemType.E_HANDS)
							{
								PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Place", false) + " " + PLNetworkManager.Instance.LocalPlayer.MyInventory.ActiveItem.GetItemName(false), "", "activate_station");
								if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station))
								{
									if (PLNetworkManager.Instance.LocalPlayer.MyInventory.ActiveItem.PawnItemType == EPawnItemType.E_SCANNER)
									{
										PLTabMenu.Instance.TimedErrorMsg = PLLocalize.Localize("Scanner can't be upgraded!", false);
									}
									else if (PLNetworkManager.Instance.LocalPlayer.MyInventory.ActiveItem.PawnItemType == EPawnItemType.E_AMMO_CLIP)
									{
										PLTabMenu.Instance.TimedErrorMsg = PLLocalize.Localize("Ammo Clips can't be upgraded!", false);
									}
									else
									{
										PLNetworkManager.Instance.MyLocalPawn.CurrentShip.photonView.RPC("Request_WeapUpgrade_PlaceItem", PhotonTargets.MasterClient, new object[]
										{
								PLNetworkManager.Instance.LocalPlayerID.GetDecrypted(),
								PLNetworkManager.Instance.LocalPlayer.MyInventory.ActiveItem.NetID
										});
									}
								}
							}
						}
						else if (plliarsDiceGame != null && plliarsDiceGame.photonView != null)
						{
							PLGlobal.Instance.SetBottomInfo("", PLLocalize.Localize("Play Liar's Dice", false), "", "activate_station");
							if (PLInput.Instance.GetButtonUp(PLInputBase.EInputActionName.activate_station) && Time.time - PLNetworkManager.Instance.LocalPlayer.LocalEditTime_CurrentlyInLiarsDiceGame > 0.5f)
							{
								PLNetworkManager.Instance.LocalPlayer.photonView.RPC("TellServerIAmPlayingLiarsDiceGame", PhotonTargets.MasterClient, new object[] { plliarsDiceGame.photonView.viewID });
								if (PLNetworkManager.Instance.MyLocalPawn != null)
								{
									Vector3 vector2;
									Quaternion quaternion2;
									PLLiarsDiceGame.GetCameraPosAndRotForClassID(plliarsDiceGame, PLNetworkManager.Instance.LocalPlayer.GetClassID(), out vector2, out quaternion2);
									PLNetworkManager.Instance.MyLocalPawn.HorizontalMouseLook.RotationX = quaternion2.y;
									PLNetworkManager.Instance.MyLocalPawn.VerticalMouseLook.RotationY = quaternion2.x;
								}
							}
						}
						else if (flag16)
						{
							float num72 = 4f;
							num72 *= 1f - (float)(PLNetworkManager.Instance.LocalPlayer.Talents[59] + PLNetworkManager.Instance.LocalPlayer.Talents[60]) * 0.08f;
							int num73 = 40;
							if (Time.time - __instance.LastAttemptToResetTalentPointsTime > 5f)
							{
								if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > 0.05f)
								{
									StringBuilder stringBuilder7 = new StringBuilder();
									stringBuilder7.Append(PLLocalize.Localize("RELOADING", false));
									stringBuilder7.Append("\n");
									stringBuilder7.Append("[");
									float num74 = Mathf.Clamp01(PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) / num72);
									for (int num75 = 0; num75 < num73; num75++)
									{
										float num76 = (float)num75 / (float)num73;
										if (num74 >= num76)
										{
											stringBuilder7.Append("|");
										}
										else
										{
											stringBuilder7.Append(" ");
										}
									}
									stringBuilder7.Append("]");
									flag = true;
									PLGlobal.Instance.SetBottomInfo("", stringBuilder7.ToString(), "", "");
								}
							}
							else
							{
								PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.activate_station);
							}
							if (PLInput.Instance.GetHeldDownTime(PLInputBase.EInputActionName.activate_station) > num72)
							{
								__instance.LastAttemptToResetTalentPointsTime = Time.time;
								PLInput.Instance.ResetHeldDownTime(PLInputBase.EInputActionName.activate_station);
								if (__instance.CurrentPlayer_Clips.Count > 0)
								{
									PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory.RemoveItem(__instance.CurrentPlayer_Clips[0]);
									PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory.ActiveItem.AmmoCurrent = PLNetworkManager.Instance.MyLocalPawn.GetPlayer().MyInventory.ActiveItem.AmmoMax;
									PLMusic.PostEvent("play_sx_player_item_grenadelauncher_ammorefill", __instance.gameObject);
								}
								__instance.CurrentPlayer_Clips.Clear();
								__instance.shouldUpdateClips = true;
							}
						}
					}
					state = "29";
					if (PLCameraSystem.Instance.CurrentCameraMode != null && PLCameraSystem.Instance.CurrentCameraMode.GetModeString() == "LocalPawn" && PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetPawn() != null && PLNetworkManager.Instance.LocalPlayer.GetPawn().MyController != null && PLNetworkManager.Instance.LocalPlayer.GetPawn().MyController.IsEncumbered() && Time.time % 2f < 1f)
					{
						PLGlobal.Instance.SetBottomInfo("", "<color=red>" + PLLocalize.Localize("You are encumbered", false) + "</color>", "", "");
					}
				}
				int num77 = -1;
				state = "30";
				if (PLNetworkManager.Instance.ViewedPawn != null)
				{
					num77 = PLNetworkManager.Instance.ViewedPawn.GetSubHub();
				}
				state = "31";
				if (PLCameraSystem.Instance != null && PLCameraSystem.Instance.IsCameraControlledByCutscene() && PLCameraSystem.Instance.CurrentPlayingDirector_Volume != null && PLCameraSystem.Instance.CurrentPlayingDirector_Volume.MyTLI != null)
				{
					num77 = PLCameraSystem.Instance.CurrentPlayingDirector_Volume.MyTLI.SubHubID;
				}
				for (int num78 = 0; num78 < __instance.AllPawns.Count; num78++)
				{
					PLPawn plpawn3 = __instance.AllPawns[num78];
					if (plpawn3 != null && plpawn3.gameObject != null && !plpawn3.PreviewPawn)
					{
						PLPlayer myPlayer = plpawn3.MyPlayer;
						if (myPlayer != null)
						{
							if (((plpawn3.MyRoomArea == null || plpawn3.MyRoomArea.IsVisible()) | plpawn3.HadRecentLOSSuccessToTarget(PLNetworkManager.Instance.ViewedPawn) | (plpawn3 == PLNetworkManager.Instance.ViewedPawn)) && ((myPlayer.MyCurrentTLI != null && PLNetworkManager.Instance.ViewedPawn != null && myPlayer.MyCurrentTLI.SubHubID == num77) || PLNetworkManager.Instance.ViewedPawn == null))
							{
								if (plpawn3 == PLNetworkManager.Instance.ViewedPawn && !plpawn3.ExteriorViewActive())
								{
									if (PLServer.GetCurrentSector() != null && PLServer.GetCurrentSector().VisualIndication == ESectorVisualIndication.ACADEMY)
									{
										plpawn3.gameObject.layer = 24;
										plpawn3.MySkinnedMeshRenderer.gameObject.layer = 24;
									}
									else if (myPlayer.OnPlanet)
									{
										plpawn3.gameObject.layer = 24;
										plpawn3.MySkinnedMeshRenderer.gameObject.layer = 24;
									}
									else
									{
										plpawn3.gameObject.layer = 13;
										plpawn3.MySkinnedMeshRenderer.gameObject.layer = 13;
									}
								}
								else if (myPlayer.OnPlanet)
								{
									plpawn3.gameObject.layer = 14;
									plpawn3.MySkinnedMeshRenderer.gameObject.layer = 14;
								}
								else
								{
									plpawn3.gameObject.layer = 13;
									plpawn3.MySkinnedMeshRenderer.gameObject.layer = 13;
								}
							}
							else
							{
								plpawn3.gameObject.layer = 26;
								plpawn3.MySkinnedMeshRenderer.gameObject.layer = 26;
							}
						}
						else
						{
							plpawn3.gameObject.layer = 26;
							plpawn3.MySkinnedMeshRenderer.gameObject.layer = 26;
						}
					}
				}
				state = "32";
				if (PLNetworkManager.Instance.MyLocalPawn != null && flag != __instance.ReloadingSFXIsPlaying)
				{
					__instance.ReloadingSFXIsPlaying = flag;
					if (flag)
					{
						PLMusic.PostEvent("play_ship_generic_internal_computer_ui_progress", PLNetworkManager.Instance.MyLocalPawn.gameObject);
					}
					else
					{
						PLMusic.PostEvent("stop_ship_generic_internal_computer_ui_progress", PLNetworkManager.Instance.MyLocalPawn.gameObject);
					}
				}
				state = "33";
				PLPersistantEncounterInstance cpei = PLEncounterManager.Instance.GetCPEI();
				if (cpei != null)
				{
					foreach (PLShipInfoBase plshipInfoBase in PLEncounterManager.Instance.AllShips.Values)
					{
						if (plshipInfoBase != null && plshipInfoBase.Exterior != null && (cpei.SpaceInteriorAABB == null || (plshipInfoBase.CurrentRace == cpei.Race && !cpei.SpaceInteriorAABB.Contains(plshipInfoBase.Exterior.transform.position, 1f))))
						{
							ParticleSystem particleSystem = __instance.GetLightningPSForShip(plshipInfoBase);
							if (particleSystem == null)
							{
								particleSystem = UnityEngine.Object.Instantiate<GameObject>(PLGlobal.Instance.ShipRaceLightningPS, plshipInfoBase.Exterior.transform).GetComponent<ParticleSystem>();
								particleSystem.gameObject.SetActive(true);
								particleSystem.transform.localPosition = Vector3.zero;
								particleSystem.transform.localRotation = Quaternion.identity;
								particleSystem.transform.localScale = Vector3.one;
								__instance.ShipRaceLightningPSMap.Add(plshipInfoBase, particleSystem);
								if (PLAbyssShipInfo.Instance != null && plshipInfoBase != PLAbyssShipInfo.Instance)
								{
									ParticleSystem.ShapeModule shape = particleSystem.shape;
									EShipType shipTypeID = plshipInfoBase.ShipTypeID;
									if (shipTypeID != EShipType.E_MATRIX_DRONE)
									{
										switch (shipTypeID)
										{
											case EShipType.E_ABYSS_LAVA_BOSS:
												shape.radius = 30f;
												break;
											case EShipType.E_ABYSS_BOSS:
												shape.radius = 5f;
												break;
											case EShipType.E_ABYSS_SPHERE_BOSS:
												shape.radius = 10f;
												break;
											case EShipType.E_ABYSS_WARDEN_BOSS:
												shape.shapeType = ParticleSystemShapeType.MeshRenderer;
												shape.meshRenderer = plshipInfoBase.Exterior.GetComponent<MeshRenderer>();
												shape.useMeshColors = false;
												break;
										}
									}
									else
									{
										shape.radius = 5f;
									}
								}
							}
							float num79 = plshipInfoBase.DischargeAmount;
							if (PLAbyssShipInfo.Instance != null && plshipInfoBase != PLAbyssShipInfo.Instance && PLAbyssShipInfo.EMPIsActive())
							{
								num79 = 1f;
							}
							var em = particleSystem.emission;
							em.rateOverTime = Mathf.Pow(Mathf.Clamp01(num79), 2f) * 12f * (particleSystem.shape.radius / 3f);
							float num80 = 360f;
							List<PLGameStatic.ShockLineInfo> list = __instance.ShipRaceLightning_ShockLineInfo_PSMap[plshipInfoBase.ShipID];
							float num81 = 0f;
							if (__instance.GetType() == typeof(PLRace3Encounter))
							{
								num81 = 1f;
							}
							float num82 = PLBeaconInfo.GetBeaconStatAdditive(EBeaconType.E_OVERCHARGE, plshipInfoBase.GetIsPlayerShip()) * 3f;
							num81 += num82;
							num80 += num82 * 150f;
							if (num81 >= 0.99f)
							{
								foreach (PLShipInfoBase plshipInfoBase2 in PLEncounterManager.Instance.AllShips.Values)
								{
									if (plshipInfoBase2 != null && plshipInfoBase2 != plshipInfoBase && plshipInfoBase2.CurrentRace == cpei.Race && Vector3.SqrMagnitude(plshipInfoBase2.Exterior.transform.position - plshipInfoBase.Exterior.transform.position) < num80 * num80)
									{
										PLGameStatic.ShockLineInfo shockLineInfo = __instance.GetSLIInListToTransform(ref list, plshipInfoBase2.Exterior.transform);
										if (shockLineInfo == null)
										{
											shockLineInfo = __instance.CreateSLI(plshipInfoBase, plshipInfoBase2.Exterior.transform, plshipInfoBase2);
										}
										shockLineInfo.Range = num80;
										shockLineInfo.Intensity = 0.8f * num81;
										shockLineInfo.LastProcessedFrame = Time.frameCount;
									}
								}
							}
							foreach (PLShockSphere plshockSphere in PLShockSphere.All)
							{
								if (plshockSphere != null && Vector3.SqrMagnitude(plshockSphere.transform.position - plshipInfoBase.Exterior.transform.position) < plshockSphere.Range * plshockSphere.Range)
								{
									PLGameStatic.ShockLineInfo shockLineInfo2 = __instance.GetSLIInListToTransform(ref list, plshockSphere.transform);
									if (shockLineInfo2 == null)
									{
										shockLineInfo2 = __instance.CreateSLI(plshipInfoBase, plshockSphere.transform, null);
									}
									shockLineInfo2.Range = plshockSphere.Range;
									shockLineInfo2.Intensity = plshockSphere.Intensity;
									shockLineInfo2.LastProcessedFrame = Time.frameCount;
								}
							}
							foreach (PLGameStatic.ShockLineInfo shockLineInfo3 in list)
							{
								if (shockLineInfo3 != null && shockLineInfo3.LastProcessedFrame == Time.frameCount)
								{
									float num83 = Vector3.SqrMagnitude(shockLineInfo3.ToTransform.position - plshipInfoBase.Exterior.transform.position);
									Vector3.Lerp(plshipInfoBase.Exterior.transform.position, shockLineInfo3.ToTransform.position, 0.5f);
									Vector3 vector3 = Vector3.Normalize(plshipInfoBase.Exterior.transform.position - shockLineInfo3.ToTransform.position);
									shockLineInfo3.PS.transform.localRotation = PLGlobal.SafeLookRotation(shockLineInfo3.PS.transform.parent.InverseTransformDirection(vector3));
									ParticleSystem.EmissionModule emission = shockLineInfo3.PS.emission;
									float num84 = Mathf.Sqrt(num83) / shockLineInfo3.Range;
									emission.rateOverTime = Mathf.Pow(1f - num84, 2f) * 15f * shockLineInfo3.Intensity;
									float num85 = 0.05f;
									if (plshipInfoBase.DischargeAmount > 0.75f)
									{
										num85 *= 0.75f;
									}
									else if (plshipInfoBase.DischargeAmount > 0.9f)
									{
										num85 *= 0.5f;
									}
									plshipInfoBase.DischargeAmount += Time.deltaTime * num85 * (1f - num84) * shockLineInfo3.Intensity;
								}
							}
						}
					}
				}
				state = "34";
				foreach (int num86 in __instance.ShipRaceLightning_ShockLineInfo_PSMap.Keys)
				{
					foreach (PLGameStatic.ShockLineInfo shockLineInfo4 in __instance.ShipRaceLightning_ShockLineInfo_PSMap[num86])
					{
						if (shockLineInfo4 != null && shockLineInfo4.PS != null)
						{
							if (shockLineInfo4.ToTransform == null || shockLineInfo4.LastProcessedFrame < Time.frameCount - 20000)
							{
								UnityEngine.Object.Destroy(shockLineInfo4.PS.gameObject);
							}
							else if (shockLineInfo4.LastProcessedFrame < Time.frameCount && shockLineInfo4.PS.emission.enabled)
							{
								var em = shockLineInfo4.PS.emission;
								em.enabled = false;
							}
						}
					}
					__instance.ShipRaceLightning_ShockLineInfo_PSMap[num86].RemoveAll((PLGameStatic.ShockLineInfo item) => item == null || item.PS == null);
				}
				state = "NICE";
			}
			catch(Exception e)
			{
				PulsarModLoader.Utilities.Logger.Info($"Oops! Last pos: {state}!{e.Message}!");
			}
			return false; // override
		}

		static T Check<T>(this T ptr, string help)
		{
			if (ptr == null)
				PulsarModLoader.Utilities.Logger.Info($"[LOGGER3000] DETECTED NULL REFERENCE!!!11!1 -> {help}");
			return ptr;
		}
	} */
}
