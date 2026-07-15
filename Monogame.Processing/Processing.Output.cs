using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Monogame.Processing
{
    public class PrintWriter : IDisposable
    {
        private readonly TextWriter _writer;

        public PrintWriter(TextWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public PrintWriter(Stream stream)
            : this(new StreamWriter(stream ?? throw new ArgumentNullException(nameof(stream))))
        {
        }

        public Encoding Encoding => _writer.Encoding;

        public void print(object value) => _writer.Write(value);
        public void println() => _writer.WriteLine();
        public void println(object value) => _writer.WriteLine(value);
        public void printf(string format, params object[] args) => _writer.Write(format, args);
        public void flush() => _writer.Flush();
        public void close() => _writer.Close();
        public void Dispose() => _writer.Dispose();
    }

    public abstract partial class Processing
    {
        public FileStream createOutput(string path)
        {
            EnsureOutputDirectory(path);
            return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        }

        public StreamWriter createWriter(string path) => new StreamWriter(createOutput(path));

        public void saveBytes(string path, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using var output = createOutput(path);
            output.Write(data, 0, data.Length);
        }

        public void saveStrings(string path, string[] lines)
        {
            if (lines == null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            EnsureOutputDirectory(path);
            File.WriteAllLines(path, lines);
        }

        public void saveStream(string target, Stream source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            using var output = createOutput(target);
            source.CopyTo(output);
        }

        public void saveJSONObject(JSONObject json, string path) => saveStrings(path, new[] { json?.ToString() ?? string.Empty });
        public void saveJSONArray(JSONArray json, string path) => saveStrings(path, new[] { json?.ToString() ?? string.Empty });
        public void saveXML(XML xml, string path) => saveStrings(path, new[] { xml?.ToString() ?? string.Empty });

        public void saveTable(Table table, string path)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var columns = table.getColumnTitles();
            var lines = new[] { string.Join(",", columns.Select(EscapeCsv)) }
                .Concat(table.rows().Select(row => string.Join(",", columns.Select(column => EscapeCsv(row.getString(column))))))
                .ToArray();

            saveStrings(path, lines);
        }

        /// <summary>
        /// Recording vector/offscreen output is not available yet because this implementation does not expose
        /// a PGraphics-compatible renderer or an exportable offscreen graphics target.
        /// </summary>
        public virtual void beginRecord(params object[] args) => throw new NotSupportedException("beginRecord requires PGraphics/offscreen export support, which is not implemented yet.");

        /// <summary>
        /// Completes a recording started with beginRecord. This is reserved for future PGraphics/offscreen export support.
        /// </summary>
        public virtual void endRecord() => throw new NotSupportedException("endRecord requires PGraphics/offscreen export support, which is not implemented yet.");

        /// <summary>
        /// Raw output capture is not available yet because this implementation does not expose an exportable raw renderer.
        /// </summary>
        public virtual void beginRaw(params object[] args) => throw new NotSupportedException("beginRaw requires raw renderer export support, which is not implemented yet.");

        /// <summary>
        /// Completes raw output capture started with beginRaw. This is reserved for future renderer export support.
        /// </summary>
        public virtual void endRaw() => throw new NotSupportedException("endRaw requires raw renderer export support, which is not implemented yet.");

        private static void EnsureOutputDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Output path cannot be empty.", nameof(path));
            }

            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static string EscapeCsv(string value)
        {
            value ??= string.Empty;
            return value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0
                ? $"\"{value.Replace("\"", "\"\"")}\""
                : value;
        }
    }
}
