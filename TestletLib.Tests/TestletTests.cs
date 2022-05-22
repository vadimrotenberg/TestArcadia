using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Moq;
using TestletLib.Configuration;
using TestletLib.Models;
using TestletLib.Repository.Abstractions;
using TestletLib.Repository.Implementations;
using Xunit;

namespace TestletLib.Tests
{
    public class TestletTests
    {
        private readonly QuestionsConfiguration _configuration;

        public TestletTests()
        {
            var config = InitConfiguration();
            _configuration = new QuestionsConfiguration
            {
                OperationalNumber = int.Parse(config[nameof(IQuestionsConfiguration.OperationalNumber)]),
                PretestNumber = int.Parse(config[nameof(IQuestionsConfiguration.PretestNumber)]),
                NumberOfLeadingPretestItems =
                    int.Parse(config[nameof(IQuestionsConfiguration.NumberOfLeadingPretestItems)])
            };
        }

        [Fact(DisplayName = "Verify_Total_Count_and_Pretests_Count_and_OperationalCount")]
        public void TestletHasCorrectNumberOfItems()
        {
            var (id, result) = BuildTestlet();
            var total = _configuration.OperationalNumber + _configuration.PretestNumber;

            Assert.NotNull(result);
            Assert.NotNull(result.Items);

            Assert.Equal(id, result.TestletId);

            //Items are all different
            Assert.Equal(total, result.Items.Select(it => it.QuestionItemId).Distinct().Count());

            Assert.Equal(total, result.Items.Count);

            Assert.Equal(_configuration.OperationalNumber, result.OperationalItems?.Count);
            Assert.Equal(_configuration.PretestNumber, result.PretestItems?.Count);
        }

        [Fact(DisplayName = "First_2_items_are_always_pretest_items")]
        public void FirstTwoItemsAlwaysPretest()
        {
            var (id, result) = BuildTestlet();
            Assert.Equal(ItemTypeEnum.Pretest, result.Items.Single(it => it.Order == 1).ItemType);
            Assert.Equal(ItemTypeEnum.Pretest, result.Items.Single(it => it.Order == 2).ItemType);
        }


        [Fact(DisplayName = "Last_8_items_are_mix_of_pretest_and_operational_items_ordered_randomly")]
        public void LastEightItemsAreMixPretestAndOperationalItems()
        {
            var (id, result) = BuildTestlet();

            Assert.Equal(_configuration.OperationalNumber,
                result.Items.Count(it =>
                    it.Order > _configuration.NumberOfLeadingPretestItems && it.ItemType == ItemTypeEnum.Operational));

            Assert.Equal(_configuration.PretestNumber - _configuration.NumberOfLeadingPretestItems,
                result.Items.Count(it =>
                    it.Order > _configuration.NumberOfLeadingPretestItems && it.ItemType == ItemTypeEnum.Pretest));
        }

        [Fact(DisplayName = "All_items_are_correctly_ordered")]
        public void AllItemsAreCorrectlyOrdered()
        {
            var (id, result) = BuildTestlet();

            // All items are correctly ordered
            var groups = result.Items.GroupBy(it => it.Order).Select(g => new { order = g.Key, count = g.Count() });
            foreach (var g in groups)
                Assert.Equal(1, g.count);

            for (var order = 1; order <= _configuration.PretestNumber + _configuration.OperationalNumber; order++)
                Assert.Equal(1, result.Items.Count(it => it.Order == order));
        }

        private (string id, Testlet result) BuildTestlet()
        {
            var id = Guid.NewGuid().ToString();
            var configurationMock = new Mock<IQuestionsConfiguration>();
            configurationMock.SetupGet(c => c.OperationalNumber).Returns(_configuration.OperationalNumber);
            configurationMock.SetupGet(c => c.PretestNumber).Returns(_configuration.PretestNumber);
            configurationMock.SetupGet(c => c.TestletId).Returns(id);

            var mockRepository = new Mock<IQuestionsRepository>();
            mockRepository.SetupGet(r => r.TestletData)
                .Returns(DataSource.TestletDataSource.FetchQuestions(configurationMock.Object));

            ITestletService testletService = new TestletService(mockRepository.Object);

            return (id, testletService.BuildTestlet(id));
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }
    }
}