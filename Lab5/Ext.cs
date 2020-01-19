using System;

namespace Lab5
{
    class Ext
    {
        public static double[] findFirstOrderGradient(Func[] derivatives, double[] vector)
        {
            if (vector.Length != derivatives.Length)
            {
                throw new Exception("Unable to find a gradient, the number of derivatives does not equal the number of variables");
            }

            double[] answer = new double[vector.Length];

            for (int i = 0; i < vector.Length; i++)
            {
                answer[i] = derivatives[i].calculate(vector);
            }

            return answer;
        }

        public static double[,] findSecondOrderGradient(Func[,] derivatives, double[] vector)
        {
            if (vector.Length != Math.Sqrt(derivatives.Length))
            {
                throw new Exception("Unable to find a gradient, the number of derivatives does not equal the number of variables");
            }

            double[,] answer = new double[vector.Length, vector.Length];

            for (int i = 0; i < vector.Length; i++)
            {
                for (int j = 0; j < vector.Length; j++)
                {
                    answer[i, j] = derivatives[i, j].calculate(vector);
                }
            }

            return answer;
        }

        public static double normOfVector(double[] vector)
        {
            double s = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                s += Math.Pow(vector[i], 2);
            }
            return Math.Sqrt(s);
        }

        public static double[] cloneVector(double[] vector)
        {
            double[] clonedVector = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                clonedVector[i] = vector[i];
            }
            return clonedVector;
        }

        public static double[] subtructOfVectors(double[] vectorA, double[] vectorB)
        {
            double[] result = new double[vectorA.Length];

            for (int i = 0; i < vectorA.Length; i++)
            {
                result[i] = vectorA[i] - vectorB[i];
            }

            return result;
        }

        public static double[,] findInversedMatrix(double[,] inp)
        {
            int len = Convert.ToInt32(Math.Sqrt(inp.Length));
            double[,] input = copy2DArray(inp);
            double[,] res = new double[len, len];
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    res[i, j] = (i == j) ? 1 : 0;
                }
            }
            double dive;
            for (int i = 0; i < len - 1; i++)
            {
                dive = input[i, i];
                for (int j = 0; j < len; j++)
                {
                    res[i, j] /= dive;
                    input[i, j] /= dive;
                }
                for (int j = i + 1; j < len; j++)
                {
                    dive = input[j, i];
                    for (int k = 0; k < len; k++)
                    {
                        res[j, k] -= res[i, k] * dive;
                        input[j, k] -= input[i, k] * dive;
                    }
                }
            }

            dive = input[len - 1, len - 1];
            for (int j = 0; j < len; j++)
            {
                res[len - 1, j] /= dive;
                input[len - 1, j] /= dive;
            }
            for (int i = len - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    dive = input[j, i];
                    for (int k = 0; k < len; k++)
                    {
                        res[j, k] -= res[i, k] * dive;
                        input[j, k] -= input[i, k] * dive;
                    }
                }
            }
            return res;
        }

        public static double[,] copy2DArray(double[,] arr)
        {
            int len = Convert.ToInt32(Math.Sqrt(arr.Length));
            double[,] input = new double[len, len];
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    input[i, j] = arr[i, j];
                }
            }
            return input;
        }
    }
}
