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
    class MealRepositoryTest
    {
        private Mock<IMealRepository> mockRepository;
        private IUnitOfWork unitOfWork;
        private List<Meal> mealList;
        private MealService service;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IMealRepository>();
            unitOfWork = new Mock<IUnitOfWork>().Object;
            service = new MealService(null, mockRepository.Object, null, unitOfWork);

            mealList = new List<Meal>()
            {
                new Meal()  {
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
        public void AddSumOfNutritionPerMeal_ShouldReturnCorrectSum()
        {

            mockRepository.Setup(x => x.SumOfNutrientsPerMeal(1))
                    .Returns(new Func<int, MealTotalNutrients>
                    (f => mealList.GroupBy(x => x.Id).Select(x => new MealTotalNutrients
                    {
                        TotalCalories = x.Sum(k => k.Foods.Sum(c => (int?)c.KCalory)),
                        TotalFats = x.Sum(k => k.Foods.Sum(c => (int?)c.Fats)),
                        TotalCarbs = x.Sum(k => k.Foods.Sum(c => (int?)c.Hydrates)),
                        TotalProteins = x.Sum(k => k.Foods.Sum(c => (int?)c.Protein))
                    }).FirstOrDefault()));

            var result = service.SumOfNutrientsPerMeal(1);


            Assert.AreEqual(42, result.TotalCarbs);
            Assert.AreEqual(210, result.TotalCalories);
            Assert.AreEqual(15, result.TotalFats);
            Assert.AreEqual(9, result.TotalProteins);
        }
    }
}
