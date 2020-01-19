using System;

namespace Lab5
{
    class QuickestDestcent: GradientMethod
    {        
        struct TemporaryParams
        {
            public double[] oldVectorX;
            public double[] vectorX;
            public double[] gradient;
        }

        private TemporaryParams param;

        public QuickestDestcent(Input input) : base(input) { }   

        public override GradientMethod.Answer GetAnswer()
        {
            param = new TemporaryParams();

            GradientMethod.Answer answer = new GradientMethod.Answer();
            answer.iterCount = 0;
            answer.funcCalcCount = 0;

            param.vectorX = Ext.cloneVector(input.vectorX0);
            param.gradient = Ext.findFirstOrderGradient(input.firstOrderDerivatives, param.vectorX);

            answer.funcCalcCount += input.vectorX0.Length;

            if (Ext.normOfVector(param.gradient) > input.epsilon)
            {
                double step = 100 * input.epsilon;
                DichotomyMethod.Answer answerDM;
                do
                {
                    answerDM = findStep(step);
                    step = answerDM.approxMinX;

                    answer.funcCalcCount += answerDM.funcCalcCount;
                    answer.iterCount += answerDM.iterCount;                   

                    param.oldVectorX = param.vectorX;
                    param.vectorX = takeAStep(step, param.vectorX, param.gradient);
                    param.gradient = Ext.findFirstOrderGradient(input.firstOrderDerivatives, param.vectorX);

                    answer.funcCalcCount += input.vectorX0.Length;
                    answer.iterCount++;
                }
                while (Ext.normOfVector(Ext.subtructOfVectors(param.vectorX, param.oldVectorX)) >= input.epsilon
                && Ext.normOfVector(param.gradient) >= input.epsilon);
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

            DichotomyMethod.Input inputDM = new DichotomyMethod.Input(answerSOL.a, answerSOL.b, input.epsilon / 3, input.epsilon, calculateStep);
            return DichotomyMethod.calculate(inputDM);
        }

        private double calculateStep(double step)
        {
            double[] newDot = takeAStep(step, param.vectorX, param.gradient);

            double funcResult = input.mainFunc.calculate(newDot);
            return funcResult;
        }

        private double[] takeAStep(double step, double[] vector, double[] gradient)
        {
            double[] vectorWithStep = new double[vector.Length];

            for (int i = 0; i < vector.Length; i++)
            {
                vectorWithStep[i] = vector[i] - step * gradient[i];
            }

            return vectorWithStep;
        }
    }
}
