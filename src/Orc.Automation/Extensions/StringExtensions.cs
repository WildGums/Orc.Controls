namespace Orc.Automation
{
    using System.IO;

    public static class StringExtensions
    {
        public static Stream ToStream(this string s)
        {
            var stream = new MemoryStream();
#pragma warning disable IDISP001 // Dispose created.
            var writer = new StreamWriter(stream);
#pragma warning restore IDISP001 // Dispose created.
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
