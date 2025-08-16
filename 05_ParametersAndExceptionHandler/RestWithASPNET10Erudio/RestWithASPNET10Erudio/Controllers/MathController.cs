using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Services;
using RestWithASPNET10Erudio.Utils;

namespace RestWithASPNET10Erudio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MathController : ControllerBase
    {
        private readonly MathService _service;
          
        public MathController(MathService service)
        {
            _service = service;
        }

        [HttpGet("sum/{firstNumber}/{secondNumber}")]
        public IActionResult Sum(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber)) {
                var sum = _service.Sum(
                    NumberHelper.ConvertToDecimal(firstNumber),
                    NumberHelper.ConvertToDecimal(secondNumber)
                );
                return Ok(sum);
            }
            return BadRequest("Invalid Input!");
        }

        [HttpGet("subtraction/{firstNumber}/{secondNumber}")]
        public IActionResult Subtraction(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber)) {

                var subtraction = _service.Subtraction(
                    NumberHelper.ConvertToDecimal(firstNumber),
                    NumberHelper.ConvertToDecimal(secondNumber)
                );

                return Ok(subtraction);
            }
            return BadRequest("Invalid Input!");
        }

        [HttpGet("multiplication/{firstNumber}/{secondNumber}")]
        public IActionResult Multiplication(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber)) {
                var multiplication = _service.Multiplication(
                    NumberHelper.ConvertToDecimal(firstNumber),
                    NumberHelper.ConvertToDecimal(secondNumber)
                );
                return Ok(multiplication);
            }
            return BadRequest("Invalid Input!");
        }

        [HttpGet("division/{firstNumber}/{secondNumber}")]
        public IActionResult Division(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber)) {
                var division = _service.Division(
                    NumberHelper.ConvertToDecimal(firstNumber),
                    NumberHelper.ConvertToDecimal(secondNumber)
                );
                return Ok(division);
            }
            return BadRequest("Invalid Input!");
        }

        [HttpGet("square-root/{number}")]
        public IActionResult SquareRoot(string number)
        {
            if (NumberHelper.IsNumeric(number))
            {
                var sqrt = _service.SquareRoot(
                    NumberHelper.ConvertToDecimal(number)
                );
                return Ok(sqrt);
            }
            return BadRequest("Invalid Input!");
        }


        [HttpGet("mean/{firstNumber}/{secondNumber}")]
        public IActionResult Mean(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber))
            {
                var sum = _service.Mean(
                    NumberHelper.ConvertToDecimal(firstNumber),
                    NumberHelper.ConvertToDecimal(secondNumber)
                );
                return Ok(sum);
            }
            return BadRequest("Invalid Input!");
        }

       
    }
}
