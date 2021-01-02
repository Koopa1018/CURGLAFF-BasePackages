using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using Clouds.Combat;

namespace Clouds.ActionGame.Weapons {
	[AddComponentMenu("Combat/I'm A Weapon Strike")]
	public class WeaponStrike : MonoBehaviour, IAttack {
		const int UNINITIALIZED = -32768;

		[SerializeField] int _faction;
		[SerializeField] bool _oneShot;

		public int Faction => _faction;
		public bool IsOneShot => _oneShot;

		static int NEXT_STRIKE = 0;
		int weaponStrikeID = UNINITIALIZED;

		public void BeginNew () {
			weaponStrikeID = NEXT_STRIKE++;

			//If we're now suddenly _lower_ than the current strike ID, we've wrapped around and should clear all old-hits arrays.
			//This ought to keep things working right, at the expense of potentially being hit by very, very old one-shot strikes twice.
		}
	}
}