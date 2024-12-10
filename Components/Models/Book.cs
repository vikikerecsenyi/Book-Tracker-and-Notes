namespace booknote.Components.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsRead { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();
    }

    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
