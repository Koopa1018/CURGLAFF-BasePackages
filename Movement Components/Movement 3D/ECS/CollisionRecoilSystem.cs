using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

using Clouds.MovementBase;
using Clouds.Movement3D;


namespace Clouds.Collision3D {
	[UpdateBefore(typeof(CollisionsGroup))]
	public class CollisionRecoilSystem3D_Before : SystemBase {
		protected override void OnUpdate() {	
			Entities.ForEach((ref CollisionRecoil3DComponent recoil, in Translation translation, in Velocity3DComponent velocity) => {
				//Store the current position, unchanged, into the collision recoil to be processed later.
				recoil.Value = velocity.Value;
			}).Schedule();
		}
	}
	[UpdateAfter(typeof(CollisionsGroup))] 
	[UpdateBefore(typeof(MovementFinalizer3D))]
	public class CollisionRecoilSystem3D_After : SystemBase {
		protected override void OnUpdate() {	
			Entities.ForEach((ref CollisionRecoil3DComponent recoil, in Translation translation, in Velocity3DComponent velocity) => {
				//After CollisionsGroup but before MovementFinalizer3D, the velocity will have been changed.
				//So the true recoil will be _from_ old velocity to new velocity!
				recoil.Value = recoil.Value - velocity.Value;
			}).Schedule();
		}
	}
}