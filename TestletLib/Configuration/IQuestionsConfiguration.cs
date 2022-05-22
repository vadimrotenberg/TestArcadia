namespace TestletLib.Configuration;

public interface IQuestionsConfiguration
{
    int PretestNumber { get; set; }
    int OperationalNumber { get; set; }
    string TestletId { get; set; }
    int NumberOfLeadingPretestItems { get; set; }
}