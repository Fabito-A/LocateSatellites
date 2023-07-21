using LocateSatellites.Dtos;
using System.Net.WebSockets;
using System.Text;

namespace LocateSatellites.BusinesLogic
{
    public class WebsocketsConection
    {
        public async Task<CoordinateDataDto?> ConecToserver()
        {
            ClientWebSocket ws = new ClientWebSocket();
            CoordinateDataDto? data = new ();

            await ws.ConnectAsync(new Uri("ws://127.0.0.1:9001"), CancellationToken.None);
            byte[] buf = new byte[1056];

            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buf, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);

                    Console.WriteLine(result.CloseStatusDescription);
                }
                else
                {
                    var json = Encoding.UTF8.GetString(buf, 0, result.Count);
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject<CoordinateDataDto>(json);

                    Console.WriteLine(Encoding.ASCII.GetString(buf, 0, result.Count));
                }
            }

            return data;   

        }

    }
}
