namespace Lab5
{
    class DichotomyMethod
    {

        public struct Input
        {
            public double a, b, delta, epsilon;
            public delegate double Function(double x);
            public Function func;

            public Input(double a, double b, double delta, double epsilon, Function func)
            {
                this.a = a;
                this.b = b;
                this.delta = delta;
                this.epsilon = epsilon;
                this.func = func;
            }
        }

        public struct Answer
        {
            public int funcCalcCount, iterCount;
            public double approxMinX, approxMinFunc;
        }

        public static Answer calculate(in Input input)
        {
            Answer answer = new Answer();
            answer.iterCount = 0;

            double leftBorder = input.a, rightBorder = input.b;
            double x1, x2, f1, f2, tempSum;

            do
            {
                tempSum = leftBorder + rightBorder;

                x1 = (tempSum - input.delta) / 2;
                x2 = (tempSum + input.delta) / 2;

                f1 = input.func(x1);
                f2 = input.func(x2);

                if (f1 <= f2)
                {
                    rightBorder = x2;
                    answer.approxMinX = leftBorder;
                    answer.approxMinFunc = f1;
                }
                else
                {
                    leftBorder = x1;
                    answer.approxMinX = rightBorder;
                    answer.approxMinFunc = f2;
                }
                answer.iterCount++;
            } while (rightBorder - leftBorder >= input.epsilon);

            answer.funcCalcCount = answer.iterCount * 2;
            return answer;
        }
    }
}
