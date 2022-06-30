using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Clouds.Combat;

namespace Clouds.Combat.ActionGame.Weapons {
	[AddComponentMenu("Combat/Trigger On Weapon Strike (requires vulnerability map)")]
	public sealed class TriggerOnWeaponStrike : ReactToWeaponStrike {
		[SerializeField] UnityEvent<WeaponStrike, float> onHitEvent;
		[SerializeField] float damageIfNoVulnerabilityMap = 1;
		protected override float defaultDamageMultiplier => damageIfNoVulnerabilityMap;

		IVulnerabilityMap myVulnerabilities;
		protected override IVulnerabilityMap vulnerabilities => myVulnerabilities;

		HashSet<int> oneShotStrikesUsed;


		void Start () {
			myVulnerabilities = GetComponent<IVulnerabilityMap>();

			oneShotStrikesUsed = new HashSet<int>();
		}

		public void Initialize () {
			oneShotStrikesUsed.Clear();
		}

		protected override void DoReaction (WeaponStrike strike, float hitFactor) {
			onHitEvent?.Invoke(strike, hitFactor);
		}

	}
}