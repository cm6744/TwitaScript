using TwitaScript.Compile;

namespace TwitaScript.Values
{

	public class VGReferDelegate : VBasic
	{

		private readonly string code;
		private readonly VParams pars;

		public VGReferDelegate(string raw, VParams ps)
		{
			code = raw;
			pars = ps;
		}

		public object TryInvoke()
		{
			VFunc delg = (VFunc) Globe.Gvs[code];
			return delg.Invoke(pars);
		}

		public object Fixed => TryInvoke();

	}

}
