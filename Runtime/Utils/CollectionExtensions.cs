using System.Collections.Generic;

namespace GoodHub.Core.Runtime.Utils
{
    public static class CollectionExtensions
    {
        public static bool ContainsAtLeastOne<T>(this List<T> list, List<T> items) where T : class
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    if (list[i] == items[j])
                        return true;
                }
            }

            return false;
        }
    }
}