using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Infrastructure;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Service;

namespace NutritionDiary.TestProject
{
    class FoodServiceTests
    {
        private Mock<IFoodRepository> mockRepository;
        private IUnitOfWork unitOfWork;
        private List<Food> foodList;
        private List<Meal> mealList;

        [SetUp]
        protected void SetUp()
        {
            mockRepository = new Mock<IFoodRepository>();
            unitOfWork = new Mock<IUnitOfWork>().Object;

            foodList = new List<Food>()
            {
                new Food { Id = 42, Fats = 13 },
                new Food { Id = 21, Fats = 33 }
            };
        }


        [Test]
        public void GetFood_ShouldReturnAllFood()
        {

            mockRepository.Setup(x => x.GetAll()).Returns(foodList);
            var service = new FoodService(mockRepository.Object, unitOfWork);

            var result = service.GetFoods();

            Assert.NotNull(result);
            Assert.AreEqual(2, foodList.Count);

        }

        [Test]
        public void GetFoodById_SholdReturnFoodWithSameId()
        {

            mockRepository.Setup(x => x.GetById(It.IsAny<int>())).
                Returns(new Func<int, Food>(id => foodList.Find(p => p.Id.Equals(id))));

            var service = new FoodService(mockRepository.Object, unitOfWork);

            var result = service.GetFoodById(21);

            Assert.NotNull(result);
            Assert.AreEqual(21, result.Id);
        }
    }
}
