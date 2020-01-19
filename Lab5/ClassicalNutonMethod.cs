using System;

namespace Lab5
{
    class ClassicalNutonMethod : GradientMethod
    {
        struct TemporaryParams
        {
            public double[] vectorX;          
        }

        private TemporaryParams param;

        public ClassicalNutonMethod(Input input) : base(input) { }

        public override GradientMethod.Answer GetAnswer()
        {
            param = new TemporaryParams();

            GradientMethod.Answer answer = new GradientMethod.Answer();
            answer.iterCount = 0;
            answer.funcCalcCount = 0;

            double[] firstOrderGradient = Ext.findFirstOrderGradient(input.firstOrderDerivatives, input.vectorX0);
            double[,] secondOrderGradient, invertedSecondOrderGradient;
            answer.funcCalcCount += input.vectorX0.Length;

            double[] sums = new double[input.vectorX0.Length];
            param.vectorX = Ext.cloneVector(input.vectorX0);

            if (Ext.normOfVector(firstOrderGradient) > input.epsilon)
            {
                do
                {
                    secondOrderGradient = Ext.findSecondOrderGradient(input.secondOrderDerivatives, param.vectorX);
                    answer.funcCalcCount += Convert.ToInt32(Math.Pow(param.vectorX.Length, 2));
                    invertedSecondOrderGradient = Ext.findInversedMatrix(secondOrderGradient);

                    for (int i = 0; i < param.vectorX.Length; i++)
                    {
                        sums[i] = 0;
                        for (int j = 0; j < param.vectorX.Length; j++)
                        {
                            sums[i] += invertedSecondOrderGradient[i, j] * firstOrderGradient[j];
                        }

                    }
                    param.vectorX = Ext.subtructOfVectors(param.vectorX, sums);
                    firstOrderGradient = Ext.findFirstOrderGradient(input.firstOrderDerivatives, param.vectorX);
                    answer.funcCalcCount += input.vectorX0.Length;
                    answer.iterCount++;
                }
                while (Ext.normOfVector(firstOrderGradient) >= input.epsilon);
            }

            answer.approxMinVectorX = param.vectorX;
            answer.approxMinFunc = input.mainFunc.calculate(param.vectorX);
            answer.funcCalcCount++;

            return answer;
        }
    }
}
