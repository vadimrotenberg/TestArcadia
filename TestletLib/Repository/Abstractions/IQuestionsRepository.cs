using TestletLib.Models;

namespace TestletLib.Repository.Abstractions;

public interface IQuestionsRepository
{
    Testlet TestletData { get; set; }
}