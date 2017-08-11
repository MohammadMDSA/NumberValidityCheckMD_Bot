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
			{2,     3,      -1,     -2,      1,     1,      -4}, ///start
			{2,     3,      -11,    -5,     -6,     -7,     -8}, ///1
			{2,     3,      4,      7,      -9,     -10,    10}, ///2
			{-12,   -13,    4,      7,      -14,    -15,    11}, ///3
			{5,     5,      -16,    -17,    -18,    -19,    -20}, ///4
			{5,     5,      -21,    7,      -22,    -23,    12}, ///5
			{9,     9,      -24,    -25,    8,      8,      -26}, ///6
			{9,     9,      -27,    -28,    -29,    -30,    -31}, ///7
			{9,     9,      -32,    -33,    -34,    -35,    13} ///8
		};

		private static string getErrorMessage(int errorNumber)
		{
			switch (errorNumber)
			{
				case -1:
					return "A number cannot start with \'.\'";
				case -2:
					return "A number cannot start with \'e\' or \'E\'";
				//case -3:
				//	return "";
				case -4:
					return "A number must contain digits";
				case -5:
					return "\'E\' or \'e\' cannot appear after \'-\'";
				case -6:
					return "\'+\' cannot appear after \'-\'";
				case -7:
					return "\'-\' cannot appear after \'-\'";
				case -8:
					return "A number cannot end with \'-\'";
				case -9:
					return "\'+\' cannot appear after a digit in the middle of a number";
				case -10:
					return "\'-\' cannot appear after a digit in the middle of a number";
				case -11:
					return "\'.\' cannot appear after \'-\'";
				case -12:
					return "A number cannot start with \'0\' with digits following";
				case -13:
					return "A number cannot start with \'0\' with \'0\' following";
				case -14:
					return "A number cannot start with \'0\' with \'+\' following";
				case -15:
					return "A number cannot start with \'0\' with \'-\' following";
				case -16:
					return "\'.\' cannot appear after \'.\'";
				case -17:
					return "\'E\' or \'e\' cannot appear after \'.\'";
				case -18:
					return "\'+\' cannot appear after \'.\'";
				case -19:
					return "\'-\' cannot appear after \'.\'";
				case -20:
					return "A number cannot end with \'.\'";
				case -21:
					return "A number cannot contain more than one \'.\'";
				case -22:
					return "\'+\' cannot appear in the middle of a number";
				case -23:
					return "\'-\' cannot appear in the middle of a number";
				case -24:
					return "\'.\' cannot appear after \'E\' or \'e\'";
				case -25:
					return "A number cannot contain more than one \'E\' or \'e\'";
				case -26:
					return "A number cannot end with \'E\' or \'r\'";
				case -27:
					return "\'.\' cannot appear after \'E\' or \'e\'";
				case -28:
					return "A number cannot contain more than one \'E\' or \'e\'";
				case -29:
					return "\'+\' cannot appear after \'E\' or \'e\'";
				case -30:
					return "\'-\' cannot appear after \'E\' or \'e\'";
				case -31:
					return "A number cannot end with \'+\' or \'-\'";
				case -32:
					return "\'.\' cannot appear after \'E\' or \'e\'";
				case -33:
					return "A number cannot contain more than one \'E\' or \'e\'";
				case -34:
					return "\'+\' cannot appear after \'E\' or \'e\' in a number";
				case -35:
					return "\'-\' cannot appear after \'E\' or \'e\' in a number";
				default:
					return "";
			}
		}
	}
}