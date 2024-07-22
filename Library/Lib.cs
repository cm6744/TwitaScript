using System.Collections.Generic;
using TwitaScript.Values;

namespace TwitaScript.Library
{

	public class Lib
	{

		public string LibName;
		public Dictionary<string, VFunc> Functions = new();
		public Dictionary<string, object> Consts = new();

		public virtual VFunc Search(string name)
		{
			return Functions.GetValueOrDefault(name, null);
		}

		public virtual object TryConst(string name)
		{
			return Consts.GetValueOrDefault(name, null);
		}

		public virtual void Load() { }

	}

}
