using TwitaScript.Compile;

namespace TwitaScript.Values
{

	public class VGRefer : VBasic
	{

		private readonly string coderaw;

		public VGRefer(string raw)
		{
			coderaw = raw;
		}

		public object Fixed
		{
			get
			{
				return VBasic.Cast(Globe.Gvs[coderaw]);
			}
		}

	}

}
