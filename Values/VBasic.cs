using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TwitaScript.Values;
using Twita.Common;

namespace TwitaScript.Values
{

	public interface VBasic
	{

		public static object Undefined = new object();

		public object Fixed { get; }

		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		public static dynamic Cast(object o)
		{
			return o is VBasic v ? v.Fixed : o;
		}

		public static string EnsureS(object o)
		{
			object o1 = Cast(o);
			return o1 == null ? "Null" : o1.ToString();
		}

	}

}
