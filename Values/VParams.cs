using System.Collections.Generic;

namespace TwitaScript.Values
{

	public class VParams
	{

		private readonly object[] vals;

		public VParams(List<object> vals)
		{
			this.vals = vals.ToArray();
		}

		public VParams(object[] vals)
		{
			this.vals = vals;
		}

		public dynamic this[int index]
		{
			get => VBasic.Cast(vals[index]);
		}

		public object[] Prims => vals;
		public int Len => vals.Length;

	}

}
