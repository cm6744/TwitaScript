using Twita.Codec;
using TwitaScript.Compile;

namespace TwitaScript.Values
{

	public class VExpr : VBasic
	{

		public object Left, Right;
		public OpExpr Op;

		public object Fixed => Op.Calc(VBasic.Cast(Left), VBasic.Cast(Right));

		public VExpr(object l, object r, OpExpr op)
		{
			Left = l;
			Right = r;
			Op = op;
		}

		public VExpr GetElse()
		{
			OpExpr opo = null;
			if(Op == OpExpr.OR)
			{
				opo = OpExpr.AND;
			}
			else if(Op == OpExpr.AND)
			{
				opo = OpExpr.OR;
			}
			else if(Op == OpExpr.EQ)
			{
				opo = OpExpr.NE;
			}
			else if(Op == OpExpr.NE)
			{
				opo = OpExpr.EQ;
			}
			else if(Op == OpExpr.EQ_LG)
			{
				opo = OpExpr.RG;
			}
			else if(Op == OpExpr.EQ_RG)
			{
				opo = OpExpr.LG;
			}
			else if(Op == OpExpr.LG)
			{
				opo = OpExpr.EQ_RG;
			}
			else if(Op == OpExpr.RG)
			{
				opo = OpExpr.EQ_LG;
			}
			return new VExpr(Left, Right, opo);
		}

	}

}
