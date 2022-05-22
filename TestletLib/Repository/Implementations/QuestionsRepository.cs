using TestletLib.Configuration;
using TestletLib.Models;
using TestletLib.Repository.Abstractions;

namespace TestletLib.Repository.Implementations;

public class QuestionsRepository : IQuestionsRepository
{
    public QuestionsRepository(IQuestionsConfiguration configuration)
    {
        TestletData = DataSource.TestletDataSource.FetchQuestions(configuration);
    }

    public Testlet TestletData { get; set; }
}