namespace Application.Utils;

public static class TimeUtils
{
    public static (TimeSpan, TimeSpan) GetTimeFromSlot(int slot, int minutes)
    {
        TimeSpan open = TimeSpan.Zero;
        TimeSpan close = TimeSpan.Zero;
        switch (slot)
        {
            // Start 8:00 - End (Start+Minutes)
            case 1:
                open = new TimeSpan(8, 0, 0);
                close = open.Add(new TimeSpan(0, minutes, 0));
                break;
            // Start 10:00 - End (Start+Minutes)
            case 2:
                open = new TimeSpan(10, 0, 0);
                close = open.Add(new TimeSpan(0, minutes, 0));
                break;
            // Start 14:00 - End (Start+Minutes)
            case 3:
                open = new TimeSpan(14, 0, 0);
                close = open.Add(new TimeSpan(0, minutes, 0));
                break;
            // Start 16:00 - End (Start+Minutes)
            case 4:
                open = new TimeSpan(16, 0, 0);
                close = open.Add(new TimeSpan(0, minutes, 0));
                break;
            // Start 18:00 - End (Start+Minutes)
            case 5:
                open = new TimeSpan(18, 0, 0);
                close = open.Add(new TimeSpan(0, minutes, 0));
                break;
            // Start 20:00 - End (Start+Minutes)
            case 6:
                open = new TimeSpan(20, 0, 0);
                close = open.Add(new TimeSpan(0, minutes, 0));
                break;
        }

        return (open, close);
    }
}