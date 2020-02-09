namespace SimpleDictionary.Models.DataModels
{
    public class Hashtag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Definition Definition { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Hashtag hashtag = obj as Hashtag;

            if (hashtag.Name.Equals(Name))
                return true;
            else return false;   
        }
    }
}
