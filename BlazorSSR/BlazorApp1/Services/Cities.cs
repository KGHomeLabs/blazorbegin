namespace BlazorApp1.Services
{
    public static class Cities
    {
        private static List<string> cities = new List<string>()
        {
            "Toronto",
            "Montreal",
            "Ottawa",
            "Calgary",
            "Halifax"
        };
        public static List<string> GetCities() => cities;
    }
}
