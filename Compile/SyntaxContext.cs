using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitaScript.Library;
using TwitaScript.Values;
using Twita.Common.Toolkit;

namespace TwitaScript.Compile
{

	public class SyntaxContext
	{

		public readonly Lib											LocalLib = new();
		public readonly List<string>								Usings = new();
		public readonly Dictionary<string, Dictionary<string, int>>	FunctionalVarlist = new();
		public readonly Dictionary<string, int>						Marks = new();
		public readonly Dictionary<string, int>						VarIncreaser = new();
		public readonly Dictionary<string, List<Operation>>			FuncBodies = new();
		public readonly HashSet<string>								GlobalVars = new();

		public int GetLocalVar(string func, string code)
		{
			if(string.IsNullOrEmpty(code)) return -1;
			if(!FunctionalVarlist.ContainsKey(func)) FunctionalVarlist[func] = new();
			if(!VarIncreaser.ContainsKey(func)) VarIncreaser[func] = 0;
			if(FunctionalVarlist[func].TryGetValue(code, out var i))
			{
				return i;
			}
			int l = FunctionalVarlist[func][code] = VarIncreaser[func];
			VarIncreaser[func]++;
			return l;
		}

		public VParams GetParams(string code, string funcn)
		{
			List<object> values = new();
			bool instr = false;

			//The bracket depth,
			//Meet a left bracket and increase.
			int layers = 0;
			int lastInd = -1;
			for(int i = 0; i < code.Length; i++)
			{
				char c = code[i];
				if(c == '\"')
				{
					instr = !instr;
				}
				if(c == '(')
				{
					if(layers == 0)
					{
						if(lastInd == -1) lastInd = i;
						else
						{
							object o = ToValue(Strings.SubString(code, lastInd, i), funcn);
							if(o != VBasic.Undefined) values.Add(o);
							lastInd = -1;
						}
					}
					layers++;
				}
				else if(c == ')')
				{
					layers--;
					if(layers == 0)
					{
						if(lastInd == -1) lastInd = i;
						else
						{
							object o = ToValue(Strings.SubString(code, lastInd, i), funcn);
							if(o != VBasic.Undefined) values.Add(o);
							lastInd = -1;
						}
					}
				}
				if(layers == 1 && c == ',' && !instr)
				{
					if(lastInd != -1)
					{
						object o = ToValue(Strings.SubString(code, lastInd, i), funcn);
						if(o != VBasic.Undefined) values.Add(o);
					}
					lastInd = i;
				}
			}
			return new VParams(values);
		}

		public VExpr GetExpr(string code, string funcn)
		{
			VParams ps = GetParams(code, funcn);

			OpExpr opr = null;
			if(code.StartsWith('~')) opr = OpExpr.OR;
			else if(code.StartsWith('&')) opr = OpExpr.AND;

			return new VExpr(ps.Prims[0], ps.Prims[1], opr);//do not get the values
		}

		public VExpr GetExprTiny(string code, string funcn)
		{
			foreach(var kv in OpExpr.Map)
			{
				string key = kv.Key;
				if(code.Contains(key))
				{
					string[] arr = Strings.CutBy(code, key);
					return new VExpr(ToValue(arr[0], funcn), ToValue(arr[1], funcn), kv.Value);
				}
			}
			return null;
		}

		public object ToValue(string code, string funcn)
		{
			code = code.Trim();//Bug fix.

			//BASIC VALUES
			if(string.IsNullOrEmpty(code))
			{
				return VBasic.Undefined;
			}
			if(code == "null")
			{
				return null;
			}
			if(bool.TryParse(code, out bool r4))
			{
				return r4;
			}
			if(int.TryParse(code, out int r3))
			{
				return r3;
			}
			if(float.TryParse(code, out float r5))
			{
				return r5;
			}

			if(code.Contains('+') || code.Contains('-') || code.Contains('*') || code.Contains('/'))
			{
				return GetLinkedOp(code, funcn);
			}

			//EXPR
			if(code.StartsWith('&') || code.StartsWith('~'))
			{
				return GetExpr(code, funcn);
			}
			VExpr expr = GetExprTiny(code, funcn);
			if(expr != null)
			{
				return expr;
			}

			//ENUM & STRING
			if(code.StartsWith('"'))
			{
				return Strings.SubString2(code, '"', '"');//Literal value
			}
			if(code.StartsWith('%'))
			{
				return code.Replace("%", "").Trim();//Enum value
			}

			//CONST
			object o = GetConst(code);
			if(o != null)
			{
				return o;
			}

			//FUNCTION REF
			VFunc func = GetFunc(code);
			if(func != null)
			{
				if(code.Contains('('))
				{
					VParams pars = GetParams(code, funcn);
					return new VReferFunc(func, pars);
				}
				return new VDelegate(func);
			}

			//GLOBAL VARIABLE
			if(GlobalVars.Contains(code))
			{
				if(code.Contains('('))
				{
					VParams pars = GetParams(code, funcn);
					return new VGReferDelegate(code, pars);
				}
				return new VGRefer(code);
			}

			//LOCAL VARIABLE
			if(code.Contains('('))
			{
				string refvar = Strings.LastString(code, '(');
				VParams pars = GetParams(code, funcn);
				return new VLReferDelegate(GetLocalVar(funcn, refvar), pars);
			}
			return new VLRefer(GetLocalVar(funcn, code));
		}

		public object GetConst(string code)
		{
			var f = Globe.GlobalLib.SearchConsts(Usings, code);
			return f == null ? LocalLib.TryConst(code) : f;
		}

		public VFunc GetFunc(string code)
		{
			string last = code;
			if(code.Contains('('))
			{
				last = Strings.LastString(code, '(').Trim();
			}
			var f = Globe.GlobalLib.SearchAvailables(Usings, last);
			return f == null ? LocalLib.Search(last) : f;
		}

		static char[] M = new char[] { '+', '-', '*', '/' };

		public VLinkedOp GetLinkedOp(string code, string funcn)
		{
			StringBuilder sb = new();
			List<object> lst = new();

			foreach(char ch in code)
			{
				if(M.Contains(ch))
				{
					lst.Add(ToValue(sb.ToString(), funcn));
					lst.Add(ch);
					sb.Clear();
					continue;
				}
				sb.Append(ch);
			}

			lst.Add(ToValue(sb.ToString(), funcn));

			return new VLinkedOp(lst);
		}

	}

}
