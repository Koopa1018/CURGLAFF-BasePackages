using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.LowLevel;

namespace Clouds.CustomPlayerLoop
{
	public static class PlayerLoopExtend {
		static int i;
		public static void AddToFixedUpdate (PlayerLoopSystem before, PlayerLoopSystem after, int beforeOrder = 0, bool listFixedUpdateContents = false) {
			PlayerLoopSystem pl = PlayerLoop.GetCurrentPlayerLoop();

			for (i = 0; i < pl.subSystemList.Length; i++) {
				if (pl.subSystemList[i].type.IsEquivalentTo(typeof(UnityEngine.PlayerLoop.FixedUpdate))) {
					List<PlayerLoopSystem> plSystems = new List<PlayerLoopSystem>(pl.subSystemList[i].subSystemList);
					
					//Insert the before systems.
					plSystems.Insert(beforeOrder, before);

					//Add the after systems.
					plSystems.Add(after);

					//Now's the time to Debug.Log if desired.
					if (listFixedUpdateContents) {
						Debug.Log($"Added systems to FixedUpdate:");

						foreach(PlayerLoopSystem subs in plSystems) {
							Debug.Log("\t- " + subs.type.Name);
						}
					}

					//Instate the modified FixedUpdate function.
					pl.subSystemList[i].subSystemList = plSystems.ToArray();
				}
			}
		}
		public static void AddToFixedUpdate (PlayerLoopSystem[] before, PlayerLoopSystem[] after, int beforeOrder = 0, bool listFixedUpdateContents = false) {
			PlayerLoopSystem pl = PlayerLoop.GetCurrentPlayerLoop();

			for (i = 0; i < pl.subSystemList.Length; i++) {
				if (pl.subSystemList[i].type.IsEquivalentTo(typeof(UnityEngine.PlayerLoop.FixedUpdate))) {
					List<PlayerLoopSystem> plSystems = new List<PlayerLoopSystem>(pl.subSystemList[i].subSystemList);
					
					//Insert the before systems.
					plSystems.InsertRange(beforeOrder, before);

					//Add the after systems.
					plSystems.AddRange(after);

					//Now's the time to Debug.Log if desired.
					if (listFixedUpdateContents) {
						Debug.Log($"Added systems to FixedUpdate:");

						foreach(PlayerLoopSystem subs in plSystems) {
							Debug.Log("\t- " + subs.type.Name);
						}
					}

					//Instate the modified FixedUpdate function.
					pl.subSystemList[i].subSystemList = plSystems.ToArray();

				}
			}
		}

	}
}