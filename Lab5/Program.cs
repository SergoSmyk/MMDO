using System;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {

            execute();

        }

        static void execute()
        {
            Data func1Data = new Data(@"C:\Users\Sergey\Desktop\Lab5\1_func.txt");
            GradientMethod.Input input = new GradientMethod.Input();
            func1Data.ReadInputWithoutSecondOrderDerivatives(ref input);

            GradientMethod.Answer answer;

            answer = new QuickestDestcent(input).GetAnswer();
            showAnswer("Quickest Destcent method", answer);

            answer = new DestcentByCoordinatesMethod(input).GetAnswer();
            showAnswer("Destcent By Coordinates Method", answer);

            Data func2Data = new Data(@"C:\Users\Sergey\Desktop\Lab5\rosenbrok.txt");
            func2Data.ReadFullInput(ref input);

            answer = new ClassicalNutonMethod(input).GetAnswer();
            showAnswer("Classical Nuton method", answer);

            answer = new ModifiedNutonMethod(input).GetAnswer();
            showAnswer("Modified Nuton method", answer);
        }

        private static void showAnswer(string methodName, GradientMethod.Answer answer)
        {
            Console.WriteLine($"Answer for {methodName}");
            Console.WriteLine($"    Approximate minimum of function: {answer.approxMinFunc}");
            string dot = "";
            for (int i = 0; i < answer.approxMinVectorX.Length - 1; i++)
            {
                dot += answer.approxMinVectorX[i] + ",";
            }
            dot += answer.approxMinVectorX[answer.approxMinVectorX.Length - 1];

            Console.WriteLine($"    Dot : x({dot})");
            Console.WriteLine($"    Functionc calculation count : {answer.funcCalcCount}");
            Console.WriteLine($"    Iteration count : {answer.iterCount}\n");
        }
    }
}
