using System.Collections.Generic;
using System.Linq;
using TwitaScript.Compile;
using TwitaScript.Library;
using TwitaScript.Values;

namespace TwitaScript.Library
{

	public class LibOverall : Lib
	{

		public Dictionary<string, Lib> Vars = new();

		public void Link(Lib lib)
		{
			lib.Load();

			if(lib.LibName == null) return;
			if(Vars.ContainsKey(lib.LibName)) return;

			Vars[lib.LibName] = lib;
		}

		public VFunc SearchAvailables(List<string> lst, string name)
		{
			foreach(string s in lst)
			{
				Lib lib = Vars[s];
				VFunc func = lib.Search(name);
				if(func != null) return func;
			}
			return Search(name);
		}

		public object SearchConsts(List<string> lst, string name)
		{
			foreach(string s in lst)
			{
				Lib lib = Vars[s];
				object o = lib.TryConst(name);
				if(o != null) return o;
			}
			return null;
		}

	}

}
