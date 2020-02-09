namespace SimpleDictionary.Models.DataModels
{
    public class LikedDefinition
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }

        public int DefinitionId { get; set; }
        public Definition Definition { get; set; }
    }
}
