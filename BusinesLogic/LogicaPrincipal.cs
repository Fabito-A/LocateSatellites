using LocateSatellites.Dtos;
using Newtonsoft.Json;
using System.IO;

namespace LocateSatellites.BusinesLogic
{
    public class LogicaPrincipal
    {/*
        Tuple<double, double, double> Kenobi = Tuple.Create(-500.0, -200.0, 0.0);
        Tuple<double, double, double> Skywalker = Tuple.Create(100.0, -100.0, 0.0);
        Tuple<double, double, double> Sato = Tuple.Create(500.0, 100.0, 0.0);
        */
        Tuple<double, double, double> Kenobi;
        Tuple<double, double, double> Skywalker;
        Tuple<double, double, double> Sato;

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
            }

            return new List<string> { "not found" };
        }

        public List<SatelliteDto> CalcSatellite(List<CoordinateDataDto> data)
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
            LeerDatos leerTxt = new LeerDatos();    
            List<Tuple<double, double, double>> coordinates = leerTxt.ParseCoordinatesFromFile(filePath);

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

            //trilateracion
            CoordinateDto position = SatelliteTriangulation.TriangulatePosition(kenobyReference, skywalwerReference, satoReference);


            double kenobiDistance =  SatelliteTriangulation.CalculateDistance(pointReference, kenobyReference);
            double skywalkerDistance = SatelliteTriangulation.CalculateDistance(pointReference, skywalwerReference);
            double satoDistance = SatelliteTriangulation.CalculateDistance(pointReference, satoReference);
            double triangulate = SatelliteTriangulation.CalculateDistance(pointReference, position);

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
                    Distance = kenobiDistance,
                    Message = kenobiMessage
                });

                satellites.Add(new SatelliteDto
                {
                    Name = "Skywalker",
                    Distance = skywalkerDistance,
                    Message = skywalkerMessage
                });

                satellites.Add(new SatelliteDto
                {
                    Name = "Sato",
                    Distance = satoDistance,
                    Message = satoMessage
                });
            }

            return satellites;
        }

        public List<SatelliteDto> CalcDistance(List<CoordinateDataDto> data)
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

            LeerDatos leerTxt = new LeerDatos();
            List<Tuple<double, double, double>> coordinates = leerTxt.ParseCoordinatesFromFile(filePath);

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

            // CoordinateDto pointReference = new CoordinateDto(x, y, z);
            double kenobiDistance = SatelliteTriangulation.CalculateDistance(pointReference, kenobyReference);
            double skywalkerDistance = SatelliteTriangulation.CalculateDistance(pointReference, skywalwerReference);
            double satoDistance = SatelliteTriangulation.CalculateDistance(pointReference, satoReference);

            double triangulate = SatelliteTriangulation.CalculateDistance(pointReference, pointReference);

            List<string> kenobiMessage = GetMessage(kenobiDistance, triangulate, "Kenobi");
            List<string> skywalkerMessage = GetMessage(skywalkerDistance, triangulate, "Skywalker");
            List<string> satoMessage = GetMessage(satoDistance, triangulate, "Sato");

            satellites.Add(new SatelliteDto
            {
                Name = "Kenobi",
                Distance = Math.Round(kenobiDistance, 2),
                Message = kenobiMessage
            });

            satellites.Add(new SatelliteDto
            {
                Name = "Skywalker",
                Distance = Math.Round(skywalkerDistance, 2),
                Message = skywalkerMessage
            });

            satellites.Add(new SatelliteDto
            {
                Name = "Sato",
                Distance = Math.Round(satoDistance, 2),
                Message = satoMessage
            });

            return satellites;
        }

        public List<string> DecodeMessage(List<SatelliteDTO> data)
        {
            List<string> frase = new List<string>();
            List<string> decode = new List<string> { "este", "es", "un", "mensaje", "secreto" };
            List<string> mensaje = new List<string>();

            foreach (var objeto in data)
            {

                if (objeto.message is string)
                {
                    if (objeto.message != null)
                    {
                        mensaje.AddRange(objeto.message.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim()).Where(m => m != ""));
                    }
                }
                else
                {
                    mensaje.AddRange(objeto.message.Where(m => m != ""));
                }

                frase.AddRange(mensaje.Where(m => m != ""));
                mensaje.Clear();
            }

            if (decode.ToHashSet().SetEquals(frase.ToHashSet()))
            {
               // MessageOutput messageOutput = new MessageOutput();

                return decode.ToList();
            }
            else
            {
                return null;
            }
        }

        bool MarginValue(double valor2)
        {
            double margenMinimo = -50;
            double margenMaximo = 50;
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
    }
}
