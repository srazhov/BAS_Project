using System.Globalization;

namespace BAS_Project.Helpers
{
    public static class DateHelpers
    {
        public static string GetISO_8601String(DateTime datetime)
        {
            return datetime.ToString("o", CultureInfo.InvariantCulture);
        }
    }
}

