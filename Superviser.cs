using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Common;
using TwitaScript.Compile;

namespace TwitaScript
{

	public class Superviser
	{

		public static void Test(string[] code, int times = 1, int warming = 0)
		{
			var i1 = Platform.Graph.Nanotime;
			Syntax syn = new();
			var seq = syn.Compile(code);
			var i2 = Platform.Graph.Nanotime;
			Console.WriteLine("-STT");
			Console.WriteLine("COMPILE: " + (i2 - i1) / 1000f / 1000f);
			for(int i = 0; i < warming; i++) Globe.Execute(seq);
			i1 = Platform.Graph.Nanotime;
			for(int i = 0; i < times; i++) Globe.Execute(seq);
			i2 = Platform.Graph.Nanotime;
			Console.WriteLine("EXECUTE: " + (i2 - i1) / 1000f / 1000f);
			Console.WriteLine("-END");
		}

	}

}
