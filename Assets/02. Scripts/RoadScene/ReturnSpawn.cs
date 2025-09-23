public static class ReturnSpawn
{
    private static string _nextKey; // 돌아올 때 사용할 스폰 키

    public static void Set(string key) => _nextKey = key;
    public static bool HasKey => !string.IsNullOrEmpty(_nextKey);
    public static string Consume()
    {
        var k = _nextKey;
        _nextKey = null; // 1회성
        return k;
    }
    public static void Clear() => _nextKey = null;
}
