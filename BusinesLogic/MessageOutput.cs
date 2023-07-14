using Newtonsoft.Json.Linq;

namespace LocateSatellites.BusinesLogic
{
    public class MessageOutput
    {
        public object GetRMessage()
        {
            JObject objJson = new JObject
            {
                ["position"] = new JObject
                {
                    ["x"] = -100,
                    ["y"] = 75.5
                },
                ["message"] = "ESTE ES UN MENSAJE SECRETO"
            };

            return objJson;
        }
    }
}
