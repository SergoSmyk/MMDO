using org.mariuszgromada.math.mxparser;
using System;

namespace Lab5
{
	public class Func
	{
		private Function func;
		private string funcName;
		private string[] variables;

		public string Name
		{
			get { return funcName; }
		}

		public int VariablesCount
		{
			get { return variables.Length; }
		}

		public Func(string functionBody, string name, params string[] variables)
		{
			this.funcName = buildFuncName(variables, name);
			this.variables = variables;
			this.func = new Function($"{funcName} = {functionBody}");
		}

		private string buildFuncName(string[] variables, string name)
		{
			if (variables.Length == 0)
			{
				throw new Exception("You haven`t variables");
			}

			string vars = "";

			for (int i = 0; i < variables.Length - 1; i++)
			{
				string var = variables[i];
				if (vars.Contains(var) || var.Equals(name))
				{
					throw new Exception("You cannot have dublicate of variables or variable which equal function name");
				}

				vars += var + ",";
			}

			vars += variables[variables.Length - 1];

			return $"{name}({vars})";
		}

		public double calculate(params double[] vector)
		{
			if (vector.Length != variables.Length)
			{
				throw new Exception("Vector size not equal variables count");
			}

			string dot = funcName.Replace(" ", " ");

			for (int i = 0; i < variables.Length; i++)
			{
				dot = dot.Replace(variables[i], $"{vector[i]}");
			}

			return new Expression(dot, func).calculate();
		}
	}
}
