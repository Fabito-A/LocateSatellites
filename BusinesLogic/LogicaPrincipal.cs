using LocateSatellites.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LocateSatellites.BusinesLogic
{
    public class LogicaPrincipal
    {/*
        Tuple<double, double, double> Kenobi = Tuple.Create(-500.0, -200.0, 0.0);
        Tuple<double, double, double> Skywalker = Tuple.Create(100.0, -100.0, 0.0);
        Tuple<double, double, double> Sato = Tuple.Create(500.0, 100.0, 0.0);
        */
        private Tuple<double, double, double> Kenobi;
        private Tuple<double, double, double> Skywalker;
        private Tuple<double, double, double> Sato;

        public List<string> GetMessage(double distance, double triangulate, string name)
        {

            bool compare = MarginValue(triangulate);

            if (name == "Kenobi")
            {
                if (compare == true)
                {
                    return new List<string> { "este", "", "", "mensaje", "" };
                }
                else if (compare == false)
                {
                    return new List<string> { "xxx", "xxxx", "Error", "xxxx", "xxx" };
                }
            }
            else if (name == "Skywalker")
            {
                if (compare == true)
                {
                    return new List<string> { "este", "es", "", "", "secreto" };
                }
                else if (distance <= 133.042 || distance <= 23)
                {
                    return new List<string> { "este", "es", "un", "", "secreto" };
                }
                else
                {
                    return new List<string> { "xxx", "xxxx", "Error", "xxxx", "xxx" };
                }
            }
            else if (name == "Sato")
            {
                if (compare == true)
                {
                    return new List<string> { "este", "", "un", "", "" };
                }
                else if (distance <= 300 || distance <= 57.75)
                {
                    return new List<string> { "este", "", "un", "", "secreto" };
                }
                else 
                {
                    return new List<string> { "xxx", "xxxx", "Error", "xxxx", "xxx" };
                }
            }

            return new List<string> { "not found" };
        }

        public async Task<List<SatelliteDto>> CalcSatellite(List<CoordinateDataDto> data)
        {
            CoordinateCtrlDto coord = new CoordinateCtrlDto();

            foreach (CoordinateDataDto values in data)
            {
                coord.x = values.coordinate[0].x;
                coord.y = values.coordinate[0].y;
                coord.z = values.coordinate[0].z;
            }
            //bool continueLogic;
            List<SatelliteDto> satellites = new();
            WebsocketsConection clientWsockect = new();
            List<Tuple<double, double, double>>? dataWs = new();

            // conectar al servidor websocket
            dataWs = await clientWsockect.ConecToserver();

            if (dataWs.Count > 0) 
            {
                int cont = 0;
                //continuar el codigo 
                foreach (var item in dataWs) 
                {
                    cont++;
                    if (cont == 1){Kenobi = item;}
                    if (cont == 2){Skywalker = item;}
                    if (cont == 3){Sato = item;}
                }
                CoordinateDto pointReference = new CoordinateDto(coord.x, coord.y, coord.z);
                CoordinateDto kenobyReference = new CoordinateDto(Kenobi.Item1, Kenobi.Item2, Kenobi.Item3);
                CoordinateDto skywalwerReference = new CoordinateDto(Skywalker.Item1, Skywalker.Item2, Skywalker.Item3);
                CoordinateDto satoReference = new CoordinateDto(Sato.Item1, Sato.Item2, Sato.Item3);

                //trilateracion
                CoordinateDto position = SatelliteTriangulation.TriangulatePosition(kenobyReference, skywalwerReference, satoReference);


                double kenobiDistance = SatelliteTriangulation.CalculateDistance(pointReference, kenobyReference);
                double skywalkerDistance = SatelliteTriangulation.CalculateDistance(pointReference, skywalwerReference);
                double satoDistance = SatelliteTriangulation.CalculateDistance(pointReference, satoReference);
                double triangulate = SatelliteTriangulation.CalculateDistance(pointReference, position);
                Console.WriteLine("TRIANGULACION x: {0:0.00}", position.X + "y:" + position.Y + "z:" + position.Z);
                // Triangulación de posición

                List<string> kenobiMessage = GetMessage(kenobiDistance, triangulate, "Kenobi");
                List<string> skywalkerMessage = GetMessage(skywalkerDistance, triangulate, "Skywalker");
                List<string> satoMessage = GetMessage(satoDistance, triangulate, "Sato");

                //double[] coordinates = CoordinatesTransform.Trilateration(kenobiDistance, skywalkerDistance, satoDistance, Kenobi, Skywalker, Sato);

                if (position != null)
                {


                    satellites.Add(new SatelliteDto
                    {
                        Name = "Kenobi",
                        Distance = Math.Round(kenobiDistance, 3),
                        Message = kenobiMessage
                    });

                    satellites.Add(new SatelliteDto
                    {
                        Name = "Skywalker",
                        Distance = Math.Round(skywalkerDistance, 3),
                        Message = skywalkerMessage
                    });

                    satellites.Add(new SatelliteDto
                    {
                        Name = "Sato",
                        Distance = Math.Round(satoDistance, 3),
                        Message = satoMessage
                    });
                }
            }
            else
            {
                satellites.Add(new SatelliteDto
                {
                    Name = "ERROR",
                    Distance = 0.0,
                    Message = new List<string> { "Error Api comunication" }
                });
            }
            

            return satellites;
        }

        public List<SatelliteDto> CalcDistance(string satelliteName,List<CoordinateDataDto> data)
        {
            CoordinateCtrlDto coord = new CoordinateCtrlDto();

            foreach (CoordinateDataDto values in data)
            {
                coord.x = values.coordinate[0].x;
                coord.y = values.coordinate[0].y;
                coord.z = values.coordinate[0].z;
            }

            //cambiar el directorio 
            string filePath = @"C:\Users\Fabio\Documents\PruebasDesarrollo\SatelliteLocation\LocateSatellites\Files\Satellites.txt";

            ReadData leerTxt = new ReadData();
            List<Tuple<double, double, double>> coordinates = leerTxt.ParseCoordinatesFromFile(filePath);
            double satelliteDistance = 0.0;
            List<string> MessageSatellite =new List<string>();

            if (coordinates.Count >= 1)
                Kenobi = coordinates[0];
            if (coordinates.Count >= 2)
                Skywalker = coordinates[1];
            if (coordinates.Count >= 3)
                Sato = coordinates[2];

            List<SatelliteDto> satellites = new List<SatelliteDto>();
            CoordinateDto pointReference = new CoordinateDto(coord.x, coord.y, coord.z);
            CoordinateDto kenobyReference = new CoordinateDto(Kenobi.Item1, Kenobi.Item2, Kenobi.Item3);
            CoordinateDto skywalwerReference = new CoordinateDto(Skywalker.Item1, Skywalker.Item2, Skywalker.Item3);
            CoordinateDto satoReference = new CoordinateDto(Sato.Item1, Sato.Item2, Sato.Item3);
            double triangulate = SatelliteTriangulation.CalculateDistance(pointReference, pointReference);

            if (satelliteName == "Kenobi")
            {
                satelliteDistance = SatelliteTriangulation.CalculateDistance(pointReference, kenobyReference);
                MessageSatellite = GetMessage(satelliteDistance, triangulate, "Kenobi");
            }
            else if (satelliteName == "Skywalker")
            {
                satelliteDistance = SatelliteTriangulation.CalculateDistance(pointReference, skywalwerReference);
                MessageSatellite = GetMessage(satelliteDistance, triangulate, "Skywalker");
            }
            else if
            (satelliteName == "Sato")
            {
                satelliteDistance = SatelliteTriangulation.CalculateDistance(pointReference, satoReference);
                MessageSatellite = GetMessage(satelliteDistance, triangulate, "Sato");
            }
            else 
            {
                satelliteDistance = 0;
                MessageSatellite.Add("Satellite no existe");

            }

            satellites.Add(new SatelliteDto { Name = satelliteName, Distance = Math.Round(satelliteDistance, 2), Message = MessageSatellite });

            return satellites;
        }

        public List<string> DecodeMessage(List<SatelliteDTO> data)
        {
            List<string> frase = new List<string>();
            List<string> decode = new List<string> { "este", "es", "un", "mensaje", "secreto" };
            List<string> mensaje = new List<string>();

            foreach (var objeto in data)
            {
                //mensaje.AddRange(objeto.message.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim()).Where(m => m != ""));
                mensaje.AddRange(objeto.message.Where(m => m != ""));
                foreach (var palabra in mensaje)
                {
                    if (!frase.Contains(palabra))
                    {
                        frase.Add(palabra);
                    }
                   
                }
                mensaje.Clear();
                
     
            }

            if (decode.ToHashSet().SetEquals(frase.ToHashSet()))
            {
               // MessageOutput messageOutput = new MessageOutput();

                return decode.ToList();
            }
            else
            {
                List<string> msj = new List<string> { "No encontrado" };
                return msj;
            }
        }

        bool MarginValue(double valor2)
        {
            double margenMinimo = -100;
            double margenMaximo = 100;
            bool result;

            double resultado = valor2;

            if (resultado >= margenMinimo && resultado <= margenMaximo)
            {
                result = true;
            }
            else
            {

                result = false;
            }

            return result;
        }
        /*
        bool FillTuplesCoordinate(List<Dictionary<string, double>>? dataWs) 
        {
            bool result = false;
            int count = 0;

            if (dataWs.Count > 0)
            {
                foreach (var coord in dataWs)
                {
                    count++;
                    if (count == 1)
                    {
                        Kenobi = Tuple.Create(coord["x"], coord["y"], coord["z"]);
                    }
                    if (count == 2)
                    {
                        Skywalker = Tuple.Create(coord["x"], coord["y"], coord["z"]);
                    }
                    if (count == 3)
                    {
                        Sato = Tuple.Create(coord["x"], coord["y"], coord["z"]);
                    }

                }
            }
            else 
            {
                Kenobi = Tuple.Create(0.0, 0.0, 0.0);
                Skywalker = Tuple.Create(0.0, 0.0, 0.0);
                Sato = Tuple.Create(0.0, 0.0, 0.0);

            }
            return result;
        }

        */
    }
}
