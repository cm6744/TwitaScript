using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Codec;
using Twita.Codec.General;
using Twita.Common.Toolkit;
using TwitaScript.Library;

namespace TwitaScript.Library
{

	public class LibStructs : Lib
	{

		public override void Load()
		{
			LibName = "Structs";

			Functions["Array"] = (values) =>
			{
				return new object[(int) values[0]];
			};
			Functions["List"] = (values) =>
			{
				return new List<object>();
			};
			Functions["Map"] = (values) =>
			{
				return new Dictionary<object, object>();
			};
			Functions["Put"] = (values) =>
			{
				object o = values[0];
				switch(o)
				{
					case object[] arr:
						arr[(int) values[1]] = values[2];
						break;
					case Dictionary<object, object> map:
						map[values[1]] = values[2];
						break;
					case List<object> lst:
						lst[(int) values[1]] = values[2];
						break;
				}
				return null;
			};
			Functions["Get"] = (values) =>
			{
				object o = values[0];
				switch(o)
				{
					case object[] arr:
						return arr[(int) values[1]];
					case Dictionary<object, object> map:
						return map[values[1]];
					case List<object> lst:
						return lst[(int) values[1]];
				}
				return null;
			};
			Functions["Len"] = (values) =>
			{
				object o = values[0];
				switch(o)
				{
					case object[] arr:
						return arr.Length;
					case Dictionary<object, object> map:
						return map.Count;
					case List<object> lst:
						return lst.Count;
				}
				return 0;
			};
			Functions["Add"] = (values) =>
			{
				List<object> lst = values[0];
				lst.Add(values[1]);
				return null;
			};
			Functions["Insert"] = (values) =>
			{
				List<object> lst = values[0];
				lst.Insert((int) values[1], values[2]);
				return null;
			};
		}

	}

}
