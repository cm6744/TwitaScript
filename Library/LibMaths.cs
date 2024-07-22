using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Maths;
using TwitaScript.Library;

namespace TwitaScript.Library
{

	public class LibMaths : Lib
	{

		public override void Load()
		{
			LibName = "Maths";

			Functions["Add"] = (values) =>
			{
				return values[0] + values[1];
			};
			Functions["Sub"] = (values) =>
			{
				return values[0] - values[1];
			};
			Functions["Mul"] = (values) =>
			{
				return values[0] * values[1];
			};
			Functions["Div"] = (values) =>
			{
				return values[0] / values[1];
			};
			Functions["Mod"] = (values) =>
			{
				return values[0] % values[1];
			};
			Functions["Randf"] = (values) =>
			{
				return RandomGenerator.Global.NextFloat((float) values[0], (float) values[1]);
			};
			Functions["Randi"] = (values) =>
			{
				return RandomGenerator.Global.NextInt((int) values[0], (int) values[1]);
			};
			Functions["Pow"] = (values) =>
			{
				return (float) Math.Pow(values[0], values[1]);
			};
			Functions["Log10"] = (values) =>
			{
				return (float) Math.Log10((float) values[0]);
			};
			Functions["Log2"] = (values) =>
			{
				return (float) Math.Log2((float) values[0]);
			};
			Functions["Sind"] = (values) =>
			{
				return (float) Mth.SinDeg((float) values[0]);
			};
			Functions["Sinr"] = (values) =>
			{
				return (float) Mth.SinRad((float) values[0]);
			};
			Functions["Cosd"] = (values) =>
			{
				return (float) Mth.CosDeg((float) values[0]);
			};
			Functions["Cosr"] = (values) =>
			{
				return (float) Mth.CosRad((float) values[0]);
			};
			Functions["Abs"] = (values) =>
			{
				return (float) Math.Abs((float) values[0]);
			};
			Functions["Sgn"] = (values) =>
			{
				return (float) Math.Sign((float) values[0]);
			};
			Functions["Sqrt"] = (values) =>
			{
				return (float) Math.Sqrt((float) values[0]);
			};
			Functions["Clamp"] = (values) =>
			{
				return (float) Mth.Clamp((float) values[0], (float) values[1], (float) values[2]);
			};
			Functions["Floor"] = (values) =>
			{
				return (float) Mth.Floor((float) values[0]);
			};
			Functions["Ceil"] = (values) =>
			{
				return (float) Math.Ceiling((float) values[0]);
			};

			Consts["Pi"] = (float) Math.PI;
			Consts["E"] = (float) Math.E;
		}

	}

}
