using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Clouds.PlayerInput;

namespace Clouds.ActionGame.Weapons {
	[AddComponentMenu("Combat/I'm A Ranged Attack")]
	public class RangedAttack : MonoBehaviour {
		[Header("Outputs")]
		[SerializeField] WeaponStrike myStrike;

		[Header("Properties")]
		[SerializeField] float shotLifetime = 1f;

		[Header("Events")]
		[SerializeField] UnityEvent onAttackBegin;
		[SerializeField] UnityEvent onAttackFizzle;
		[SerializeField] UnityEvent onAttackHit; //not yet used

		float strikeTimer = 0;
		bool timerIsGoing = false;
		
		public void BeginAttack () {
			//Abort if the strike timer hasn't expired yet.
			if (strikeTimer > 0) {
				return;
			}
			GetComponent<Animation>().SetClip(attackAnimations.GetNext());
			GetComponent<Animation>().Play();

			strikeTimer = shotLifetime;
			timerIsGoing = shotLifetime > 0;

			myStrike.BeginNew();
			onAttackBegin?.Invoke();
		}

		void FixedUpdate () {
			strikeTimer -= Time.fixedDeltaTime;
			strikeTimer = Mathf.Max(strikeTimer, 0);

			if (timerIsGoing && strikeTimer == 0) {
				onAttackFizzle?.Invoke();

				timerIsGoing = false;
				
				GetComponent<Animation>().SetClip(fizzleAnimations.GetNext());
				GetComponent<Animation>().Play();
			}
		}

	}
}