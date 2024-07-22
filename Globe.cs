using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TwitaScript.Compile;
using TwitaScript.Library;
using TwitaScript.Values;
using TwitaScript.Library;
using Twita.Codec;

namespace TwitaScript
{

	public static class Globe
	{

		public static void PrepareEnv()
		{
			GlobalLib = new LibOverall();
			GlobalLib.Link(new LibStructs());
			GlobalLib.Link(new LibMaths());
			GlobalLib.Link(new LibIO());
			GlobalLib.Link(new LibString());
			GlobalLib.Link(new LibInterop());
		}

		//LIBING

		public static LibOverall GlobalLib;

		//EXECUTE

		public static dynamic[,] Lvs = new dynamic[128, 64];		//Locals
		public static Dictionary<string, dynamic> Gvs = new(256);   //Globals
		public static int Depth = 0;
		public static int Line = -1;
		public static bool Interrupted;

		public static void ExecuteFunction(Domain seq, string ivkname, params object[] pass)
		{
			seq.FuncDefs.Search(ivkname).Invoke(new VParams(pass));
			Terminate();
		}

		public static void Execute(Domain seq, params object[] pass)
		{
			seq.Init.Invoke(new VParams(pass));
			Terminate();
		}

		public static void ExecuteFunction(Domain seq, string ivkname)
		{
			seq.FuncDefs.Search(ivkname).Invoke(null);
			Terminate();
		}

		public static void Execute(Domain seq)
		{
			seq.Init.Invoke(null);
			Terminate();
		}

		public static void Terminate()
		{
			Line = -1;
			Interrupted = false;
			Gvs.Clear();
		}

		public static void Inject(string key, object v)
		{
			Gvs[key] = v;
		}

		public static void PushParams(VParams ps)
		{
			for(int i = 0, j = ps.Len; i < j; i++)
			{
				Lvs[Depth + 1, i] = ps[i];
			}
		}

		public static object Execute(VParams values, List<Operation> lines)
		{
			//If inc first then push, the VRefer cannot get local vars. So we push first to next frame.
			if(values != null) PushParams(values);
			Depth++;
			object val;
			for(int i = 0, j = lines.Count; i < j; i++)
			{
				val = Execute(lines[i]);
				if(val != null)
				{
					Depth--;
					return val;
				}
				if(Line != -1)
				{
					i = Line;
					Line = -1;
				}
			}
			Depth--;
			return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static object Execute(Operation code)
		{
			if(code.Opcode == Opcode.None) return null;

			switch(code.Opcode)
			{
				case Opcode.LVarinc: MM3(code); break;
				case Opcode.LVarset: MM2(code); break;

				case Opcode.Funccall: MM4(code); break;
				case Opcode.FuncLDcall: MM41(code); break;
				case Opcode.FuncGDcall: MM42(code); break;

				case Opcode.JumpUnless: MM5(code); break;
				case Opcode.JumpIf: MM6(code); break;
				case Opcode.Jump: MM7(code); break;

				case Opcode.GVarset: MM00(code); break;

				case Opcode.Return: return VBasic.Cast(code.O1);
				case Opcode.Interrupt: Interrupted = true; break;
			}

			return null;
		}

		//FOR JIT
		private static void MM00(Operation code) { Gvs[(string) code.O1] = VBasic.Cast(code.O2); }
		private static void MM2(Operation code) { Lvs[Depth, code.AC1] = VBasic.Cast(code.O2); }
		private static void MM3(Operation code) { Lvs[Depth, code.AC1]++; }
		private static void MM4(Operation code) { ((VFunc) code.O1).Invoke((VParams) code.O2); }
		private static void MM41(Operation code) { ((VLReferDelegate) code.O1).TryInvoke(); }
		private static void MM42(Operation code) { ((VGReferDelegate) code.O1).TryInvoke(); }
		private static void MM5(Operation code) { if(!VBasic.Cast(code.O2)) Line = code.AC1; }
		private static void MM6(Operation code) { if(VBasic.Cast(code.O2)) Line = code.AC1; }
		private static void MM7(Operation code) { Line = code.AC1; }

	}

}
