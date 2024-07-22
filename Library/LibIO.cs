using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Codec;
using Twita.Codec.General;
using Twita.Common.Toolkit;
using Twita.Script.Structs;
using TwitaScript.Library;
using TwitaScript.Values;

namespace TwitaScript.Library
{

	public class LibIO : Lib
	{

		public override void Load()
		{
			LibName = "IO";

			Functions["FOpen"] = (values) =>
			{
				if(values[1])
					return FileSystem.GetLocal(values[0]);
				return FileSystem.GetAbsolute(values[0]);
			};
			Functions["ReadBytes"] = (values) =>
			{
				return BytesIO.Read(values[0]);
			};
			Functions["ReadTexts"] = (values) =>
			{
				if(values[1])
					return StringIO.ReadArray(values[0]);
				return StringIO.Read(values[0]);
			};
			Functions["ReadTwid"] = (values) =>
			{
				return TwidIO.Read(values[0], values[1]);
			};
			Functions["WriteConsole"] = (values) =>
			{
				Console.WriteLine(VBasic.EnsureS(values[0]));
				return null;
			};
			Functions["WriteLog"] = (values) =>
			{
				Log.Info(VBasic.EnsureS(values[0]));
				return null;
			};
		}

	}

}
