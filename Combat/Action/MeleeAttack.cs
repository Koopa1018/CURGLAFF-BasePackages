using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Clouds.PlayerInput;

namespace Clouds.Combat.ActionGame.Weapons {
	[AddComponentMenu("Combat/I'm A Melee Attack")]
	public class MeleeAttack : MonoBehaviour {
		[System.Serializable]
		struct HurtboxState {
			public Collider2D enabledCollider;
			public float startTime, endTime;
		}

		[Header("Outputs")]
		[SerializeField] WeaponStrike myStrike;

		[Header("Properties")]
		[SerializeField] float strikeDuration = 0.15f;

		[Header("Events")]
		[SerializeField] UnityEvent onAttackBegin;
		[SerializeField] UnityEvent onAttackEnd;

		float strikeTimer = 0;
		bool timerIsGoing = false;
		
		public void BeginAttack () {
			//Abort if the strike timer hasn't expired yet.
			if (strikeTimer > 0) {
				return;
			}
			myStrike.BeginNew();

			strikeTimer = strikeDuration;
			timerIsGoing = true;

			onAttackBegin?.Invoke();
		}

		void FixedUpdate () {
			strikeTimer -= Time.fixedDeltaTime;
			strikeTimer = Mathf.Max(strikeTimer, 0);

			if (timerIsGoing && strikeTimer == 0) {
				timerIsGoing = false;
				
				onAttackEnd?.Invoke();
			}
		}

	}
}