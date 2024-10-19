using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using TechTest.Controller;

namespace TechTest.Tests
{
    public class NumberToWordsControllerTests
    {
        private readonly Mock<IWebHostEnvironment> _mockEnvironment;
        private readonly NumberToWordsController _controller;

        public NumberToWordsControllerTests()
        {
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockEnvironment.Setup(env => env.WebRootPath).Returns("wwwroot");
            _controller = new NumberToWordsController(_mockEnvironment.Object);
        }

        [Fact]
        public void GetHtml_FileExists_ReturnsHtmlFile()
        {
            // Arrange
            var directoryPath = Path.Combine("wwwroot");
            var filePath = Path.Combine(directoryPath, "index.html");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(filePath, "<html><body>Test</body></html>");

            // Act
            var result = _controller.GetHtml();

            // Assert
            var physicalFileResult = Assert.IsType<PhysicalFileResult>(result);
            Assert.Equal("text/html", physicalFileResult.ContentType);

            // Clean up
            File.Delete(filePath);
            Directory.Delete(directoryPath);
        }


        [Theory]
        [InlineData("123", "ONE HUNDRED AND TWENTY-THREE DOLLARS")]
        [InlineData("0", "ZERO DOLLARS")]
        [InlineData("123.45", "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS")]
        [InlineData("-123.45", "NEGATIVE ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS")]
        [InlineData("1000", "ONE THOUSAND DOLLARS")]
        [InlineData("1000000", "ONE MILLION DOLLARS")]
        [InlineData("1000000000", "ONE BILLION DOLLARS")]
        [InlineData("1000000000000", "ONE TRILLION DOLLARS")]
        [InlineData("999999999999999.99", "NINE HUNDRED AND NINETY-NINE TRILLION NINE HUNDRED AND NINETY-NINE BILLION NINE HUNDRED AND NINETY-NINE MILLION NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS AND NINETY-NINE CENTS")]
        [InlineData("0.99", "NINETY-NINE CENTS")]
        [InlineData("100.01", "ONE HUNDRED DOLLARS AND ONE CENT")]
        [InlineData("-1000000.50", "NEGATIVE ONE MILLION DOLLARS AND FIFTY CENTS")]
        [InlineData("123456789.987", "ONE HUNDRED AND TWENTY-THREE MILLION FOUR HUNDRED AND FIFTY-SIX THOUSAND SEVEN HUNDRED AND EIGHTY-NINE DOLLARS AND NINETY-EIGHT CENTS")]
        public void Convert_ValidNumber_ReturnsWords(string number, string expectedWords)
        {
            // Act
            var result = _controller.Convert(number);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedWords, ((dynamic)okResult.Value).words);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("")]
        [InlineData(null)]
        public void Convert_InvalidNumber_ReturnsBadRequest(string number)
        {
            // Act
            var result = _controller.Convert(number);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid number format", ((dynamic)badRequestResult.Value).error);
        }

        [Theory]
        [InlineData("9223372036854775808")] // greater than long.MaxValue
        [InlineData("-9223372036854775809")] // less than long.MinValue
        public void Convert_NumberOutOfRange_ReturnsBadRequest(string number)
        {
            // Act
            var result = _controller.Convert(number);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Number is out of range. Please enter a number between the max and min number of a 64-bit integer.", ((dynamic)badRequestResult.Value).error);
        }


    }
}
