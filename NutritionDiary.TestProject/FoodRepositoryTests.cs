using Moq;
using NUnit.Framework;
using System.Data.Entity;
using WebApplication1.DAL;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace NutritionDiary.TestProject
{
    class FoodRepositoryTests
    {
        [Test]
        public void AddFoodToDbSet()
        {
            var factoryMock = new Mock<IDbFactory>();
            var contextMock = new Mock<CustomerContext>();
            var dbSetFoodMock = new Mock<DbSet<Food>>();


            dbSetFoodMock.Setup(x => x.Add(It.IsAny<Food>())).Returns((Food f) => f);
            contextMock.Setup(x => x.Set<Food>()).Returns(dbSetFoodMock.Object);
            factoryMock.Setup(x => x.Init()).Returns(contextMock.Object);

            var mockRepository = new Mock<IFoodRepository>();
            var unitOfWork = new Mock<IUnitOfWork>().Object;

            var expected = new Food { Id = 42, Fats = 13 };

            var repository = new FoodRepository(factoryMock.Object);

            repository.Add(expected);

            contextMock.Verify(x => x.Set<Food>());
            dbSetFoodMock.Verify(x => x.Add(It.Is<Food>(y => y == expected)));
        }
    }
}
