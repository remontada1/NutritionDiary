using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WebApplication1.Infrastructure;
using WebApplication1.Models;
using WebApplication1.Service;

namespace NutritionDiary.TestProject
{
    class FoodServiceTestscs
    {

        [Test]
        public void GetFood_ShouldReturnAllFood()
        {
            var mockRepository = new Mock<IFoodRepository>();
            var unitOfWork = new Mock<IUnitOfWork>().Object;

            var expected = new List<Food>()
            {
                new Food { Id = 42, Fats = 13 },
                new Food { Id = 21, Fats = 33 }
            };

            mockRepository.Setup(x => x.GetAll()).Returns(expected);
            var service = new FoodService(mockRepository.Object, unitOfWork);

            var result = service.GetFoods();

            Assert.NotNull(result);
            Assert.AreEqual(2, expected.Count);

        }

        [Test]
        public void GetFoodById_SholdReturnFoodWithSameId()
        {
             var mockRepository = new Mock<IFoodRepository>();
            var unitOfWork = new Mock<IUnitOfWork>().Object;

            var expected = new List<Food>()
            {
                new Food { Id = 42, Fats = 13 },
                new Food { Id = 21, Fats = 33 }
            };

            mockRepository.Setup(x => x.GetById(It.IsAny<int>())).
                Returns(new Func<int, Food>(id => expected.Find(p => p.Id.Equals(id))));

            var service = new FoodService(mockRepository.Object, unitOfWork);

            var result = service.GetFoodById(21);

            Assert.NotNull(result);
            Assert.AreEqual(21, result.Id);
        }
    }
}
