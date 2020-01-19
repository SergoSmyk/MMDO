using System;

namespace Lab5
{
    class Data
    {
        private string[] filteredLines;
        public Data(string filePath)
        {
            string[] fileLines = System.IO.File.ReadAllLines(filePath);
            filteredLines = new string[fileLines.Length];

            for (int i = 0, j = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].StartsWith("//"))
                {
                    continue;
                }
                filteredLines[j] = fileLines[i];
                j++;
            }
        }

        public int ReadInputWithoutSecondOrderDerivatives(ref GradientMethod.Input input)
        {
            int index = 0;

            input.epsilon = readEpsilon(ref index);

            int variablesCount = readVaribleCount(ref index);
            string[] variables = readVariables(ref index, variablesCount);
            string mainFunc = readFunctionName(ref index);

            input.mainFunc = new Func(mainFunc, "M", variables);
            input.firstOrderDerivatives = readFirstOrderDerivatives(ref index, variables);
            input.vectorX0 = readStartVector(ref index, variablesCount);

            return index;
        }

        public void ReadFullInput(ref GradientMethod.Input input)
        {
            int index = 0;

            input.epsilon = readEpsilon(ref index);

            int variablesCount = readVaribleCount(ref index);
            string[] variables = readVariables(ref index, variablesCount);
            string mainFunc = readFunctionName(ref index);

            input.mainFunc = new Func(mainFunc, "M", variables);
            input.firstOrderDerivatives = readFirstOrderDerivatives(ref index, variables);
            input.vectorX0 = readStartVector(ref index, variablesCount);
            input.secondOrderDerivatives = readSecondOrderDerivatives(ref index, variables);
        }

        private double readEpsilon(ref int index)
        {
            double epsilon = Math.Pow(10, Int32.Parse(filteredLines[index]));
            index++;

            return epsilon;
        }

        private int readVaribleCount(ref int index)
        {
            int variablesCount = Int32.Parse(filteredLines[index]);
            index++;

            return variablesCount;
        }

        private string[] readVariables(ref int index, int variablesCount)
        {
            string[] varsNames = new string[variablesCount];

            for (int i = 0; i < variablesCount; i++)
            {
                varsNames[i] = filteredLines[index];
                index++;
            }

            return varsNames;
        }

        private string readFunctionName(ref int index)
        {
            string func = filteredLines[index];
            index++;
            return func;
        }

        private Func[] readFirstOrderDerivatives(ref int index, string[] variables)
        {
            Func[] derivatives = new Func[variables.Length];

            for (int i = 0; i < variables.Length; i++)
            {
                string firstOrderDerivative = filteredLines[index];
                derivatives[i] = new Func(firstOrderDerivative, $"F{i}", variables);
                index++;
            }

            return derivatives; 
        }

        private double[] readStartVector(ref int index, int variablesCount)
        {
            double[] vectorX0 = new double[variablesCount];

            for (int i = 0; i < variablesCount; i++)
            {
                vectorX0[i] = Double.Parse(filteredLines[index]);
                index++;
            }
            return vectorX0;
        }

        private Func[,] readSecondOrderDerivatives(ref int index, string[] variables)
        {
            Func[,] derivatives = new Func[variables.Length, variables.Length];

            for (int i = 0; i < variables.Length; i++)
            {
                for (int j = 0; j < variables.Length; j++)
                {
                    string secondOrderDerivative = filteredLines[index];
                    derivatives[i, j] = new Func(secondOrderDerivative, $"S{i}{j}", variables);
                    index++;
                }
            }

            return derivatives;
        }
    }
}
