namespace RestWithASPNET10Erudio.Services
{
    public class MathService
    {
        public decimal Sum(decimal firstNumber, decimal secondNumber) => firstNumber + secondNumber;
        public decimal Mean(decimal firstNumber, decimal secondNumber) => (firstNumber + secondNumber) / 2;
        public decimal Subtraction(decimal firstNumber, decimal secondNumber) => firstNumber - secondNumber;
        public decimal Multiplication(decimal firstNumber, decimal secondNumber) => firstNumber * secondNumber;
        public decimal Division(decimal firstNumber, decimal secondNumber) {
            if (secondNumber == 0) throw new DivideByZeroException("Division by zero is not allowed.");
            return firstNumber / secondNumber;
        }

        public double SquareRoot(decimal number) {
            if (number < 0) throw new ArgumentOutOfRangeException(
                "Cannot calculate the square root of a negative number.");
            return Math.Sqrt((double)number);
        }
    }
}
