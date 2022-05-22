using TestletLib.Models;
using TestletLib.Repository.Abstractions;

namespace TestletLib.Repository.Implementations;

public class TestletService : ITestletService
{
    private readonly IQuestionsRepository _repository;

    public TestletService(IQuestionsRepository repository)
    {
        _repository = repository;
    }

    public Testlet BuildTestlet(string testLetId)
    {
        var order = 0;
        var testLet = _repository.TestletData;
        var first2Items = testLet.PretestItems?.Take(2).Select(it => it.WithOrder(++order)).ToList();
        testLet.Items?.RemoveAll(it => it.Order > 0);
        if (testLet.Items != null)
        {
            var next8Items = testLet.Items.OrderBy(it => it.QuestionItemId).Select(it => it.WithOrder(++order))
                .ToList();
            testLet.Items.Clear();
            if (first2Items != null) testLet.Items.AddRange(first2Items);
            testLet.Items.AddRange(next8Items);
        }

        return testLet;
    }
}