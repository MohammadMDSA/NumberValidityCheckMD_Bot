using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.SSL.NumberValidator
{
	public class NumberValidator
	{

		private static int[,] StateAutomata =
		//	1-9		0		.		E-e		+		-		end
		{
			{2,     3,      -1,     -2,     -3,     1,      -4}, ///start
			{2,     3,      -11,    -5,     -6,     -7,     -8}, ///1
			{2,     3,      4,      7,      -9,     -10,    10}, ///2
			{-12,	-13,	4,		7,		-14,	-15,	11}, ///3
			{5,		5,		-16,	-17,	-18,	-19,	-20}, ///4
			{5,		5,		-21,	7,		-22,	-23,	12}, ///5
			{9,     9,      -24,    -25,    8,      8,      -26}, ///6
			{9,     9,      -27,    -28,    -29,    -30,    -31}, ///7
			{9,		9,		-32,	-33,	-34,	-35,	13}
		};


	}
}