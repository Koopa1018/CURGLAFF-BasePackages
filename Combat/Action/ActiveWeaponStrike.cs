using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;

using Clouds.Combat;

namespace Clouds.Combat.ActionGame.Weapons {
	[RequireComponent(typeof(WeaponStrike))]
	public class ActiveWeaponStrike : MonoBehaviour {
		const int HIT_DICTIONARY_CAPACITY = 8; //@TODO: does this need to be aligned as PoT?
		//public enum CollideMode {Overlap, Directional};

		[SerializeField] WeaponStrike myStrike;

		[SerializeField] Collider2D[] strikeColliders;
		//[SerializeField] CollideMode collideMode;
		[SerializeField] LayerMask targetLayers;
		[SerializeField] bool clearReactionCacheOnStart = true;

		[SerializeField] UnityEvent onHitReactor = new UnityEvent();
		[SerializeField] UnityEvent onHitSomethingElse = new UnityEvent();

		[Header("Advanced")]
		[Tooltip("How big to initialize the colliders-overlapped list that's written to each frame.")]
		int hitListInitCapacity = 8;
		[Tooltip(@"If left unchecked, objects that have a ReactToWeaponStrike component when first detected by a strike will be assumed to keep that component until destroyed.
Check this to GetComponent on every ReactToWeaponStrike-less object every frame, which can make the game quite slow but will detect when objects have the component added."
		)]
		bool useAccurateReactionCheck = false;


		/// <summary>
		/// A list of Collider2Ds we've collided.
		/// </summary>
		List<Collider2D> hitsTmp;
		/// <summary>
		/// So that we can't check a collider twice, we dump our hits list into a HashSet after every check.
		/// </summary>
		HashSet<Collider2D> hits;

		/// <summary>
		/// Colliders that've been collided with and found to have ReactToWeaponStrike components.
		/// (The assumption is that this isn't going to change during a frame.)
		/// <para/> If this were ECS, we'd not need this because we could just put weapon strikes on each
		/// hit object in a chunk with "ReactToWeaponStrike," run systems on chunks with "WeaponStruck(int)"
		/// components alongside "ReactThisWay<WeaponStruck>" components, and then remove all WeaponStrucks!~
		/// </summary>
		static Dictionary<Collider2D, ReactToWeaponStrike> foundWith;
		/// <summary>
		/// Colliders that've been collided with and found to be missing ReactToWeaponStrike components.
		/// (The assumption is that this isn't going to change during a frame.)
		/// <para/> If this were ECS, we'd not need this because we could just put weapon strikes on each
		/// hit object in a chunk with "ReactToWeaponStrike," run systems on chunks with "WeaponStruck(int)"
		/// components alongside "ReactThisWay<WeaponStruck>" components, and then remove all WeaponStrucks!~
		/// </summary>
		static HashSet<Collider2D> foundWithout;

		void Start () {
			//Initialize our hits-check list with a configurable capacity.
			hitsTmp = new List<Collider2D>(hitListInitCapacity);
			hits = new HashSet<Collider2D>();

			//Initialize our found-reactors list if need be.
			if (foundWith == null) {
				foundWith = new Dictionary<Collider2D, ReactToWeaponStrike>(HIT_DICTIONARY_CAPACITY);
			}
			//Or clear it if need be. @TODO: Should probably only run on scene unload, to avoid pointless cache wipes.
			else if (foundWith.Count > 0) {
				foundWith.Clear();
			}

			//Do likewise with the no-found-reactors list.
			if (foundWithout == null) {
				foundWithout = new HashSet<Collider2D>();
			}
			else if (foundWithout.Count > 0) {
				foundWithout.Clear();
			}
		}

		void FixedUpdate () {
			//To filter overlapped colliders by.
			ContactFilter2D hitFilter = new ContactFilter2D();
			hitFilter.SetLayerMask(targetLayers);
			
			//Do overlap checks on all our colliders.
			overlapAllColliders(hitFilter, ref hitsTmp, ref hits);

			bool didHitReactor = false;
			
			//Interact objects which can react to weapon strikes.
			foreach (Collider2D hit in hits) {
				ReactToWeaponStrike reactor = getReactor(hit);
				reactor?.React(myStrike);
				
				didHitReactor |= reactor != null;
			}

			if (didHitReactor) {
				onHitReactor?.Invoke();
			}
			else if (hits.Count > 0) { //Didn't hit a reactor--but did hit SOMETHING!
				onHitSomethingElse?.Invoke();
			}

			hits.Clear();
		}

		void overlapAllColliders (ContactFilter2D filter, ref List<Collider2D> hitsTmp, ref HashSet<Collider2D> hits) {
			//Check each collider of ours against collideable objects.
			foreach (Collider2D col in strikeColliders) {
				if (col.isActiveAndEnabled) {
					Physics2D.OverlapCollider(col, filter, hitsTmp);
				}
			}

			foreach (Collider2D hit in hitsTmp) {
				hits.Add(hit);
			}
			hitsTmp.Clear();
		}

		ReactToWeaponStrike getReactor (Collider2D tested) {
			ReactToWeaponStrike returner;

			
			//If this object's been found with a reaction maker, return the previous result.
			if (foundWith.TryGetValue(tested, out returner)) {
				//VALIDATE: Is the previous result a real component?
				if (returner == null) {
					//If it's null, the component was deleted and we shouldn't store this reference.
					foundWith.Remove(tested);
				}

				//Return the found ReactToWeaponStrike (null or not~!).
				return returner;
			}
			//If this object's been found without reaction (and we want to rely on that assumption), return no reaction.
			else if (!useAccurateReactionCheck && foundWithout.Contains(tested)) {
				return null;
			}

			//The object's not known to be with; it's not known to be without; let's probe it so we know!
			//We get into returner because that's what we'll be returning anyway; no other variable is needed for this!~
			returner = tested.GetComponent<ReactToWeaponStrike>();

			//If it has, cache that.
			if (returner != null) {
				foundWith.Add(tested, returner);
			}
			//If it hasn't, cache that. (Even if we're doing slow checks, some other weapon strike will find that useful.)
			else {
				foundWithout.Add(tested);
			}

			return returner;
		}
				
	}
}