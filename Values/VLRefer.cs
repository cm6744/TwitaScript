using TwitaScript.Compile;

namespace TwitaScript.Values
{

	public class VLRefer : VBasic
	{

		private readonly int code;
		
		public VLRefer(int varid)
		{
			code = varid;
		}

		public object Fixed
		{
			get
			{
				return VBasic.Cast(Globe.Lvs[Globe.Depth, code]);
			}
		}

	}

}
