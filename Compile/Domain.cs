using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitaScript.Values;
using TwitaScript.Library;

namespace TwitaScript.Compile
{

	public class Domain
	{

		public VFunc Init;//Nullable
		public Lib FuncDefs;

		public void ExecuteFunction(string ivkname, params object[] pass)
		{
			Globe.ExecuteFunction(this, ivkname, pass);
		}

		public void Execute(params object[] pass)
		{
			Globe.Execute(this, pass);
		}

		public void ExecuteFunction(string ivkname)
		{
			Globe.ExecuteFunction(this, ivkname);
		}

		public void Execute()
		{
			Globe.Execute(this);
		}

	}

}
