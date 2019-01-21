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

            mealList = new List<Meal>()
            {
                new Meal() {
                    Id = 1,
                    SetDate = new DateTime(2017, 10, 17),
                    Foods = new List<Food>() {
                        new Food { Id = 42, KCalory = 155, Hydrates = 30, Fats = 10, Protein = 6},
                        new Food { Id = 21, KCalory = 55, Hydrates = 12, Fats = 5, Protein = 3 }
                        }
                },
                 new Meal() {
                    Id = 2,
                    SetDate = new DateTime(2017, 10, 17),
                    Foods = new List<Food>() {
                        new Food { Id = 43, KCalory = 150, Hydrates = 30, Fats = 10, Protein = 6},
                        new Food { Id = 22, KCalory = 55, Hydrates = 12, Fats = 5, Protein = 3 }
                        }
                }
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

            mockRepository.Setup(x => x.GetById(21)).
                Returns(new Func<int, Food>(id => foodList.Find(p => p.Id.Equals(id))));

            var service = new FoodService(mockRepository.Object, unitOfWork);

            var result = service.GetFoodById(21);

            Assert.NotNull(result);
            Assert.AreEqual(21, result.Id);
        }

        [Test]
        public void AddSumOfNutritionPerMeal_ShouldReturnCorrectSum()
        {

            var mealRepository = new Mock<IMealRepository>();
            var unitOfWork = new Mock<IUnitOfWork>().Object;


            mealRepository.Setup(x => x.SumOfNutrientsPerMeal(1))
                    .Returns(new Func<int, MealTotalNutrients>
                    (f => mealList.GroupBy(x => x.Id).Select(x => new MealTotalNutrients
                    {
                        TotalCalories = x.Sum(k => k.Foods.Sum(c => (int?)c.KCalory)),
                        TotalFats = x.Sum(k => k.Foods.Sum(c => (int?)c.Fats)),
                        TotalCarbs = x.Sum(k => k.Foods.Sum(c => (int?)c.Hydrates)),
                        TotalProteins = x.Sum(k => k.Foods.Sum(c => (int?)c.Protein))
                    }).FirstOrDefault()));


            var service = new MealService(null, mealRepository.Object, null, unitOfWork);


            var result = service.SumOfNutrientsPerMeal(1);


            Assert.AreEqual(42, result.TotalCarbs);
            Assert.AreEqual(210, result.TotalCalories);
            Assert.AreEqual(15, result.TotalFats);
            Assert.AreEqual(9, result.TotalProteins);
        }
    }
}
