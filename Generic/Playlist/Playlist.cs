using UnityEngine;
using Unity.Mathematics;

namespace Clouds.Generic {
	[System.Serializable]
	public struct Playlist<T> {
		public enum SequenceMode{Sequential, Random/*, ShuffleEachCycle*/};
		public enum EndMode {Repeat, Clamp, Mirror};

		[SerializeField] SequenceMode sequenceMode;
		[SerializeField] EndMode repetitionMode;

		[SerializeField] T[] elements;

		int current;
		bool isDone;

		public T GetNext () {
			T returner = elements[math.abs(current)];

			switch (sequenceMode) {
				case SequenceMode.Sequential:
					current++;
					isDone = current >= elements.Length;
					switch (repetitionMode){
						case EndMode.Repeat:
							current %= elements.Length;
							break;
						case EndMode.Clamp:
							current = math.min(current, elements.Length - 1);
							break;
						case EndMode.Mirror:
							current = math.select(
								current,
								-(elements.Length - 1),
								current >= elements.Length
							);
							break;
					}
					break;
				case SequenceMode.Random:
					current = UnityEngine.Random.Range(0, elements.Length);
					break;
			}

			return returner;
		}

		public void Reset () {
			current = 0;
			isDone = false;
		}
		

	}
}