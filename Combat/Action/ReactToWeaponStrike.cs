using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Clouds.Combat;

namespace Clouds.ActionGame.Weapons {
	public sealed class ReactToWeaponStrike : MonoBehaviour {
		[SerializeField] UnityEvent<WeaponStrike, float> onHitEvent;

		IVulnerabilityMap myVulnerabilities;

		HashSet<int> oneShotStrikesUsed;

		void Start () {
			myVulnerabilities = GetComponent<IVulnerabilityMap>();

			oneShotStrikesUsed = new HashSet<int>();
		}

		public void React (WeaponStrike strike) {
			if (strike == null) {
				return;
			}
			
			float hitFactor = 0;
			if (myVulnerabilities != null) {
				hitFactor = myVulnerabilities.CheckAttack(strike);
			}
			onHitEvent?.Invoke(strike, hitFactor);
		}				

	}
}