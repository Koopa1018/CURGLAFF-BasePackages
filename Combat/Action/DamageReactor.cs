using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Clouds.Combat;

namespace Clouds.Combat.ActionGame.Weapons {
	/// <summary>
	/// Base class for a thing that reacts to taking damage.
	/// </summary>
	public abstract class DamageReactor : MonoBehaviour {
		[SerializeField] protected Collider2D myCollider;

		/// <summary>
		/// The vulnerability map which all strike damage will be run through.
		/// If null, the <c>defaultDamageMultiplier</c> will be used instead.
		/// </summary>
		protected abstract IVulnerabilityMap vulnerabilities {get;}
		/// <summary>
		/// When <c>vulnerabilities</c> is null, all damage will be multiplied
		/// by this number.
		/// </summary>
		protected abstract float defaultDamageMultiplier {get;}

		/// <summary>
		/// The meat of the reaction code.
		/// </summary>
		/// <param name="strike">The weapon strike we've been hit with. Guaranteed to be non-null.</param>
		/// <param name="damageMultiplier">The damage multiplier calculated from the vulnerability map.</param>
		protected abstract void DoReaction (DamageDealer strike, float damageMultiplier);


		/// <summary>
		/// Reacts to the given weapon strike.
		/// </summary>
		/// <param name="strike">The weapon strike being reacted to. If null, silently exits.</param>
		public void React (DamageDealer strike) {
			if (strike == null) {
				return;
			}
			
			float damageMultiplier = defaultDamageMultiplier;
			if (vulnerabilities != null) {
				damageMultiplier = vulnerabilities.CheckAttack(strike);
			}

			DoReaction(strike, damageMultiplier);
		}
		

		const int REACTOR_DICTIONARY_CAPACITY = 256; //@TODO: does this need to be aligned as PoT?

		static Dictionary<Collider2D, DamageReactor> _reactorsInScene;
		public static Dictionary<Collider2D, DamageReactor> ReactorsInScene {
			get {
				if (_reactorsInScene == null) {
					_reactorsInScene = new Dictionary<Collider2D, DamageReactor>(REACTOR_DICTIONARY_CAPACITY);
				}

				return _reactorsInScene;
			}
		}

		protected void OnEnable () {
			ReactorsInScene.Add(myCollider, this);
		}
		protected void OnDisable () {
			ReactorsInScene.Remove(myCollider);
		}

#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod]
		void ClearDictionaryOnBegin () {
			_reactorsInScene.Clear();
		}
#endif


	}
}