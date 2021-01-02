using UnityEngine;

namespace Clouds.Combat {
	public class HitByTheseFactions : MonoBehaviour, IVulnerabilityMap {
		[SerializeField] int[] factions = new int[1];

		public float CheckAttack (IAttack attack) {
			foreach (int faction in factions) {
				if (attack.Faction == faction) {
					return 1;
				}
			}

			return 0;
		}

	}
}