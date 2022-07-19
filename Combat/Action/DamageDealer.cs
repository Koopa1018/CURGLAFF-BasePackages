using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using Clouds.Combat;

namespace Clouds.Combat.ActionGame.Weapons {
	[AddComponentMenu("Combat/Passive Damage Dealer")]
	public class DamageDealer : MonoBehaviour, IAttack {
		[SerializeField] int _faction;
		public int Faction => _faction;
		[SerializeField] float _strength;
		public float Strength {get => _strength; set => _strength = value;}
	}
}