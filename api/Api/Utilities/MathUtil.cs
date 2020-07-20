﻿using System;
using MathNet.Spatial.Euclidean;

namespace SynthesisAPI.Utilities
{
	public static class MathUtil
	{
		public static Quaternion FromEuler(EulerAngles inAngle)
		{
			// Math from https://math.stackexchange.com/questions/2975109/how-to-convert-euler-angles-to-quaternions-and-get-the-same-euler-angles-back-fr

			var yaw = inAngle.Alpha.Radians;
			var pitch = inAngle.Beta.Radians;
			var roll = inAngle.Gamma.Radians;

			var sinYaw = Math.Sin(yaw / 2);
			var cosYaw = Math.Cos(yaw / 2);
			var sinPitch = Math.Sin(pitch / 2);
			var cosPitch = Math.Cos(pitch / 2);
			var sinRoll = Math.Sin(roll / 2);
			var cosRoll = Math.Cos(roll / 2);

			var qx = sinRoll * cosPitch * cosYaw - cosRoll * sinPitch * sinYaw;

			var qy = cosRoll * sinPitch * cosYaw + sinRoll * cosPitch * sinYaw;

			var qz = cosRoll * cosPitch * sinYaw - sinRoll * sinPitch * cosYaw;

			var qw = cosRoll * cosPitch * cosYaw + sinRoll * sinPitch * sinYaw;

			// This z, y, x order works correctly with interchanging Quaternions and EulerAngles
			// using Quaternion.ToEulerAngles()
			return new Quaternion(qw, qz, qy, qx);
		}

		public static Vector3D ToVector(EulerAngles eulerAngles)
		{
			return new Vector3D(eulerAngles.Alpha.Degrees, eulerAngles.Beta.Degrees, eulerAngles.Gamma.Degrees);
		}
	}
}