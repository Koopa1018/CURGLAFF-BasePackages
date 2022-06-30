using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;

using Clouds.Combat;

namespace Clouds.Combat.ActionGame.Weapons {
	[AddComponentMenu("Combat/I'm A Weapon Strike")]
	public class WeaponStrike : DamageDealer {
		//INVARIANTS TO BE UPHELD:
		//- Strike timer is always 0 or greater.
		//- Strike timer threshold is always 0 or greater.
		//- Strike timer ticks every frame.
		//- Strike timer is always set to 0 when an attack is begun.
		//- Strike timer is always set to 0 when an attack ends.
		//- Strike timer is not checked against the threshold (does not "ring") unless attack is active.
		//- Strike timer never rings if the threshold is 0.
		//- Attack active becomes true only when an attack begins.
		//- Attack active becomes false when an attack ends, including by timer ring.

		[System.Flags] public enum ResetEvent {
			None, 
			Begin = 1,
			Reset = 2,
			End = 4
		}
		//public enum CollideMode {Overlap, Directional};
		
		const int UNINITIALIZED = -32768;

		[Tooltip("How long the strike lasts before ending. 0 = never ends.")]
		[SerializeField] [Min(0)] float strikeDuration = 0.15f;
		[Tooltip("How many hits this attack can give before ending. 0 = infinite.")]
		[SerializeField] [Min(0)] int endAfterHits = 0;
		[Tooltip("If set, even non-reactive objects will count towards the end-after-hits tally.")]
		[SerializeField] bool countNonReactorHits = false;
		[Tooltip("If set, a new attack can abort an attack which has already begun. If clear, new attacks will be ignored while old ones are in progress.")]
		[SerializeField] bool allowResettingMidAttack = false;

		[SerializeField] LayerMask targetLayers;
		//[SerializeField] CollideMode collideMode;
		[SerializeField] Collider2D[] strikeColliders;

		[Header("Attack Events")]
		[Tooltip("Which of these events should be run when the attack is reset? Only used if allowResettingMidAttack is set.")]
		[SerializeField] ResetEvent resetEvent = ResetEvent.Begin | ResetEvent.End;

		[Tooltip("Event run when this strike is used in an attack.")]
		[SerializeField] public UnityEvent<int> onBeginStrike = new UnityEvent<int>();
		[Tooltip("Event run when this strike is used in an attack, and is already being used for an attack.")]
		[SerializeField] public UnityEvent<int> onResetStrike = new UnityEvent<int>();
		[Tooltip("Event run when this strike ends being used.")]
		[SerializeField] public UnityEvent<int> onEndStrike = new UnityEvent<int>();

		[Header("Hit Events")]
		[Tooltip("Event run for every reactor hit.")]
		[SerializeField] UnityEvent<WeaponStrike> onHitIndividualReactor = new UnityEvent<WeaponStrike>();
		[Tooltip("Event run for every non-reactor hit.")]
		[SerializeField] UnityEvent onHitIndividualNonReactor = new UnityEvent();
		[Tooltip("Event run once per frame if ANY object was hit, reactive or not.")]
		[SerializeField] UnityEvent afterHitAnyObject = new UnityEvent();
		[Tooltip("Event run once per frame if ANY reactor was hit.")]
		[UnityEngine.Serialization.FormerlySerializedAs("onHitReactor")]
		[SerializeField] UnityEvent afterHitAnyReactor = new UnityEvent();
		[Tooltip("Event run once per frame if ONLY non-reactors were hit.")]
		[UnityEngine.Serialization.FormerlySerializedAs("onHitSomethingElse")]
		[SerializeField] UnityEvent afterHitOnlyNonReactors = new UnityEvent();

		[Header("Advanced")]
		[Tooltip("How big to initialize the colliders-overlapped list that's written to each frame.")]
		int hitListInitCapacity = 2;

		float strikeTimer = 0;
		bool attackActive = false;
		int hitsGiven = 0;

		/// <summary>
		/// How far into the current strike we are, in seconds. If no strike is active, returns -1. 
		/// </summary>
		public float CurrentLifetime => attackActive ? strikeTimer : -1;
		/// <summary>
		/// How far into the current strike we are, between 0 (start) and 1 (end). If no strike is active, returns -1. 
		/// </summary>
		public float CurrentLifetimeNormalized => attackActive ? strikeTimer / strikeDuration : -1;
		/// <summary>
		/// How long it's been since the last strike ended (or this component was created), in seconds. If a strike is currently active, returns -1. 
		/// </summary>
		public float TimeSinceLastStrike => attackActive ? -1 : strikeTimer;

		/// <summary>
		/// How many hits this attack can give before ending. 0 = infinite. Never less than 0.
		/// </summary>
		public int EndAfterNHits => endAfterHits;

		static int NEXT_STRIKE = 0;
		int weaponStrikeID = UNINITIALIZED;

		//Defined and created out here to avoid GC alloc per frame.
		ContactFilter2D hitFilter;
		//Likewise with these.
		List<Collider2D> hitsTmp;
		HashSet<Collider2D> hits;

		void Start () {
			//Initialize with the strike.
			hitFilter = new ContactFilter2D();
			hitFilter.SetLayerMask(targetLayers);

			hitsTmp = new List<Collider2D>(hitListInitCapacity);
			hits = new HashSet<Collider2D>(hitListInitCapacity);
		}

		void OnValidate () {
			//Refresh hit mask when it might have changed.
			hitFilter.SetLayerMask(targetLayers);
		}

		public void BeginNew () {
			if (attackActive && !allowResettingMidAttack) {
				return;
			}

			//Update ID
			int lastID = NEXT_STRIKE;

			weaponStrikeID = NEXT_STRIKE++;

			//If we're now suddenly _lower_ than the current strike ID, we've wrapped around and should clear all old-hits arrays.
			//This ought to keep things working right, at the expense of potentially being hit by very, very old one-shot strikes twice.
			if (NEXT_STRIKE < lastID) {
				//Stub
			}

			//Enable collision detection and event consumption
			this.enabled = true;

			//Run reset events if resetting
			if (attackActive) {
				if (resetEvent.HasFlag(ResetEvent.End)) {
					onEndStrike?.Invoke(lastID);
				}
				if (resetEvent.HasFlag(ResetEvent.Reset)) {
					onResetStrike?.Invoke(weaponStrikeID);
				}
				if (resetEvent.HasFlag(ResetEvent.Begin)) {
					onBeginStrike?.Invoke(weaponStrikeID);
				}
			}
			//Run begin event if not
			else {
				onBeginStrike?.Invoke(weaponStrikeID);
			}

			//Set strike timer
			strikeTimer = 0;
			attackActive = true;
			
			//Set hits given
			hitsGiven = 0;
		}

		public void End () {
			strikeTimer = 0;
			attackActive = false;

			this.enabled = false;
			onEndStrike?.Invoke(weaponStrikeID);
		}

		void FixedUpdate () {
			//Tick the strike timer.
			strikeTimer += Time.fixedDeltaTime;

			//If the strike timer is up, end the attack.
			//(If end's been already called, attackActive == false, so this won't run it twice.)
			if (strikeDuration != 0 && attackActive && strikeTimer >= strikeDuration) {
				End();
			}

			//If no attack is active (incl. if timer just rang), we're done here.
			if (!attackActive) {
				return;
			}


			//Attack is active.
			
			//Do overlap checks on all our colliders.
			overlapAllColliders(hitFilter, ref hits);

			bool hitSomething = false;
			bool hitReactor = false;
			
			//Interact objects which can react to weapon strikes.
			foreach (Collider2D hit in hits) {
				DamageReactor reactor;
				DamageReactor.ReactorsInScene.TryGetValue(hit, out reactor);
				reactor?.React(this);
				if (reactor != null) {
					Debug.Log("Hit reactor: " + reactor.ToString());
				}
				
				//We've definitely hit SOMEthing.
				hitSomething = true;

				bool hitReactorThisTime = reactor != null; //Set separately from hitReactor so we don't react to past hits.
				hitReactor |= hitReactorThisTime; //But if we hit one this time, do log that we hit one at all.
				
				if (hitReactorThisTime) {
					onHitIndividualReactor?.Invoke(this);
				}
				else { //Didn't hit a reactor--but did hit SOMETHING!
					onHitIndividualNonReactor?.Invoke();
				}

				//Count up how many hits have been given by this strike.
				hitsGiven += (countNonReactorHits || hitReactorThisTime)? 1 : 0;
				if (hitsGiven >= endAfterHits) {
					//If we've filled our hits-given quota, end the attack...
					End();
					//..and the collider search.
					break;
				}
			}
			
			//Hits list has served its purpose. Empty it for next frame.
			hits.Clear();


			//React if we hit an object before last update.
			if (hitSomething) {
				afterHitAnyObject?.Invoke();
			}
			if (hitReactor) {
				afterHitAnyReactor?.Invoke();
			}
			if (hitSomething && !hitReactor) {
				afterHitOnlyNonReactors?.Invoke();
			}		
		}


		void overlapAllColliders (ContactFilter2D filter, ref HashSet<Collider2D> hits) {
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
	}
}