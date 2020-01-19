namespace Lab5
{
    public abstract class GradientMethod
    {
        public struct Input
        {
            public double epsilon;
            public double[] vectorX0;
            public Func mainFunc;
            public Func[] firstOrderDerivatives;
            public Func[,] secondOrderDerivatives;

            public Input(double epsilon, double[] vectorX0, Func mainFunc, Func[] firstOrderDerivatives)
            {
                this.epsilon = epsilon;
                this.vectorX0 = vectorX0;
                this.mainFunc = mainFunc;
                this.firstOrderDerivatives = firstOrderDerivatives;
                this.secondOrderDerivatives = new Func[0, 0] { };
            }

            public Input(double epsilon, double[] vectorX0, Func mainFunc, Func[] firstOrderDerivatives, Func[,] secondOrderDerivatives)
                : this(epsilon, vectorX0, mainFunc, firstOrderDerivatives)
            {
                this.secondOrderDerivatives = secondOrderDerivatives;
            }
        }
        public struct Answer
        {
            public int funcCalcCount, iterCount;
            public double approxMinFunc;
            public double[] approxMinVectorX;
        }

        protected Input input;

        public GradientMethod(Input input)
        {
            this.input = input;
        }

        public abstract Answer GetAnswer();
    }
}
