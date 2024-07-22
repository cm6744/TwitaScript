using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Common.Toolkit;
using TwitaScript.Values;
using TwitaScript.Library;
using System.Reflection;

namespace TwitaScript.Library
{

	public class LibInterop : Lib
	{

		public override void Load()
		{
			LibName = "Interop";

			Functions["MethOf"] = (values) =>
			{
				object o = values[0];
				var arr = o.GetType().GetMethods();

				foreach(var meth in arr)
				{
					if(meth.Name == values[1])
					{
						if(values.Len == 2) return meth;
						if(values[2] == meth.GetParameters().Length) return meth;
					}
				}

				return null;
			};
			Functions["Dcall"] = (values) =>
			{
				object o = values[0];
				MethodInfo meth = values[1];
				if(values.Len > 2)
				{
					object[] os = new object[values.Len - 2];
					Array.Copy(values.Prims, 2, os, 0, os.Length);
					return meth.Invoke(o, os);
				}
				return meth.Invoke(o, Array.Empty<object>());
			};
		}

	}

}
