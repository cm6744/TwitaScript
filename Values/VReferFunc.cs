using System.Runtime.CompilerServices;
using TwitaScript.Compile;

namespace TwitaScript.Values
{

	public class VReferFunc : VBasic
	{

		private readonly VFunc func;
		private readonly VParams pars;

		public VReferFunc(VFunc func, VParams pars)
		{
			this.func = func;
			this.pars = pars;
		}

		public object TryInvoke()
		{
			return func.Invoke(pars);
		}

		public object Fixed => TryInvoke();

	}

}
