using booknote.Components.Models;
using System.Net.Http;
using System.Text.Json;

namespace booknote.Components.Services
{
    public class DataService
    {
        private readonly string FilePath;

        public DataService()
        {
            var basePath = Directory.GetCurrentDirectory();
            FilePath = Path.Combine(basePath, "wwwroot", "Data", "books.json");
        }
        public async Task<List<Book>> GetBooksAsync()
        {
            if (!File.Exists(FilePath))
            {
                return new List<Book>();
            }

            var json = await File.ReadAllTextAsync(FilePath);
            var books = JsonSerializer.Deserialize<List<Book>>(json);
            return books ?? new List<Book>();
        }
        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            var books = await GetBooksAsync();
            return books.FirstOrDefault(b => b.Id == bookId);
        }

        public async Task SaveBooksAsync(List<Book> books)
        {
            var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }

        public async Task AddBookAsync(Book newBook)
        {
            var books = await GetBooksAsync();
            newBook.Id = books.Count + 1;
            books.Add(newBook);
            await SaveBooksAsync(books);
        }
        public async Task UpdateBookAsync(Book updatedBook)
        {
            var books = await GetBooksAsync();
            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book != null)
            {
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.IsRead = updatedBook.IsRead;
                book.Notes = updatedBook.Notes;
                await SaveBooksAsync(books);
            }
        }
        public async Task DeleteBookAsync(int bookId)
        {
            var books = await GetBooksAsync();
            var book = books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                books.Remove(book);
                await SaveBooksAsync(books);
            }
        }
        public async Task AddNoteAsync(int bookId, Note note)
        {
            var books = await GetBooksAsync();
            var book = books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                note.Id = book.Notes.Count + 1;
                book.Notes.Add(note);
                await SaveBooksAsync(books);
            }
        }

        public async Task EditNoteAsync(int bookId, Note note)
        {
            var books = await GetBooksAsync();
            var book = books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                var existingNote = book.Notes.FirstOrDefault(n => n.Id == note.Id);
                if (existingNote != null)
                {
                    existingNote.Title = note.Title;
                    existingNote.Content = note.Content;
                    await SaveBooksAsync(books);
                }
            }
        }

        public async Task DeleteNoteAsync(int bookId, int noteId)
        {
            var books = await GetBooksAsync();
            var book = books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
            {
                var noteToDelete = book.Notes.FirstOrDefault(n => n.Id == noteId);
                if (noteToDelete != null)
                {
                    book.Notes.Remove(noteToDelete);
                    await SaveBooksAsync(books);
                }
            }
        }
    }
}
