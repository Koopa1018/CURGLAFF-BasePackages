using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Clouds.Combat;

namespace Clouds.Combat.ActionGame.Weapons {
	[AddComponentMenu("Combat/Trigger On Damage (requires vulnerability map)")]
	public sealed class TriggerOnDamage : DamageReactor {
		[SerializeField] float damageIfNoVulnerabilityMap = 1;
		protected override float defaultDamageMultiplier => damageIfNoVulnerabilityMap;
		[SerializeField] UnityEvent<DamageDealer, float> onHitEvent;

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

		protected override void DoReaction (DamageDealer strike, float hitFactor) {
			onHitEvent?.Invoke(strike, hitFactor);
		}

	}
}