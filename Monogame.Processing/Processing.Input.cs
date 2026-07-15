using System.Diagnostics;
using System.IO;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Input Functions

        public string[] loadStrings(string path) => File.ReadAllLines(path);

        public byte[] loadBytes(string path) => File.ReadAllBytes(path);

        public StreamReader createReader(string path) => new StreamReader(path);

        public Stream createInput(string path) => File.OpenRead(path);

        public JSONObject parseJSONObject(string json) => JSONObject.Parse(json);

        public JSONArray parseJSONArray(string json) => JSONArray.Parse(json);

        public JSONObject loadJSONObject(string path) => parseJSONObject(File.ReadAllText(path));

        public JSONArray loadJSONArray(string path) => parseJSONArray(File.ReadAllText(path));

        public XML loadXML(string path) => XML.Parse(File.ReadAllText(path));

        public Table loadTable(string path) => Table.Parse(File.ReadAllText(path));

        public void launch(string pathOrUrl) => Process.Start(new ProcessStartInfo(pathOrUrl)
        {
            UseShellExecute = true
        });

        public string selectInput() => null;

        public string selectFolder() => null;

        #endregion
    }
}
