using PulsarModLoader;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PulsarExperiments.Features.Items
{
	public class PLPawnItem_Melee : PLPawnItem_Gun
	{
		public Animation attack;
		public HitOnCollide bonk;

		bool IsAttack = false;

		public PLPawnItem_Melee() : base(EPawnItemType.E_LASERPISTOL) 
		{
			this.PawnWalkSpeed = 3.5f;
			this.MyUtilityType = EItemUtilityType.E_DAMAGE;
			UsesHeat = false;
		}

		public virtual float StaminaLoss { get => 0.3f; }

		public virtual void Bonk(PLCombatTarget target)
		{
			target.photonView.RPC("TakeDamage", PhotonTargets.All, Damage(), true, MySetupPawn.CombatTargetID);
			target.LastShouldForceShowInHUDTime = Time.time;
			PLMusic.PostEvent("play_player_item_blaster_bullet_hit_generic", target.gameObject);
		}

		public virtual bool CanAttack()
		{
			return (!PLStandaloneInputModule.MouseCursorInFreeMode || this.MySetupPawn.ShowJetpackVisuals)
				&& !this.MySetupPawn.IsDead
				&& this.MySetupPawn == PLNetworkManager.Instance.MyLocalPawn
				&& !PLNetworkManager.Instance.MainMenu.IsActive()
				&& PLNetworkManager.Instance.MainMenu.GetActiveMenuCount() == 0
				&& PLCameraSystem.Instance != null
				&& PLCameraSystem.Instance.GetModeString() == "LocalPawn"
				&& PLTabMenu.Instance != null
				&& !PLTabMenu.Instance.TabMenuActive
				&& PLTabMenu.Instance.DialogueMenu != null
				&& PLTabMenu.Instance.DialogueMenu.CurrentActorInstance == null
				&& PLTabMenu.Instance.TargetContainer == null
				&& !PLTabMenu.Instance.IsDisplayingOrderMenu()
				&& !PLVirtualKeyboard.Instance.Visuals.activeSelf
				&& !PLStarmap.Instance.IsActive
				&& PLInput.Instance.GetButton(PLInputBase.EInputActionName.fire);
		}

		public virtual IEnumerator Cooldown()
		{
			while (attack.isPlaying)
				yield return new WaitForSeconds(1);

			IsAttack = false;
			bonk.CanAttack = false;

			yield break;
		}

		public virtual float Damage() => 40f + 10f * base.Level;

		public override void OnActive()
		{
			base.OnActive();
			if (this.MySetupPawn != null && this.MySetupPawn.GetPlayer() != null)
			{
				if (IsAttack) return;
				if (!CanAttack()) return;
				if (MySetupPawn.MyController.Stamina < StaminaLoss) return;
				
				ModMessageHelper.Instance.photonView.RPC("ReceiveMessage", PhotonTargets.Others, "BadExperiments#PulsarExperiments.Features.Items.PLPawnItem_MeleeSync", new object[] { MySetupPawn.PlayerID, NetID });

				MySetupPawn.MyController.Stamina -= StaminaLoss;
				MySetupPawn.MyController.LastSprintTime = Time.time;
				attack.Play();
				bonk.CanAttack = true;
				IsAttack = true;
				MySetupPawn.StartCoroutine(Cooldown());
			}
		}

		public override void OnInActive()
		{
			base.OnInActive();
			attack.Stop();
			IsAttack = false;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.MySetupPawn == null)
			{
				return;
			}
			if (this.MyGunInstance != null)
			{
				if (this.MyGunInstance.transform.parent != base.GetTargetParentTransform())
				{
					this.MyGunInstance.transform.parent = base.GetTargetParentTransform();
					this.MyGunInstance.transform.localPosition = Vector3.zero;
					this.MyGunInstance.transform.localRotation = Quaternion.identity;
				}
				if (UnityEngine.Random.Range(0, 1000) == 0)
				{
					this.MyGunInstance.transform.localPosition = Vector3.zero;
					this.MyGunInstance.transform.localRotation = Quaternion.identity;
				}
			}
		}

		public override void Setup(PLPawn inPawn, PLPawnInventory inInventory)
		{
			if (inPawn != null)
			{
				bool installAnimator = false;
				if (MyGunInstance == null)
					installAnimator = true;

				base.Setup(inPawn, inInventory);

				if (installAnimator)
				{
					attack = MyGunInstance.transform.GetComponent<Animation>();
					bonk = MyGunInstance.gameObject.AddComponent<HitOnCollide>();
					bonk.OnHit = Bonk;
					bonk.Owner = MySetupPawn;
				}
			}
		}
	}

	public sealed class PLPawnItem_MeleeSync : ModMessage
	{
		public override void HandleRPC(object[] arguments, PhotonMessageInfo sender)
		{
			int playerId = (int)arguments[0];
			//int inNetId = (int)arguments[1];
			(PLServer.Instance.AllPlayers.First(p => p.GetPlayerID() == playerId).MyInventory.ActiveItem as PLPawnItem_Melee).attack.Play();
		}
	}
}