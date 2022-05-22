using TestletLib.Configuration;
using TestletLib.Models;

namespace TestletLib.DataSource;

public class TestletDataSource
{
    public static Testlet FetchQuestions(IQuestionsConfiguration configuration)
    {
        var totalNumber = configuration.PretestNumber + configuration.OperationalNumber;
        var items = new List<QuestionItem>(totalNumber);

        // Randomly create the pretests items according it's count
        for (var i = 1; i <= configuration.PretestNumber; i++)
            items.Add(new QuestionItem
            {
                QuestionItemId = Guid.NewGuid().ToString(),
                ItemType = ItemTypeEnum.Pretest,
                Content = $"${ItemTypeEnum.Pretest} #{i}"
            });

        // Randomly create the operational items according it's count
        for (var i = configuration.PretestNumber + 1; i <= totalNumber; i++)
            items.Add(new QuestionItem
            {
                QuestionItemId = Guid.NewGuid().ToString(),
                ItemType = ItemTypeEnum.Operational,
                Content = $"${ItemTypeEnum.Operational} #{i}"
            });

        return new Testlet(items, configuration.TestletId);
    }
}