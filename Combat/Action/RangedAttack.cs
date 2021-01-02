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
		[SerializeField] new ScriptOperatedAnimation animation;

		[Header("Properties")]
		[SerializeField] Clouds.Generic.Playlist<AnimationClip> attackAnimations;
		[SerializeField] Clouds.Generic.Playlist<AnimationClip> fizzleAnimations;
		[SerializeField] Clouds.Generic.Playlist<AnimationClip> hitAnimations; //not yet used
		[SerializeField] float shotLifetime = 1f;

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
			animation.SetClip(attackAnimations.GetNext());
			animation.Play();

			strikeTimer = shotLifetime;
			timerIsGoing = true;

			onAttackBegin?.Invoke();
		}

		void FixedUpdate () {
			strikeTimer -= Time.fixedDeltaTime;
			strikeTimer = Mathf.Max(strikeTimer, 0);

			if (timerIsGoing && strikeTimer == 0) {
				onAttackEnd.Invoke();

				timerIsGoing = false;
				
				animation.SetClip(fizzleAnimations.GetNext());
				animation.Play();
			}
		}

	}
}