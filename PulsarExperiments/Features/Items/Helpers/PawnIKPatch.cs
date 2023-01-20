using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace PulsarExperiments.Features.Items.Helpers
{
	[HarmonyPatch(typeof(PLPawnIK))]
	static class PawnIKPatch
	{
		static IEnumerable<CodeInstruction> DoPatchNTimes(int N, IEnumerable<CodeInstruction> instructions, IEnumerable<CodeInstruction> target, IEnumerable<CodeInstruction> with)
		{
			if (N==0) return instructions;
			var result = PulsarModLoader.Patches.HarmonyHelpers.PatchBySequence(instructions, target, with, PulsarModLoader.Patches.HarmonyHelpers.PatchMode.REPLACE, PulsarModLoader.Patches.HarmonyHelpers.CheckMode.ALWAYS);
			return DoPatchNTimes(N-1, result, target, with);
		}

		[HarmonyPatch("LateUpdate"), HarmonyTranspiler]
		static IEnumerable<CodeInstruction> LateUpdate(IEnumerable<CodeInstruction> instructions)
		{
			var target = new CodeInstruction[] {
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPawnIK), "Gun_diffPos_RightToLeftHand")),
				new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(PLPawnIK), "diffPos_RightToLeftHand")),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PLPawnIK), "Gun_diffRot_RightToLeftHand")),
				new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(PLPawnIK), "diffRot_RightToLeftHand")),
				};
			var add = new CodeInstruction[] {
				new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PawnIKPatch), "HookAnimIDHandsSet")),
				};
			var result = DoPatchNTimes(2, instructions, target, add);
			return result;
		}

		static void HookAnimIDHandsSet(PLPawnIK _this)
		{
			if (_this.MyPawn.MyPlayer != null && _this.MyPawn.MyPlayer.MyInventory != null && _this.MyPawn.MyPlayer.MyInventory.ActiveItem != null)
			{
				if (_this.MyPawn.MyPlayer.MyInventory.ActiveItem is KnifeMod.Knife) // knife shitcoded style
				{
					_this.diffPos_RightToLeftHand = Knife_diffPos_RightToLeftHand;
					_this.diffRot_RightToLeftHand = Knife_diffRot_RightToLeftHand;
					return;
				}
			}
			// default
			_this.diffPos_RightToLeftHand = _this.Gun_diffPos_RightToLeftHand;
			_this.diffRot_RightToLeftHand = _this.Gun_diffRot_RightToLeftHand;
		}

		//static Vector3 Knife_diffPos_RightToLeftHand = new Vector3(-0.2f, -0.1f, -0.2f); 
		//static Quaternion Knife_diffRot_RightToLeftHand = new Quaternion(-0.4394f, 0.5265f, 0.6275f, -0.3687f); // 280 290 180

		static Vector3 Knife_diffPos_RightToLeftHand = new Vector3(0, -0.08f, 0); 
		static Quaternion Knife_diffRot_RightToLeftHand = new Quaternion(-0.1f, 0.1f, 1f, 0f);
	}
}
