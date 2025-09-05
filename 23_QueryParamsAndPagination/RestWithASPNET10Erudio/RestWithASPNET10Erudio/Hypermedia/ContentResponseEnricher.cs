using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RestWithASPNET10Erudio.Hypermedia.Abstract;
using RestWithASPNET10Erudio.Hypermedia.Utils;

namespace RestWithASPNET10Erudio.Hypermedia
{
    public abstract class ContentResponseEnricher<T>
        : IResponseEnricher where T : ISupportsHypermedia
    {
        public virtual bool CanEnrich(Type contentType)
        {
            return contentType == typeof(T)
                || contentType == typeof(List<T>)
                || contentType == typeof(PagedSearchDTO<T>);
        }

        protected abstract Task EnrichModel(
            T content, IUrlHelper urlHelper);

        bool IResponseEnricher.CanEnrich(ResultExecutingContext response)
        {
            if (response.Result is OkObjectResult okObjectResult)
            {
                return CanEnrich(okObjectResult.Value.GetType());
            }
            return false;
        }
        public async Task Enrich(ResultExecutingContext response)
        {
            var urlHelper = new UrlHelperFactory()
                .GetUrlHelper(response);
            if (response.Result is OkObjectResult okObjectResult)
            {
                if (okObjectResult.Value is T model)
                {
                    await EnrichModel(model, urlHelper);
                }
                else if (okObjectResult.Value is List<T> collection)
                {
                    foreach (var element in collection)
                    {
                        await EnrichModel(element, urlHelper);
                    }
                }
                else if (okObjectResult.Value is PagedSearchDTO<T> pagedSearch)
                {
                    foreach (var element in pagedSearch.List)
                    {
                        element.Links?.Clear();
                        await EnrichModel(element, urlHelper);
                    }
                }
            }
            await Task.CompletedTask;
        }
    }
}
