namespace EndlessRunner.Events
{
    public static class CustomData
    {
        public static object Build(object payload) { return payload; }
        public static object Build(string key, object value) { return new { key, value }; }
    }
}
