namespace Data.Models
{
    public class TheatricalCharacter
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
