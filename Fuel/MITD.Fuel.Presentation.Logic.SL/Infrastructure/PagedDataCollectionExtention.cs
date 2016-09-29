using MITD.Presentation;
using MITD.Presentation.Contracts;

namespace MITD.Fuel.Presentation.Logic.SL.Infrastructure
{
    public static class PagedDataCollectionExtention
    {
        public static void SetPagedDataCollection<T>(this PagedSortableCollectionView<T> collection, PageResultDto<T> source) where T : class
        {
            collection.Clear();

            if (source != null)
            {
                collection.SourceCollection = source.Result;

                collection.ItemCount = source.TotalCount;
            }
        }
    }
}