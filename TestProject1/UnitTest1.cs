using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using Moq;
using NUnit.Framework;
using System.Data;
using Assert = NUnit.Framework.Assert;

namespace ZooZen_App
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<Api> apiMock;
        private General gen;


        [SetUp]
        public void SetUp()
        {
            // Mock the Api dependency
            apiMock = new Mock<Api>(MockBehavior.Strict);

            // Create an instance of Form1 with the mocked Api
            gen = new General(apiMock.Object);
        }



        [TestMethod]
        public void GetProducts_WhenApiReturnsValidResponse_PopulatesDataGridView()
        {
            // Arrange
            List<ProductDTO> products = new List<ProductDTO>
            {
                new ProductDTO { ProductName = "Product 1", Bvin = "Bvin1", SitePrice = 10.0m },
                new ProductDTO { ProductName = "Product 2", Bvin = "Bvin2", SitePrice = 20.0m }
            };

            ApiResponse<List<ProductDTO>> apiResponse = new ApiResponse<List<ProductDTO>>
            {
                Content = products
            };

            apiMock.Setup(mock => mock.ProductsFindAll()).Returns(apiResponse);

            // Act
            gen.GetProducts();

            // Assert
            DataTable dataTable = gen.GetDataGridViewDataSource();
            Assert.AreEqual(2, dataTable.Rows.Count);

            Assert.AreEqual("Product 1", dataTable.Rows[0]["ProductName"]);
            Assert.AreEqual("Bvin1", dataTable.Rows[0]["Sku"]);
            Assert.AreEqual(10.0m, dataTable.Rows[0]["SitePrice"]);

            Assert.AreEqual("Product 2", dataTable.Rows[1]["ProductName"]);
            Assert.AreEqual("Bvin2", dataTable.Rows[1]["Sku"]);
            Assert.AreEqual(20.0m, dataTable.Rows[1]["SitePrice"]);
        }

        [TestMethod]
        public void SavedItem_WhenApiReturnsValidResponse_UpdatesProductSitePrice()
        {
            // Arrange
            string bvin = "Bvin1";
            int newSitePrice = 50;

            var productDTO = new ProductDTO
            {
                ProductName = "Product 1",
                Bvin = bvin,
                SitePrice = 10.0m
            };

            apiMock.Setup(mock => mock.ProductsFind(bvin)).Returns(new ApiResponse<ProductDTO> { Content = productDTO });
            apiMock.Setup(mock => mock.ProductsUpdate(productDTO)).Returns(new ApiResponse<ProductDTO>());

            // Act
            gen.SavedItem(bvin, newSitePrice.ToString());

            // Assert
            apiMock.Verify(mock => mock.ProductsFind(bvin), Times.Once);
            apiMock.Verify(mock => mock.ProductsUpdate(It.Is<ProductDTO>(p => p.SitePrice == newSitePrice)), Times.Once);
        }
    }
}