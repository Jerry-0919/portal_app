using System.Collections.Generic;

namespace diga.web.Models.Pagination
{
    public class PaginatedList<T>
    {
        public List<T> List { get; set; }
        public int CountAll { get; set; }
    }
}
