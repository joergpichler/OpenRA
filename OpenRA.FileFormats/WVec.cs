#region Copyright & License Information
/*
 * Copyright 2007-2013 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.Drawing;

namespace OpenRA
{
	/// <summary>
	/// 3d World vector for describing offsets and distances - 1024 units = 1 cell.
	/// </summary>
	public struct WVec
	{
		public readonly int X, Y, Z;

		public WVec(int x, int y, int z) { X = x; Y = y; Z = z; }
		public WVec(WRange x, WRange y, WRange z) { X = x.Range; Y = y.Range; Z = z.Range; }

		public static readonly WVec Zero = new WVec(0, 0, 0);

		public static WVec operator +(WVec a, WVec b) { return new WVec(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
		public static WVec operator -(WVec a, WVec b) { return new WVec(a.X - b.X, a.Y - b.Y, a.Y - b.Y); }
		public static WVec operator -(WVec a) { return new WVec(-a.X, -a.Y, -a.Z); }

		public static bool operator ==(WVec me, WVec other) { return (me.X == other.X && me.Y == other.Y && me.Z == other.Z); }
		public static bool operator !=(WVec me, WVec other) { return !(me == other); }

		public static int Dot(WVec a, WVec b) { return a.X * b.X + a.Y * b.Y + a.Z * b.Z; }
		public int LengthSquared { get { return X * X + Y * Y + Z * Z; } }
		public int Length { get { return (int)Math.Sqrt(LengthSquared); } }

		public WVec Rotate(WRot rot)
		{
			var mtx = rot.AsMatrix();
			var lx = (long)X;
			var ly = (long)Y;
			var lz = (long)Z;
			return new WVec(
				(int)((lx * mtx[0] + ly*mtx[4] + lz*mtx[8]) / mtx[15]),
				(int)((lx * mtx[1] + ly*mtx[5] + lz*mtx[9]) / mtx[15]),
				(int)((lx * mtx[2] + ly*mtx[6] + lz*mtx[10]) / mtx[15]));
		}

		public override int GetHashCode() { return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode(); }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			WVec o = (WVec)obj;
			return o == this;
		}

		public override string ToString() { return "{0},{1},{2}".F(X, Y, Z); }
	}
}
