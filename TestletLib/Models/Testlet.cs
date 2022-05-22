namespace TestletLib.Models;

public class Testlet
{
    public string? TestletId { get; set; }
    public List<QuestionItem> Items { get; set; }

    public List<QuestionItem>? PretestItems => Items.Where(it => it.ItemType == ItemTypeEnum.Pretest).ToList();
    public List<QuestionItem>? OperationalItems => Items.Where(it => it.ItemType == ItemTypeEnum.Operational).ToList();

    public Testlet(List<QuestionItem> items, string? id)
    {
        TestletId = id;
        Items = items;
    }
}