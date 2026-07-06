namespace Microondas.Core.Helpers
{
    public static class TimeFormatter
    {
        public static string Format(int totalSeconds)
        {
            if (totalSeconds < 0)
                totalSeconds = 0;

            var minutes = totalSeconds / 60;
            var seconds = totalSeconds % 60;

            if (minutes > 0)
                return minutes + ":" + seconds.ToString("D2");

            return totalSeconds.ToString();
        }
    }
}