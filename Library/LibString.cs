using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Common.Toolkit;
using TwitaScript.Values;
using TwitaScript.Library;

namespace TwitaScript.Library
{

	public class LibString : Lib
	{

		public override void Load()
		{
			LibName = "String";

			Functions["Concat"] = (values) =>
			{
				StringBuilder sb = new();
				foreach(object v in values.Prims) sb.Append(VBasic.EnsureS(v));
				return sb.ToString();
			};
			Functions["Spilt"] = (values) =>
			{
				string k = (string) values[0];
				return k.Split(values[1]);
			};
			Functions["Replace"] = (values) =>
			{
				string k = (string) values[0];
				return k.Replace(values[1], values[2]);
			};
			Functions["Idx"] = (values) =>
			{
				string k = (string) values[0];
				return k.IndexOf(values[1]);
			};
			Functions["LastIdx"] = (values) =>
			{
				string k = (string) values[0];
				return k.LastIndexOf(values[1]);
			};
			Functions["Trim"] = (values) =>
			{
				string k = (string) values[0];
				return k.Trim();
			};
		}

	}

}
