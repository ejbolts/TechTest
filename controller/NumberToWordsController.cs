using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace NumberToWordsApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class NumberToWordsController(IWebHostEnvironment environment) : ControllerBase
    {
        private readonly IWebHostEnvironment _environment = environment ?? throw new ArgumentNullException(nameof(environment));

        [HttpGet("/")]
        public IActionResult GetHtml()
        {
            var filePath = Path.Combine(_environment.WebRootPath, "index.html");
            if (System.IO.File.Exists(filePath))
            {
                return PhysicalFile(filePath, "text/html");
            }
            return NotFound("index.html not found.");
        }

        [HttpGet("convert")]
        public IActionResult Convert(string number)
        {
            if (decimal.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal numericValue))
            {
                // Check if the numeric value exceeds the range of a long
                if (numericValue > long.MaxValue || numericValue < long.MinValue)
                    return BadRequest(new { error = "Number is out of range. Please enter a number between the max and min number of a 64int" });

                try
                {
                    string result = ConvertToWords((long)numericValue);
                    return Ok(new { words = result });
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return BadRequest(new { error = ex.Message });
                }
            }
            else
            {
                return BadRequest(new { error = "Invalid number format" });
            }
        }

        private static string ConvertToWords(decimal number)
        {
            if (number == 0)
                return "ZERO DOLLARS";



            var dollars = (long)number;
            var cents = (long)((number - dollars) * 100);

            var words = dollars > 0 ? AsWords(dollars) + " DOLLARS" : "";

            if (cents > 0)
            {
                if (words.Length > 0)
                    words += " AND ";
                words += AsWords(cents) + " CENTS";
            }

            return words;
        }

        private static string AsWords(long number)
        {
            if (number == 0)
                return "ZERO";

            // Validate if the number exceeds the max range of a long
            if (number > 9_223_372_036_854_775_807)
                throw new ArgumentOutOfRangeException(nameof(number), "Number cannot be greater than the range of a long");

            string[] unitsMap = ["", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"];
            string[] tensMap = ["", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"];

            if (number < 20)
                return unitsMap[number];

            if (number < 100)
                return tensMap[number / 10] + (number % 10 > 0 ? "-" + unitsMap[number % 10] : "");

            if (number < 1000)
                return unitsMap[number / 100] + " HUNDRED" + (number % 100 > 0 ? " AND " + AsWords(number % 100) : "");

            if (number < 1_000_000)
                return AsWords(number / 1_000) + " THOUSAND" + (number % 1_000 > 0 ? " " + AsWords(number % 1_000) : "");

            if (number < 1_000_000_000)
                return AsWords(number / 1_000_000) + " MILLION" + (number % 1_000_000 > 0 ? " " + AsWords(number % 1_000_000) : "");

            if (number < 1_000_000_000_000)
                return AsWords(number / 1_000_000_000) + " BILLION" + (number % 1_000_000_000 > 0 ? " " + AsWords(number % 1_000_000_000) : "");

            if (number < 1_000_000_000_000_000)
                return AsWords(number / 1_000_000_000_000) + " TRILLION" + (number % 1_000_000_000_000 > 0 ? " " + AsWords(number % 1_000_000_000_000) : "");

            if (number < 1_000_000_000_000_000_000)
                return AsWords(number / 1_000_000_000_000_000) + " QUADRILLION" + (number % 1_000_000_000_000_000 > 0 ? " " + AsWords(number % 1_000_000_000_000_000) : "");

            return AsWords(number / 1_000_000_000_000_000_000) + " QUINTILLION" + (number % 1_000_000_000_000_000_000 > 0 ? " " + AsWords(number % 1_000_000_000_000_000_000) : "");
        }


    }
}
