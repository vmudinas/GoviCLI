using AutoFixture;
using AutoFixture.AutoMoq;
using InvoiceService;
using Microsoft.Extensions.Logging;
using Moq;
using PdfService;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using static InvoiceService.InvoiceService;

namespace GoviCLITests
{
    public class WorkerTests
    {
        private Worker _sut;
        private IFixture _fixture = new Fixture();
        private Mock<IInvoiceService> _invoiceServiceMock = new Mock<IInvoiceService>();
        private Mock<IPdfService> _pdfServiceMock = new Mock<IPdfService>();

        public WorkerTests()
        {
            _fixture.Customize(new AutoMoqCustomization());

            _fixture.Inject(_invoiceServiceMock);
            _fixture.Inject(_pdfServiceMock);

            _sut = _fixture.Build<Worker>().OmitAutoProperties().Create();
        }

        [Theory]
        [InlineData("5", false)]
        public async Task TestGetCachedData(string userInput, bool sort)
        {
            // Arrange
            _invoiceServiceMock.Setup(x => x.FetchData(It.IsAny<Func<Invoice, DateTime>>(), sort));
            _invoiceServiceMock.Setup(x => x.GetCachedData());

            // Act
            await _sut.ProcessUserInput(
                userInput);

            // Assert
            _invoiceServiceMock.Verify(x => x.FetchData(It.IsAny<Func<Invoice, DateTime>>(), sort), Times.Never);
            _invoiceServiceMock.Verify(x => x.GetCachedData(), Times.Once);

        }

        [Theory]
        [InlineData("6", false)]
        public async Task TestPdfService(string userInput, bool sort)
        {
            // Arrange
            _invoiceServiceMock.Setup(x => x.FetchData(It.IsAny<Func<Invoice, DateTime>>(), sort));
            _invoiceServiceMock.Setup(x => x.GetCachedData());
            _pdfServiceMock.Setup(x => x.GeneratePdf());

            // Act
            await _sut.ProcessUserInput(
                userInput);

            // Assert
            _invoiceServiceMock.Verify(x => x.FetchData(It.IsAny<Func<Invoice, DateTime>>(), sort), Times.Never);
            _invoiceServiceMock.Verify(x => x.GetCachedData(), Times.Never);
            _pdfServiceMock.Verify(x => x.GeneratePdf(), Times.Once);

        }

        [Theory]
        [InlineData("1", false)]
        [InlineData("2", true)]
        public async Task TestUserInputDateSort(string userInput, bool sort)
        {
            // Arrange
             
            _invoiceServiceMock.Setup(x => x.FetchData(It.IsAny<Func<Invoice, DateTime>>(), sort));

            // Act
            await _sut.ProcessUserInput(
                userInput);

            // Assert
            _invoiceServiceMock.Verify(x => x.FetchData(It.IsAny<Func<Invoice, DateTime>>(), sort), Times.Once);
          

        }

        [Theory]
        [InlineData("3", false)]
        [InlineData("4", true)]
        public async Task TestUserInputAmountSort(string userInput, bool sort)
        {
            // Arrange

            _invoiceServiceMock.Setup(x => x.FetchData(It.IsAny<Func<Invoice, double>>(), sort));

            // Act
            await _sut.ProcessUserInput(
                userInput);

            // Assert
            _invoiceServiceMock.Verify(x => x.FetchData(It.IsAny<Func<Invoice, double>>(), sort), Times.Once);


        }
    }
}
