using System.Text.Json.Nodes;

namespace Monogame.Processing
{
    public class JSONObject
    {
        public JSONObject(JsonObject value) => Value = value;

        public JsonObject Value { get; }

        public static JSONObject Parse(string json) => new JSONObject(JsonNode.Parse(json)?.AsObject());

        public override string ToString() => Value?.ToJsonString();
    }
}
