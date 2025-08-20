using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Model.Context;

namespace RestWithASPNET10Erudio.Repositories.Impl
{
    public class BookRepository : IBookRepository
    {

        private MSSQLContext _context;

        public BookRepository(MSSQLContext context)
        {
            _context = context;
        }
        public List<Book> FindAll()
        {
            return _context.Books.ToList();
        }

        public Book FindById(long id)
        {
            return _context.Books.Find(id);
        }

        public Book Create(Book book)
        {
            _context.Add(book);
            _context.SaveChanges();
            return book;
        }

        public Book Update(Book book)
        {
            var existingBook = _context.Books.Find(book.Id);
            if (existingBook == null) return null;

            _context.Entry(existingBook).CurrentValues.SetValues(book);
            _context.SaveChanges();
            return book;
        }
        public void Delete(long id)
        {
            var existingBook = _context.Books.Find(id);
            if (existingBook == null) return;
            _context.Remove(existingBook);
            _context.SaveChanges();
        }
    }
}