using System.Runtime.CompilerServices;
using TwitaScript.Compile;

namespace TwitaScript.Values
{

	public class VDelegate : VBasic
	{

		private readonly VFunc func;
		
		public VDelegate(VFunc func)
		{
			this.func = func;
		}

		public object TryInvoke(VParams pars)
		{
			return func.Invoke(pars);
		}

		public object Fixed => func;

	}

}
