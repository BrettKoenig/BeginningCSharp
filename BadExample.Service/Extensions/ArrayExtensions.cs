namespace BadExample.Service.Extensions
{
    public static class ArrayExtensions
    {
        public static void TrimValues(this string[] items)
        {
            if (items == null)
                return;
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = items[i].Trim();
            }
        }
    }
}
