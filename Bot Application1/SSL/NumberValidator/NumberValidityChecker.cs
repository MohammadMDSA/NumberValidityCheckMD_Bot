using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.SSL.NumberValidator
{
	public class NumberValidityChecker
	{
		private string input;
		private int index;
		private NumberState state;
		private bool valid;
		private int automataState;
		private string errorMessage;
		private long integerPart;
		private long decimalPart;
		private long ePart;
		private double value;
		private bool negativeInteger;
		private bool negativeE;

		public NumberValidityChecker(string input)
		{
			this.index = 0;
			this.input = input;
			this.state = NumberState.INTEGER_PART;
			this.valid = false;
			this.automataState = 0;
			this.errorMessage = string.Empty;
			this.decimalPart = 0;
			this.integerPart = 0;
			this.ePart = 0;
			this.value = -1;
			this.negativeInteger = false;
			this.negativeE = false;

			valid = checkString();
			if (valid) value = setValue();
		}

		public NumberValidityChecker() : this(string.Empty)
		{
		}

		public long IntegerPart
		{
			get { return this.integerPart * (negativeInteger ? -1 : 1); }
		}

		public long DecimalPart
		{
			get { return this.decimalPart * (negativeInteger ? -1 : 1); }
		}

		public long EPart
		{
			get { return this.ePart * (negativeE ? -1 : 1); }
		}

		public bool Valid
		{
			get { return this.valid; }
		}

		public double Value
		{
			get { return this.value; }
		}

		private bool checkString()
		{
			char[] inputChars = input.ToCharArray();
			while (input.Length > index)
			{
				char current = inputChars[index];
				int charActionId = getActionNumber(current);
				int nextState = NumberValidityCheckUtil.StateAutomata[automataState, charActionId];
				if (nextState > 0)
				{
					setNewState(current);

					switch (state)
					{
						case NumberState.INTEGER_PART:
							handleNextCharacter(current, charActionId, ref integerPart);
							break;
						case NumberState.DECIMAL_PART:
							handleNextCharacter(current, charActionId, ref decimalPart);
							break;
						case NumberState.E_PART:
							handleNextCharacter(current, charActionId, ref ePart);
							break;
						default:
							break;
					}
				}
				else
				{
					errorMessage = NumberValidityCheckUtil.getErrorMessage(nextState);
					return false;
				}
				index++;
				automataState = nextState;
			}

			return true;
		}
		
		public string gerErrorMessage()
		{
			return this.errorMessage;
		}

		private int getActionNumber(char character)
		{
			switch (character)
			{
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return 0;
				case '0':
					return 1;
				case '.':
					return 2;
				case 'e':
				case 'E':
					return 3;
				case '+':
					return 4;
				case '-':
					return 5;
				default:
					return 6;
			}
		}

		private void setNewState(char character)
		{
			switch (character)
			{
				case '.':
					state = NumberState.DECIMAL_PART;
					break;
				case 'e':
				case 'E':
					state = NumberState.E_PART;
					break;
				default:
					break;
			}
		}

		private void handleNextCharacter(char nextItem, int actionId, ref long current)
		{

			switch (actionId)
			{
				case 0:
					long currentDigit = nextItem - '0';
					current *= 10;
					current += currentDigit;
					break;
				case 1:
					current *= 10;
					break;
				case 5:
					if (state == NumberState.INTEGER_PART)
						negativeInteger = true;
					else if (state == NumberState.E_PART)
						negativeE = true;
					break;

				default:
					break;
			}
		}

		private double setValue()
		{
			double result = integerPart * (negativeInteger ? -1 : 1);
			int decimalLen = 0;
			while (this.decimalPart >= Math.Pow(10, decimalLen)) decimalLen++;
			result += decimalPart * Math.Pow(10, -decimalLen) * (negativeInteger ? -1 : 1);
			result *= Math.Pow(10, ePart * (negativeE ? -1 : 1));
			return result;
		}

	}

	public enum NumberState
	{
		INTEGER_PART,
		DECIMAL_PART,
		E_PART

	}

	public class NumberValidityCheckUtil
	{
		internal static int[,] StateAutomata =
		//	1-9		0		.		E-e		+		-		end
		{
			{2,     3,      -1,     -2,      1,     1,      -4}, ///start
			{2,     3,      -11,    -5,     -6,     -7,     -8}, ///1
			{2,     3,      4,      7,      -9,     -10,    10}, ///2
			{-12,   -13,    4,      7,      -14,    -15,    11}, ///3
			{5,     5,      -16,    -17,    -18,    -19,    -20}, ///4
			{5,     5,      -21,    7,      -22,    -23,    12}, ///5
			{0,		0,		0,		0,		0,		0,		0}, ///empty State
			{9,     9,      -24,    -25,    8,      8,      -26}, ///7
			{9,     9,      -27,    -28,    -29,    -30,    -31}, ///8
			{9,     9,      -32,    -33,    -34,    -35,    13} ///9
		};

		internal static string getErrorMessage(int errorNumber)
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