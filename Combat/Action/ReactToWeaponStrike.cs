using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Clouds.Combat;

namespace Clouds.Combat.ActionGame.Weapons {
	[AddComponentMenu("Combat/React To Weapon Strikes (requires vulnerability map)")]
	public sealed class ReactToWeaponStrike : MonoBehaviour {
		const int REACTOR_DICTIONARY_CAPACITY = 1024; //@TODO: does this need to be aligned as PoT?

		[SerializeField] Collider2D myCollider;
		[SerializeField] UnityEvent<WeaponStrike, float> onHitEvent;

		IVulnerabilityMap myVulnerabilities;

		HashSet<int> oneShotStrikesUsed;
		
		static Dictionary<Collider2D, ReactToWeaponStrike> _reactorsInScene;
		public static Dictionary<Collider2D, ReactToWeaponStrike> ReactorsInScene {
			get {
				if (_reactorsInScene == null) {
					_reactorsInScene = new Dictionary<Collider2D, ReactToWeaponStrike>(REACTOR_DICTIONARY_CAPACITY);
				}

				return _reactorsInScene;
			}
		}

		void OnEnable () {
			ReactorsInScene.Add(myCollider, this);
		}
		void OnDisable () {
			ReactorsInScene.Remove(myCollider);
		}


		void Start () {
			myVulnerabilities = GetComponent<IVulnerabilityMap>();

			oneShotStrikesUsed = new HashSet<int>();
		}

		public void Initialize () {
			oneShotStrikesUsed.Clear();
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