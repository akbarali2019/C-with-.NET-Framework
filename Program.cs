
public class SlideWindow
{
    public static void Main()
    {
        int[] arrayList = new int[] { 2, 1, 5, 1, 3, 2 };
        int k = 3;
        int max_sum = 0;
        int window_sum = 0;

        for (int i = 0; i < arrayList.Length; i++)
        {
            window_sum += arrayList[i];
            if (i >= k - 1)
            {
                max_sum = Math.Max(max_sum, window_sum);
                window_sum -= arrayList[i - (k - 1)];
            }
        }
        Console.WriteLine($"max_sum: {max_sum}");
    }
}
