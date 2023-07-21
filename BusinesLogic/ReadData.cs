using System;
using System.Globalization;

namespace LocateSatellites.BusinesLogic
{
    public class ReadData
    {
        public List<Tuple<double, double, double>> ParseCoordinatesFromFile(string filePath)
        {
            List<Tuple<double, double, double>> coordinates = new List<Tuple<double, double, double>>();
            
            var c = CultureInfo.InvariantCulture;
            var style=NumberStyles.Float;
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (line.Contains("="))
                    {
                        string[] parts = line.Split(new char[] { '=', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 6)
                        {
                            double x, y, z;

                            if (double.TryParse(parts[2],style,c, out x) && double.TryParse(parts[4],style,c, out y) && double.TryParse(parts[6],style,c, out z))
                            {   

                                var tuple = new Tuple<double, double, double>(x, y, z);

                                coordinates.Add(tuple);

                            }
                            else
                            {
                                x = 0.0;
                                y = 0.0;
                                z = 0.0;
                                //var tuple = new Tuple<double, double, double>(x, y, z);
                                //coordinates.Add(tuple);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo: " + ex.Message);
            }

            return coordinates;
        }
    }

}
