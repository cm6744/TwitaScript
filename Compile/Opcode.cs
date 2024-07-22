using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TwitaScript.Values;
using Twita.Common;

namespace TwitaScript.Compile
{

	public enum Opcode
	{
		None = 0,

		Jump,
		JumpIf,
		JumpUnless,

		Funccall,
		FuncLDcall,
		FuncGDcall,

		Interrupt,
		Return,

		LVarset,
		LVarinc,
		GVarset,
	}

	public class Operation
	{

		public Opcode Opcode = Opcode.None;
		public object O1, O2;
		public int AC1;
		public string Source;
		
	}

}
