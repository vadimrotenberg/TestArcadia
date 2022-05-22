namespace TestletLib.Models
{
    public class QuestionItem
    {
        public int Order { get; set; }
        public string QuestionItemId { get; set; }
        public ItemTypeEnum ItemType { get; set; } = ItemTypeEnum.Undefined;
        public string Content { get; set; } = "";
        public List<(int,string)>? Variants { get; set; }
        public int CorrectAnswer { get; set; }

        public QuestionItem WithOrder(int order)
        {
            Order = order;
            return this;
        }
    }
}
