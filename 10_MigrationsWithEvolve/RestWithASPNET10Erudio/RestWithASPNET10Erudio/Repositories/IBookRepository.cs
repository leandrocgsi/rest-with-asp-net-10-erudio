using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Repositories
{
    public interface IBookRepository
    {
        Book Create(Book book);

        Book FindById(long id);

        List<Book> FindAll();

        Book Update(Book book);

        void Delete(long id);
    }
}
