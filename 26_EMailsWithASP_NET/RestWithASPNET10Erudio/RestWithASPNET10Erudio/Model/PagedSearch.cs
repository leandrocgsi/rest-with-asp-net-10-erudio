using RestWithASPNET10Erudio.Hypermedia.Abstract;
using System.Xml.Serialization;

namespace RestWithASPNET10Erudio.Model
{
    public class PagedSearch<T>
	{
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SortDirections { get; set; } = "asc";
        public int TotalResults { get; set; }
        public List<T> List { get; set; }
	}
}
