using System;

namespace Lab5
{
    class SegmentOfLocalization
    {

        public struct Input
        {
            public double startX, step, epsilon;
            public delegate double Function(double x);
            public Function func;

            public Input(double startX, double step, double epsilon, Function func)
            {
                this.startX = startX;
                this.step = step;
                this.epsilon = epsilon;
                this.func = func;
            }
        }

        public struct Answer
        {
            public double a, b;
            public double x;

        }

        public static Answer findSegment(in Input input)
        {
            double step = input.step;

            double x0 = 0, x1, x2, x;
            double f0 = input.func(x0), f1, f2, fx;

            for (; ; )
            {
                x = x0 + step;
                fx = input.func(x);

                if (fx <= f0)
                {
                    x1 = x;
                    f1 = fx;
                    break;
                }

                x = x0 - step;
                fx = input.func(x);

                if (fx <= f0)
                {
                    step = -step;
                    x1 = x;
                    f1 = fx;
                    break;
                }

                step /= 2;

                if (Math.Abs(step) < input.epsilon)
                {
                    Answer ans = new Answer();
                    ans.x = x0;
                    return ans;
                }
            }

            for (; ; )
            {
                x2 = x1 + step;
                f2 = input.func(x2);

                if (f2 <= f1)
                {
                    x0 = x1;
                    f0 = f1;
                    x1 = x2;
                    f1 = f2;

                }
                else break;
            }

            Answer output = new Answer();
            output.x = double.MaxValue;

            if (step > 0)
            {
                output.a = x0;
                output.b = x2;
            }
            else
            {
                output.a = x2;
                output.b = x0;
            }

            if (Math.Abs(output.b - output.a) < input.epsilon)
            {
                output.x = (output.b + output.a) / 2;
            }

            return output;
        }
    }
}
