namespace diga.web.Helpers
{
    public static class ByteConverter
    {
        public static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public static long ConvertMegabytesToBytes(long megabytes)
        {
            return megabytes * 1024 * 1024;
        }
    }
}
