namespace RestWithASPNET10Erudio.Repositories.QueryBuilders
{
    public class PersonQueryBuilder
    {
        public (
            string query,
            string countQuery,
            string sort,
            int size,
            int offset)
            BuildQueries(
                string name,
                string sortDirection,
                int pageSize,
                int page)
        {
            page = Math.Max(1, page);

            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? 1 : pageSize;

            var sort = !string.IsNullOrWhiteSpace(sortDirection) &&
                !sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "asc" : "desc";

            var whereClause = $" FROM person p WHERE 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(name))
                whereClause += $" AND (p.first_name LIKE '%{name}%') ";

            var query = $@"
                SELECT * {whereClause}
                ORDER BY p.first_name {sort}
                OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY";

            var countQuery = $"SELECT COUNT(*) {whereClause}";
            return (query, countQuery, sort, size, offset);
        }
    }
}
