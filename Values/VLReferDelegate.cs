using TwitaScript.Compile;

namespace TwitaScript.Values
{

	public class VLReferDelegate : VBasic
	{

		private readonly int code;
		private readonly VParams pars;
		
		public VLReferDelegate(int varid, VParams ps)
		{
			code = varid;
			pars = ps;
		}

		public object TryInvoke()
		{
			VFunc delg = (VFunc) Globe.Lvs[Globe.Depth, code];//The delegate will turn into a function
			return delg.Invoke(pars);
		}

		public object Fixed => TryInvoke();

	}

}
