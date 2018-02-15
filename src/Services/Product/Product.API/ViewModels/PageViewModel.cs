using System.Collections.Generic;

namespace Product.API.ViewModels
{
    public class PageViewModel<TModel> where TModel : class
    {
        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public long Count { get; private set; }

        public IEnumerable<TModel> Data { get; private set; }

        public PageViewModel(int pageIndex, int pageSize, long count, IEnumerable<TModel> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }
    }
}
