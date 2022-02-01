using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Clouds.Combat;

namespace Clouds.Combat.ActionGame.Weapons {
	[RequireComponent(typeof(ReactToWeaponStrike))]
	public sealed class ActiveReactToWeaponStrike : MonoBehaviour {
		[SerializeField] LayerMask weaponMask;

		ReactToWeaponStrike reactor;

		void Start () {
			reactor = GetComponent<ReactToWeaponStrike>();
		}

		void OnColliderEnter2D (Collider2D other) {
			//If the other game object isn't on a layer we care about, abort.
			if ((other.gameObject.layer & weaponMask) == 0) {
				return;
			}

			reactor.React(other.GetComponent<WeaponStrike>());
		}				

	}
}