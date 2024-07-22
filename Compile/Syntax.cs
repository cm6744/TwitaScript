using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TwitaScript.Library;
using TwitaScript.Values;
using Twita.Codec;
using Twita.Codec.General;
using Twita.Common;
using Twita.Common.Toolkit;

namespace TwitaScript.Compile
{

	public class Syntax
	{

		public static LineLimiter CodeLimiter = (code) =>
		{
			string codetrim = code.Trim();
			if(string.IsNullOrWhiteSpace(code) || codetrim.StartsWith("//"))
			{
				return false;
			}
			return true;
		};

		//***************//
		//	 COMPILING	 //
		//***************//

		//COMPILING tmps
		private Domain Seq;
		private SyntaxContext Ctx;
		private Stack<int> IfStack = new();
		private Stack<int> WhileStack = new();
		private string Exportname = null;
		
		public Domain Compile(string[] codes)
		{
			if(Seq != null)
			{
				throw new InvalidOperationException("A compiler cannot compile two or above vmwares.");
			}

			Seq = new();
			Ctx = new();
		
			Precode(codes);

			Seq.Init = Ctx.LocalLib.Search("Init");
			Seq.FuncDefs = Ctx.LocalLib;

			if(Exportname != null) Globe.GlobalLib.Link(Ctx.LocalLib);//Add to global lib

			return Seq;
		}

		public void Precode(string[] codes)
		{
			int linestart = 0;

			for(int line = 0; line < codes.Length; line++)
			{
				string code = codes[line];
				string codetrim = code.Trim();

				if(codetrim.StartsWith("use"))
				{
					string next = Strings.SubString2(codetrim, "use", ";").Trim();
					Ctx.Usings.Add(next);
				}
				if(codetrim.StartsWith("export"))
				{
					string next = Strings.SubString2(codetrim, "export", ";").Trim();
					Exportname = next;
					Ctx.LocalLib.LibName = next;
				}
				else if(codetrim.StartsWith("global"))
				{
					string next = Strings.SubString2(codetrim, "global", ";").Trim();
					Ctx.GlobalVars.Add(next);
				}
				else if(codetrim.StartsWith("const"))
				{
					string next = Strings.SubString2(codetrim, '=', ';').Trim();
					string last = Strings.SubString(codetrim, "const", "=").Trim();
					Ctx.LocalLib.Consts[last] = VBasic.Cast(Ctx.ToValue(next, null));
				}
				else if(codetrim.StartsWith('['))
				{
					string next = Strings.SubString2(codetrim, '[', ']').Trim();
					Ctx.Marks[next] = line - linestart;
				}
				else if(codetrim.StartsWith("function"))
				{
					linestart = line + 1;
					string name = Strings.SubString(codetrim, "function", "(").Trim();
					List<Operation> lines = new List<Operation>();
					Ctx.FuncBodies[name] = lines;

					string[] vars = Strings.SubString(codetrim, '(', ')').Split(',');
					foreach(string v in vars)
					{
						string k = v.Trim();
						Ctx.GetLocalVar(name, k);
					}

					//Add this func first so we can compile recursion!
					Ctx.LocalLib.Functions[name] = (values) =>
					{
						return Globe.Execute(values, lines);
					};
				}
			}

			for(int line = 0; line < codes.Length; line++)
			{
				string code = codes[line];
				string codetrim = code.Trim();

				if(codetrim.StartsWith("function"))
				{
					linestart = line + 1;
					string name = Strings.SubString(codetrim, "function", "(").Trim();
					List<Operation> lines = Ctx.FuncBodies[name];

					while(true)
					{
						line++;
						code = codes[line].Trim();
						if(code.StartsWith("end function"))
						{
							break;
						}
						CompileLine(name, code, line - linestart, lines);
					}
				}
			}
		}

		public void CompileLine(string funcn, string code, int ln, List<Operation> codeout)
		{
			string codetrim = code.Trim();

			Operation cline = new();
			cline.Source = code;
			codeout.Add(cline);

			if(codetrim.StartsWith("if"))
			{
				string next1 = Strings.SubString2(codetrim, '(', ')').Trim();
				IfStack.Push(ln);
				object expr = Ctx.ToValue(next1, funcn);
				cline.Opcode = Opcode.JumpUnless;
				cline.O2 = expr;
			}
			else if(codetrim.StartsWith("end if"))
			{
				int lastif = IfStack.Pop();
				codeout[lastif].AC1 = ln;
				return;
			}

			if(codetrim.StartsWith("while"))
			{
				string next1 = Strings.SubString2(codetrim, '(', ')').Trim();
				WhileStack.Push(ln);
				object expr = Ctx.ToValue(next1, funcn);
				cline.Opcode = Opcode.JumpUnless;
				cline.O2 = expr;
			}
			else if(codetrim.StartsWith("end while"))
			{
				int lastw = WhileStack.Pop();
				codeout[lastw].O1 = ln;
				cline.Opcode = Opcode.JumpIf;
				cline.AC1 = lastw;
				cline.O2 = codeout[lastw].O2;//Condition pass
				return;
			}

			if(codetrim.StartsWith("return "))//with a return value
			{
				string next = Strings.SubString2(codetrim, "return", ";").Trim();
				cline.Opcode = Opcode.Return;
				cline.O1 = Ctx.ToValue(next, funcn);
				return;
			}
			else if(codetrim.StartsWith("return"))
			{
				cline.Opcode = Opcode.Interrupt;
			}

			if(codetrim.StartsWith("jmpf"))
			{
				string next1 = Strings.SubString2(codetrim, '(', ')').Trim();
				string next2 = Strings.SubString2(codetrim, "to", ";").Trim();
				object expr = Ctx.ToValue(next1, funcn);
				cline.Opcode = Opcode.JumpIf;
				cline.AC1 = Ctx.Marks[next2];
				cline.O2 = expr;
			}
			else if(codetrim.StartsWith("jmpu"))
			{
				string next1 = Strings.SubString2(codetrim, '(', ')').Trim();
				string next2 = Strings.SubString2(codetrim, "to", ";").Trim();
				object expr = Ctx.ToValue(next1, funcn);
				cline.Opcode = Opcode.JumpUnless;
				cline.AC1 = Ctx.Marks[next2];
				cline.O2 = expr;
			}
			else if(codetrim.StartsWith("jmp"))
			{
				string next = Strings.SubString2(codetrim, "to", ";").Trim();
				cline.Opcode = Opcode.Jump;
				cline.AC1 = Ctx.Marks[next];
				return;
			}

			if(codetrim.StartsWith("let"))
			{
				string next = Strings.SubString2(codetrim, '=', ';').Trim();
				string last = Strings.SubString(codetrim, "let", "=").Trim();

				if(Ctx.GlobalVars.Contains(last))
				{
					object val0 = Ctx.ToValue(next, funcn);
					cline.Opcode = Opcode.GVarset;
					cline.O1 = last;
					cline.O2 = val0;
				}
				else
				{
					int vid = Ctx.GetLocalVar(funcn, last);
					object val0 = Ctx.ToValue(next, funcn);
					cline.Opcode = Opcode.LVarset;
					cline.AC1 = vid;
					cline.O2 = val0;
				}
			}
			else if(codetrim.StartsWith("inc"))
			{
				string last = Strings.SubString(codetrim, "inc", ";").Trim();

				int vid = Ctx.GetLocalVar(funcn, last);
				cline.Opcode = Opcode.LVarinc;
				cline.AC1 = vid;
			}
			//FUNCTION INVOKE
			else
			{
				VFunc func = Ctx.GetFunc(code);
				if(func != null)
				{
					VParams pars = Ctx.GetParams(code, funcn);
					cline.Opcode = Opcode.Funccall;
					cline.O1 = func;
					cline.O2 = pars;
				}
				//DELEGATE INVOKE
				else
				{
					object o = Ctx.ToValue(code, funcn);
					if(o is VLReferDelegate)
					{
						cline.Opcode = Opcode.FuncLDcall;
					}
					else if(o is VGReferDelegate)
					{
						cline.Opcode = Opcode.FuncGDcall;
					}
					cline.O1 = o;
				}
			}
		}

	}

}
