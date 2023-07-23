using LocateSatellites.Dtos;
using System.Text.Json;
//using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Numerics;

namespace LocateSatellites.BusinesLogic
{
    public class WebsocketsConection
    {
        public async Task<List<Tuple<double, double, double>>?> ConecToserver()
        {
            ClientWebSocket ws = new ClientWebSocket();
            List<Tuple<double, double, double>> result = new List<Tuple<double, double, double>>();

            await ws.ConnectAsync(new Uri("ws://127.0.0.1:9001"), CancellationToken.None);
            byte[] buf = new byte[1056];
            try
            {
                if (ws.State == WebSocketState.Open)
                {
                    var receive = await ws.ReceiveAsync(buf, CancellationToken.None);

                    if (receive.MessageType == WebSocketMessageType.Close)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);

                        Console.WriteLine(receive.CloseStatusDescription);
                    }
                    else
                    {
                        string? json = Encoding.UTF8.GetString(buf, 0, receive.Count);
                        var parseData = JsonSerializer.Deserialize<List<Dictionary<string, CoordinateCtrlDto>>>(json);
                        //List<Dictionary<string, Dictionary<string, double>>> parseData = (List<Dictionary<string, Dictionary<string, double>>>)cdata; 
                        result = FillTuplesCoordinate(parseData);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepcion websocket cli:", ex.ToString());
            }

            return result;

        }

        List<Tuple<double, double, double>> FillTuplesCoordinate(List<Dictionary<string, CoordinateCtrlDto>> dataWs)
        {
            // Crear una lista de tuplas para almacenar los resultados
            List<Tuple<double, double, double>> tuplas = new List<Tuple<double, double, double>>();

            // Utilizar un bucle foreach para asignar los valores a las tuplas
            foreach (var listData in dataWs)
            {
                foreach (var item in listData)
                {
                    var x = item.Value.x;
                    var y = item.Value.y;
                    var z = item.Value.z;

                    var tupla = new Tuple<double, double, double>(x, y, z);
                    tuplas.Add(tupla);
                }
            }

            return tuplas;
        }

    }
}
