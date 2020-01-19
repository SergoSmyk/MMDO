using System;

namespace Lab5
{
    class ModifiedNutonMethod : GradientMethod
    {
        struct TemporaryParams
        {
            public double[] vectorX;
            public double[] gradient;
            public double[] firstOrderGradient;
            public double[,] invertedSecondOrderGradient;           
        }

        private TemporaryParams param;

        public ModifiedNutonMethod(Input input) : base(input) { }

        public override GradientMethod.Answer GetAnswer()
        {
            param = new TemporaryParams();

            GradientMethod.Answer answer = new GradientMethod.Answer();
            answer.iterCount = 0;
            answer.funcCalcCount = 0;
            param.vectorX = Ext.cloneVector(input.vectorX0);

            param.firstOrderGradient = Ext.findFirstOrderGradient(input.firstOrderDerivatives, input.vectorX0);

            double[,] secondOrderGradient = Ext.findSecondOrderGradient(input.secondOrderDerivatives, param.vectorX);
            param.invertedSecondOrderGradient = Ext.findInversedMatrix(secondOrderGradient); ;

            answer.funcCalcCount += input.vectorX0.Length + Convert.ToInt32(Math.Pow(param.vectorX.Length, 2));

            double step = 1000 * input.epsilon;
          
            param.vectorX = Ext.cloneVector(input.vectorX0);

            DichotomyMethod.Answer answerDM;

            if (Ext.normOfVector(param.firstOrderGradient) > input.epsilon)
            {
                do
                {                   
                    answerDM = findStep(step);
                    step = answerDM.approxMinX;
                    answer.iterCount += answerDM.iterCount;

                    param.vectorX = takeAStep(step, param.vectorX, param.invertedSecondOrderGradient, param.firstOrderGradient);
                    param.firstOrderGradient = Ext.findFirstOrderGradient(input.firstOrderDerivatives, param.vectorX);

                    answer.funcCalcCount += input.vectorX0.Length;
                    answer.iterCount++;
                }
                while (Ext.normOfVector(param.firstOrderGradient) >= input.epsilon);
            }

            answer.approxMinVectorX = param.vectorX;
            answer.approxMinFunc = input.mainFunc.calculate(param.vectorX);
            answer.funcCalcCount++;

            return answer;
        }

        private DichotomyMethod.Answer findStep(double oldStep)
        {
            SegmentOfLocalization.Input inputSOL = new SegmentOfLocalization.Input(0, oldStep, input.epsilon, calculateStep);
            SegmentOfLocalization.Answer answerSOL = SegmentOfLocalization.findSegment(inputSOL);
            if (answerSOL.x != Double.MaxValue)
            {
                DichotomyMethod.Answer ans = new DichotomyMethod.Answer();
                ans.approxMinX = answerSOL.x;
                ans.approxMinFunc = calculateStep(answerSOL.x);
                ans.funcCalcCount = 0;
                ans.iterCount = 0;
                return ans;
            }
            Console.WriteLine($"Old Step : {oldStep}");
            DichotomyMethod.Input inputDM = new DichotomyMethod.Input(answerSOL.a, answerSOL.b, input.epsilon / 3, input.epsilon, calculateStep);
            return DichotomyMethod.calculate(inputDM);
        }

        private double calculateStep(double step)
        {
            double[] newDot = takeAStep(step, param.vectorX, param.invertedSecondOrderGradient, param.firstOrderGradient);

            return input.mainFunc.calculate(newDot);
        }

        private double[] takeAStep(double step, double[] vector, double[,] invertedSecondOrderGradient, double[] firstOrderGradient)
        {
            double[] vectorWithStep = new double[vector.Length];

            double[] sums = new double[input.vectorX0.Length];

            for (int i = 0; i < param.vectorX.Length; i++)
            {
                sums[i] = 0;
                for (int j = 0; j < param.vectorX.Length; j++)
                {
                    sums[i] += invertedSecondOrderGradient[i, j] * firstOrderGradient[j];
                }

            }

            for (int i = 0; i < vector.Length; i++)
            {
                vectorWithStep[i] = vector[i] - step * sums[i];
            }

            return vectorWithStep;
        }
    }
}
