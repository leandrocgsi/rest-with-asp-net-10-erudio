using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithASPNET10Erudio.Hypermedia.Abstract
{
    public interface IResponseEnricher
    {
        bool CanEnrich(ResultExecutingContext context);
        Task Enrich(ResultExecutingContext context);
    }
}
