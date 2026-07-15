using System;
using System.Linq;

namespace Monogame.Processing
{
    public class Table
    {
        public Table(string[][] rows) => Rows = rows;

        public string[][] Rows { get; }

        public static Table Parse(string text) => new Table(
            text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(','))
                .ToArray());
    }
}
