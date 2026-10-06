namespace Lab31.App.Common;

public static class ArrayUtil
{
    public static void Append<T>(ref T[] array, T item)
    {
        Array.Resize(ref array, array.Length + 1);
        array[^1] = item;
    }
}