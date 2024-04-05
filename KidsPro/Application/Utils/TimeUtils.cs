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
    
    public static long GetOrderTimeSpan(DateTime date)
    {
        var unixTimestamp = (long)(date.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

        // Thời điểm thanh toán
        var paymentTime = unixTimestamp - 15 * 60 * 1000; // 15 phút trước, tính bằng mili giây

        if (unixTimestamp >= paymentTime && unixTimestamp <= paymentTime + 15 * 60 * 1000)
        {
            return unixTimestamp;
        }
            throw new Exception("Thời gian tạo đơn hàng phải nằm trong khoảng 15 phút so với thời điểm thanh toán.");
    }
    

}