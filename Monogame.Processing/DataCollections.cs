using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace Monogame.Processing
{
    public class IntList
    {
        private readonly List<int> _items;
        public IntList() => _items = new List<int>();
        public IntList(IEnumerable<int> items) => _items = new List<int>(items);
        public int size() => _items.Count;
        public int get(int index) => _items[index];
        public void set(int index, int value) => _items[index] = value;
        public void append(int value) => _items.Add(value);
        public int remove(int index) { var value = _items[index]; _items.RemoveAt(index); return value; }
        public void clear() => _items.Clear();
        public bool hasValue(int value) => _items.Contains(value);
        public int[] values() => _items.ToArray();
        public List<int> toList() => new List<int>(_items);
    }

    public class FloatList
    {
        private readonly List<float> _items;
        public FloatList() => _items = new List<float>();
        public FloatList(IEnumerable<float> items) => _items = new List<float>(items);
        public int size() => _items.Count;
        public float get(int index) => _items[index];
        public void set(int index, float value) => _items[index] = value;
        public void append(float value) => _items.Add(value);
        public float remove(int index) { var value = _items[index]; _items.RemoveAt(index); return value; }
        public void clear() => _items.Clear();
        public bool hasValue(float value) => _items.Contains(value);
        public float[] values() => _items.ToArray();
        public List<float> toList() => new List<float>(_items);
    }

    public class StringList
    {
        private readonly List<string> _items;
        public StringList() => _items = new List<string>();
        public StringList(IEnumerable<string> items) => _items = new List<string>(items);
        public int size() => _items.Count;
        public string get(int index) => _items[index];
        public void set(int index, string value) => _items[index] = value;
        public void append(string value) => _items.Add(value);
        public string remove(int index) { var value = _items[index]; _items.RemoveAt(index); return value; }
        public void clear() => _items.Clear();
        public bool hasValue(string value) => _items.Contains(value);
        public string[] values() => _items.ToArray();
        public List<string> toList() => new List<string>(_items);
    }

    public class IntDict
    {
        private readonly Dictionary<string, int> _items;
        public IntDict() => _items = new Dictionary<string, int>();
        public IntDict(IDictionary<string, int> items) => _items = new Dictionary<string, int>(items);
        public int size() => _items.Count;
        public int get(string key) => _items[key];
        public int get(string key, int defaultValue) => _items.TryGetValue(key, out var value) ? value : defaultValue;
        public void set(string key, int value) => _items[key] = value;
        public void add(string key, int amount) => _items[key] = get(key, 0) + amount;
        public int remove(string key) { var value = _items[key]; _items.Remove(key); return value; }
        public bool hasKey(string key) => _items.ContainsKey(key);
        public string[] keys() => _items.Keys.ToArray();
        public int[] values() => _items.Values.ToArray();
        public void clear() => _items.Clear();
    }

    public class FloatDict
    {
        private readonly Dictionary<string, float> _items;
        public FloatDict() => _items = new Dictionary<string, float>();
        public FloatDict(IDictionary<string, float> items) => _items = new Dictionary<string, float>(items);
        public int size() => _items.Count;
        public float get(string key) => _items[key];
        public float get(string key, float defaultValue) => _items.TryGetValue(key, out var value) ? value : defaultValue;
        public void set(string key, float value) => _items[key] = value;
        public void add(string key, float amount) => _items[key] = get(key, 0) + amount;
        public float remove(string key) { var value = _items[key]; _items.Remove(key); return value; }
        public bool hasKey(string key) => _items.ContainsKey(key);
        public string[] keys() => _items.Keys.ToArray();
        public float[] values() => _items.Values.ToArray();
        public void clear() => _items.Clear();
    }

    public class StringDict
    {
        private readonly Dictionary<string, string> _items;
        public StringDict() => _items = new Dictionary<string, string>();
        public StringDict(IDictionary<string, string> items) => _items = new Dictionary<string, string>(items);
        public int size() => _items.Count;
        public string get(string key) => _items[key];
        public string get(string key, string defaultValue) => _items.TryGetValue(key, out var value) ? value : defaultValue;
        public void set(string key, string value) => _items[key] = value;
        public string remove(string key) { var value = _items[key]; _items.Remove(key); return value; }
        public bool hasKey(string key) => _items.ContainsKey(key);
        public string[] keys() => _items.Keys.ToArray();
        public string[] values() => _items.Values.ToArray();
        public void clear() => _items.Clear();
    }

    public class Table
    {
        private readonly List<string> _columns = new List<string>();
        private readonly List<TableRow> _rows = new List<TableRow>();

        public static Table Parse(string text)
        {
            var table = new Table();
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) return table;

            var headers = lines[0].Split(',');
            foreach (var header in headers) table.addColumn(header);

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                var row = table.addRow();
                for (var i = 0; i < values.Length && i < headers.Length; i++)
                {
                    row.setString(headers[i], values[i]);
                }
            }

            return table;
        }

        public int getColumnCount() => _columns.Count;
        public int getRowCount() => _rows.Count;
        public string[] getColumnTitles() => _columns.ToArray();
        public void addColumn(string title) { if (!_columns.Contains(title)) _columns.Add(title); foreach (var row in _rows) row.EnsureColumn(title); }
        public TableRow addRow() { var row = new TableRow(this); _rows.Add(row); return row; }
        public TableRow getRow(int index) => _rows[index];
        public TableRow removeRow(int index) { var row = _rows[index]; _rows.RemoveAt(index); return row; }
        public IEnumerable<TableRow> rows() => _rows.ToArray();
        internal IReadOnlyList<string> Columns => _columns;
        internal void EnsureColumn(string title) => addColumn(title);
    }

    public class TableRow
    {
        private readonly Table _table;
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();
        internal TableRow(Table table) { _table = table; foreach (var column in table.Columns) _values[column] = string.Empty; }
        internal void EnsureColumn(string title) { if (!_values.ContainsKey(title)) _values[title] = string.Empty; }
        public string getString(string column) => _values.TryGetValue(column, out var value) ? value : string.Empty;
        public int getInt(string column) => int.Parse(getString(column), CultureInfo.InvariantCulture);
        public float getFloat(string column) => float.Parse(getString(column), CultureInfo.InvariantCulture);
        public void setString(string column, string value) { _table.EnsureColumn(column); _values[column] = value; }
        public void setInt(string column, int value) => setString(column, value.ToString(CultureInfo.InvariantCulture));
        public void setFloat(string column, float value) => setString(column, value.ToString(CultureInfo.InvariantCulture));
        public bool hasColumn(string column) => _values.ContainsKey(column);
        public string[] columns() => _table.Columns.ToArray();
    }

    public class JSONArray
    {
        private readonly JsonArray _array;
        public JSONArray() => _array = new JsonArray();
        public JSONArray(string json) => _array = JsonNode.Parse(json)?.AsArray() ?? new JsonArray();
        internal JSONArray(JsonArray array) => _array = array;
        internal JsonArray Node => _array;
        public static JSONArray Parse(string json) => new JSONArray(json);
        public int size() => _array.Count;
        public void append(int value) => _array.Add(value);
        public void append(float value) => _array.Add(value);
        public void append(string value) => _array.Add(value);
        public void append(bool value) => _array.Add(value);
        public void append(JSONObject value) => _array.Add(value?.Node.DeepClone());
        public void append(JSONArray value) => _array.Add(value?._array.DeepClone());
        public JsonNode get(int index) => _array[index];
        public int getInt(int index) => (int)_array[index];
        public float getFloat(int index) => (float)_array[index];
        public string getString(int index) => (string)_array[index];
        public JSONObject getJSONObject(int index) => new JSONObject(_array[index].AsObject());
        public JSONArray getJSONArray(int index) => new JSONArray(_array[index].AsArray());
        public void set(int index, int value) => _array[index] = value;
        public void set(int index, float value) => _array[index] = value;
        public void set(int index, string value) => _array[index] = value;
        public void remove(int index) => _array.RemoveAt(index);
        public override string ToString() => _array.ToJsonString();
    }

    public class JSONObject
    {
        private readonly JsonObject _object;
        public JSONObject() => _object = new JsonObject();
        public JSONObject(string json) => _object = JsonNode.Parse(json)?.AsObject() ?? new JsonObject();
        internal JSONObject(JsonObject obj) => _object = obj;
        internal JsonObject Node => _object;
        public static JSONObject Parse(string json) => new JSONObject(json);
        public int size() => _object.Count;
        public bool hasKey(string key) => _object.ContainsKey(key);
        public string[] keys() => _object.Select(p => p.Key).ToArray();
        public JsonNode get(string key) => _object[key];
        public int getInt(string key) => (int)_object[key];
        public float getFloat(string key) => (float)_object[key];
        public string getString(string key) => (string)_object[key];
        public bool getBoolean(string key) => (bool)_object[key];
        public JSONObject getJSONObject(string key) => new JSONObject(_object[key].AsObject());
        public JSONArray getJSONArray(string key) => new JSONArray(_object[key].AsArray());
        public void setInt(string key, int value) => _object[key] = value;
        public void setFloat(string key, float value) => _object[key] = value;
        public void setString(string key, string value) => _object[key] = value;
        public void setBoolean(string key, bool value) => _object[key] = value;
        public void setJSONObject(string key, JSONObject value) => _object[key] = value?.Node.DeepClone();
        public void setJSONArray(string key, JSONArray value) => _object[key] = value?.Node.DeepClone();
        public void remove(string key) => _object.Remove(key);
        public override string ToString() => _object.ToJsonString();
    }

    public class XML
    {
        private readonly XElement _element;
        public XML(string name) => _element = new XElement(name);
        internal XML(XElement element) => _element = element;
        public static XML parse(string xml) => new XML(XElement.Parse(xml));
        public static XML Parse(string xml) => parse(xml);
        public string getName() => _element.Name.LocalName;
        public string getContent() => _element.Value;
        public void setContent(string content) => _element.Value = content;
        public string getString(string name) => (string)_element.Attribute(name);
        public void setString(string name, string value) => _element.SetAttributeValue(name, value);
        public int getInt(string name) => int.Parse(getString(name), CultureInfo.InvariantCulture);
        public void setInt(string name, int value) => setString(name, value.ToString(CultureInfo.InvariantCulture));
        public float getFloat(string name) => float.Parse(getString(name), CultureInfo.InvariantCulture);
        public void setFloat(string name, float value) => setString(name, value.ToString(CultureInfo.InvariantCulture));
        public bool hasAttribute(string name) => _element.Attribute(name) != null;
        public XML addChild(string name) { var child = new XElement(name); _element.Add(child); return new XML(child); }
        public XML getChild(string name) { var child = _element.Element(name); return child == null ? null : new XML(child); }
        public XML[] getChildren(string name) => _element.Elements(name).Select(e => new XML(e)).ToArray();
        public XML[] getChildren() => _element.Elements().Select(e => new XML(e)).ToArray();
        public override string ToString() => _element.ToString(SaveOptions.DisableFormatting);
    }
}
