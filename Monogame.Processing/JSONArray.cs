using System.Text.Json.Nodes;

namespace Monogame.Processing
{
    public class JSONArray
    {
        public JSONArray(JsonArray value) => Value = value;

        public JsonArray Value { get; }

        public static JSONArray Parse(string json) => new JSONArray(JsonNode.Parse(json)?.AsArray());

        public override string ToString() => Value?.ToJsonString();
    }
}
