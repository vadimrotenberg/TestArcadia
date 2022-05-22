namespace TestletLib.Configuration;

public class QuestionsConfiguration : IQuestionsConfiguration
{
    public int PretestNumber { get; set; }
    public int OperationalNumber { get; set; }
    public string TestletId { get; set; }

    public int NumberOfLeadingPretestItems { get; set; }
}