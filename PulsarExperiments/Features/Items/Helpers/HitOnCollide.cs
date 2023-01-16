using System;
using UnityEngine;

namespace PulsarExperiments.Features.Items
{
	public class HitOnCollide : MonoBehaviour
	{
		public Action<PLCombatTarget> OnHit;
		public bool CanAttack;
		public PLCombatTarget Owner;

		private Transform BonkMax;
		private Transform BonkMin;

		void Start()
		{
			var transforms = this.GetComponentsInChildren<Transform>();
			foreach (var t in transforms)
			{
				if (t.name == "BonkMax") BonkMax = t;
				else if (t.name == "BonkMin") BonkMin = t;
			}
		}

		void Update()
		{
			if (CanAttack)
			{
				PLCombatTarget plcombatTarget = null;

				var max = BonkMax.position;
				var min = BonkMin.position;
				float num2 = float.MaxValue;
				foreach (var target in PLGameStatic.Instance.AllCombatTargets)
				{
					if (target != null && target != Owner && target.gameObject != null && target.ShouldTakeDamage())
					{
						bool flag3 = false;
						flag3 |= Owner.CanHitFriendlyTargets;
						flag3 |= target.GetIsFriendly() != Owner.GetIsFriendly();
						//PulsarModLoader.Utilities.Messaging.Notification($"can bonk {flag3}");
						if (flag3 && target.MyCollisionSpheres != null)
						{
							foreach (PLPawnCollisionSphere plpawnCollisionSphere in target.MyCollisionSpheres)
							{
								if (plpawnCollisionSphere != null)
								{
									float closestDistance = PLGlobal.ClosestSqDistBetweenSegmentAndPoint(max, min, plpawnCollisionSphere.transform.position);
									float num4 = Vector3.SqrMagnitude(max - plpawnCollisionSphere.transform.position);
									if (closestDistance < plpawnCollisionSphere.Radius * plpawnCollisionSphere.Radius && num4 < num2)
									{
										//PulsarModLoader.Utilities.Messaging.Notification($"Should bonk {target.GetName()}");
										plcombatTarget = target;
									}
								}
							}
						}
					}
				}

				if (plcombatTarget != null)
				{
					OnHit(plcombatTarget);
					CanAttack = false;
				}
			}
		}
	}
}
