using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clouds.Combat {
	public interface IAttack {
		int Faction {get;}

		float Strength {get;}
	}
}