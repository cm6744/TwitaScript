using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Twita.Maths;

namespace TwitaScript.Compile
{

	public class OpExpr
	{

		public static Dictionary<string, OpExpr> Map = new();

		public static readonly OpExpr
		OR = new(97, "OR"),
		AND = new(98, "AND"),
		NE = new(99, "!="),
		LEQ = new(105, "~=="),
		NLEQ = new(105, "~!="),
		EQ = new(100, "=="),
		EQ_LG = new(101, ">="),
		EQ_RG = new(102, "<="),
		LG = new(103, ">"),
		RG = new(104, "<");

		int code;
		string key;

		OpExpr(int c, string k)
		{
			code = c;
			key = k;
			Map[key] = this;
		}

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public object Calc(dynamic v1, dynamic v2)
		{
			switch(code)
			{
				case 97:
					return v1 || v2;
				case 98:
					return v1 && v2;
				case 99:
					return v1 != v2;
				case 100:
					return v1 == v2;
				case 101:
					return v1 >= v2;
				case 102:
					return v1 <= v2;
				case 103:
					return v1 > v2;
				case 104:
					return v1 < v2;
				case 105:
					return Mth.Abs(v1 - v2) < 0.0001f;
				case 106:
					return Mth.Abs(v1 - v2) > 0.0001f;
			}
			throw new Exception("Unknown operator: " + code);
		}

	}

}
