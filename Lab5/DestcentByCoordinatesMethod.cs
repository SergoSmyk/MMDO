using System;

namespace Lab5
{
    class DestcentByCoordinatesMethod : GradientMethod
    {
        struct TemporaryParams
        {
            public int direction;
            public int currentVectorIndex;
            public double[] oldVectorX;
            public double[] vectorX;
        }

        private TemporaryParams param;

        public DestcentByCoordinatesMethod(Input input) : base(input) { }

        public override GradientMethod.Answer GetAnswer()
        {
            param = new TemporaryParams();
            param.vectorX = input.vectorX0;

            GradientMethod.Answer answer = new GradientMethod.Answer();
            answer.iterCount = 0;
            answer.funcCalcCount = 0;

            double[] stepsArray = new double[input.vectorX0.Length];

            for (int i = 0; i < stepsArray.Length; i++)
            {
                stepsArray[i] = input.epsilon * 100;
            }

            DichotomyMethod.Answer answerDM;
            
            do
            {
                param.oldVectorX = Ext.cloneVector(param.vectorX);

                for (int index = 0; index < param.vectorX.Length; index++)
                {
                    param.currentVectorIndex = index;

                    double varWithNegativeStep = param.vectorX[index] - 3 * input.epsilon;
                    double varWithPositiveStep = param.vectorX[index] + 3 * input.epsilon;

                    double[] copyVector = Ext.cloneVector(param.vectorX);

                    copyVector[index] = varWithNegativeStep;
                    double f1 = input.mainFunc.calculate(copyVector);

                    copyVector[index] = varWithPositiveStep;
                    double f2 = input.mainFunc.calculate(copyVector);
                    param.direction = Math.Sign(f1 - f2);

                    answerDM = findStep(stepsArray[index]);
                    answer.iterCount += answerDM.iterCount;

                    stepsArray[index] = answerDM.approxMinX;
                    param.vectorX = takeAStep(stepsArray[index], param.vectorX, index, param.direction);
                    answer.funcCalcCount += answerDM.funcCalcCount * 2 + 2;
                    answer.approxMinFunc++;
                }
            } while (Ext.normOfVector(Ext.subtructOfVectors(param.vectorX, param.oldVectorX)) >= input.epsilon);

            answer.approxMinFunc = input.mainFunc.calculate(param.vectorX);
            answer.approxMinVectorX = param.vectorX;

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
            double[] newDot = takeAStep(step, param.vectorX, param.currentVectorIndex, param.direction);

            return input.mainFunc.calculate(newDot);
        }

        private double[] takeAStep(double step, double[] vector, int index, int direction)
        {
            double[] vectorWithStep = Ext.cloneVector(vector);          

            vectorWithStep[index] += direction * step;

            return vectorWithStep;
        }
    }
}
