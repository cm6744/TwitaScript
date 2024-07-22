using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitaScript.Values
{

	public class VLinkedOp : VBasic
	{

		List<object> List = new();

		public VLinkedOp(List<object> lst)
		{
			List = lst;
		}

		public object Fixed => TryInvoke();

		private static dynamic TMP = null;

		public object TryInvoke()
		{
			object o;
			for(int i = 0; i < List.Count; i++)
			{
				o = List[i];
				if(TMP == null)
				{
					TMP = o;
					continue;
				}
				if(o is char op)
				{
					dynamic o1 = VBasic.Cast(List[i + 1]);
					switch(op)
					{
						case '+': TMP += o1; break;
						case '-': TMP -= o1; break;
						case '*': TMP *= o1; break;
						case '/': TMP /= o1; break;
					}
					i++;
				}
			}
			o = TMP;
			TMP = null;
			return o;
		}

	}

}
