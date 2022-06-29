using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Unity.Mathematics;

namespace Clouds.Facing2D.Tests {
	public class TestFlipLogic
	{
		// A Test behaves as an ordinary method
		[Test]
		public void TestSignDidNotFlip()
		{
			int2 newInt, oldInt;
			
			//Test unchanged case.
			newInt = new int2(1,1);
			oldInt = new int2(1,1);

			Assert.False(
				math.all(FlipUtils.checkSignFlip(newInt,oldInt)),
				"Detected a sign flip between equal old/new int2s (1,1)."
			);
		}

		[Test]
		public void TestSignFlipX()
		{
			int2 newInt, oldInt;

			oldInt = new int2(1,1);
			newInt = new int2(-1,1);

			//See if we detect flipping X positive to negative.
			bool2 flipCase = FlipUtils.checkSignFlip(newInt,oldInt);
			Assert.True(
				flipCase.x,
				"Did not detect X sign flip from 1 to -1."
			);
			//Catch false positives on Y also, just in case.
			Assert.False(
				flipCase.y,
				"False positive on Y when sign flipped on X."
			);

			//Test flipping the opposite direction too, negative to positive.
			flipCase = FlipUtils.checkSignFlip(oldInt,newInt);
			Assert.True(
				flipCase.x,
				"Did not detect X sign flip from -1 to 1."
			);
			Assert.False(
				flipCase.y,
				"False positive on Y when sign flipped on X."
			);
		}

		[Test]
		public void TestSignFlipY()
		{
			int2 newInt, oldInt;

			oldInt = new int2(1,1);
			newInt = new int2(1,-1);

			//See if we detect flipping X positive to negative.
			bool2 flipCase = FlipUtils.checkSignFlip(newInt,oldInt);
			Assert.True(
				flipCase.y,
				"Did not detect Y sign flip from 1 to -1."
			);
			//Catch false positives on Y also, just in case.
			Assert.False(
				flipCase.x,
				"False positive on X when sign flipped on Y."
			);

			//Test flipping the opposite direction too, negative to positive.
			flipCase = FlipUtils.checkSignFlip(oldInt,newInt);
			Assert.True(
				flipCase.y,
				"Did not detect Y sign flip from -1 to 1."
			);
			Assert.False(
				flipCase.x,
				"False positive on X when sign flipped on Y."
			);
		}

		

		[Test]
		public void TestSignFlipXY()
		{
			int2 newInt, oldInt;

			oldInt = new int2(1,1);
			newInt = new int2(-1,-1);

			//See if we detect flipping both axes positive to negative.
			bool2 flipCase = FlipUtils.checkSignFlip(newInt,oldInt);
			Assert.True(
				flipCase.x,
				"Did not detect X sign flip from 1 to -1."
			);
			Assert.True(
				flipCase.y,
				"Did not detect Y sign flip from 1 to -1."
			);

			//Test flipping the opposite direction too, negative to positive.
			flipCase = FlipUtils.checkSignFlip(oldInt,newInt);
			Assert.True(
				flipCase.x,
				"Did not detect X sign flip from -1 to 1."
			);
			Assert.True(
				flipCase.y,
				"Did not detect Y sign flip from -1 to 1."
			);
		}

		[Test]
		public void TestSign0OnX()
		{
			int2 newInt, oldInt;

			oldInt = new int2(1,1);
			newInt = new int2(0,1);

			//See if we properly ignore setting positive X to 0.
			bool2 flipCase = FlipUtils.checkSignFlip(newInt,oldInt);
			Assert.False(
				flipCase.x,
				"Detected setting X from 1 to 0 as a sign flip."
			);
			//Catch false positives on Y also, just in case.
			Assert.False(
				flipCase.y,
				"False positive on positive Y in a 1 -> 0 case on X."
			);

			oldInt = new int2(-1,1);

			//Test setting negative X to 0 also.
			flipCase = FlipUtils.checkSignFlip(newInt,oldInt);
			Assert.False(
				flipCase.x,
				"Detected setting X from -1 to 0 as a sign flip."
			);
			Assert.False(
				flipCase.y,
				"False positive on positive Y in a -1 -> 0 case on X."
			);
		}

		[Test]
		public void TestSign0OnY()
		{
			int2 newInt, oldInt;

			oldInt = new int2(1,1);
			newInt = new int2(1,0);

			//See if we properly ignore setting positive Y to 0.
			bool2 flipCase = FlipUtils.checkSignFlip(newInt,oldInt);
			Assert.False(
				flipCase.y,
				"Detected setting Y from 1 to 0 as a sign flip."
			);
			//Catch false positives on Y also, just in case.
			Assert.False(
				flipCase.x,
				"False positive on positive X in a 1 -> 0 case on Y."
			);

			oldInt = new int2(1,-1);

			//Test setting negative X to 0 also.
			flipCase = FlipUtils.checkSignFlip(newInt,oldInt);
			Assert.False(
				flipCase.y,
				"Detected setting Y from -1 to 0 as a sign flip."
			);
			Assert.False(
				flipCase.x,
				"False positive on positive X in a -1 -> 0 case on Y."
			);
		}
	}
}