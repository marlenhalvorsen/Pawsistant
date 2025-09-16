using Library.Shared.Model;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq; 
using PawsistantAPI.Adapters.Interfaces;
using PawsistantAPI.Services; 

namespace PawsistantTest
{
    [TestClass]
    public class PawsistantServiceTest
    {
        [TestMethod]
        public async Task GetResponseAsync_ForwardsCallAndReturnAdapterResult()
        {
            //ARRANGE
            var input = new ChatMessage { Role = "user", Content = "Hi" };
            var expected = new ChatMessage { Role = "asssistant", Content = "Hello!" };

            var adapter = new Mock<IAiChatProviderAdapter>();
            adapter.Setup(a=> a.GetChatMessageAsync(input)).ReturnsAsync(expected);
            var sut = new PawsistantService(adapter.Object);


            //ACT
            var actual = await sut.GetResponseAsync(input);


            //ASSERT
            Assert.AreSame(expected, actual);
            adapter.Verify(a => a.GetChatMessageAsync(input), Times.Once);
            adapter.VerifyNoOtherCalls();

        }

        [TestMethod]
        public async Task GetResponseAsync_NullMessage_ThrowsArgumentNullException()
        {
            //ARRANGE
            var adapter = new Mock<IAiChatProviderAdapter>();
            var sut = new PawsistantService(adapter.Object);

            //ASSERT
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => sut.GetResponseAsync(null!)
            );
        }

        [TestMethod]
        public async Task GetResponseAsync_EmptyContent_ThrowsArgumentException()
        {
            //ARRANGE
            var adapter = new Mock<IAiChatProviderAdapter>(); 
            var sut = new PawsistantService(adapter.Object);

            var msg = new ChatMessage { Role = "user ", Content = " " };

            //ASSERT
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => sut.GetResponseAsync(msg)
            );
        }

        [TestMethod]

        public async Task Ctor_NullAdapterThrows()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new PawsistantService(null!)
                );
        }
    }
}
