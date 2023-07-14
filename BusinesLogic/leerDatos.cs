namespace LocateSatellites.BusinesLogic
{
    public class leerDatos
    {
        public List<Tuple<double, double, double>> ParseCoordinatesFromFile(string filePath)
        {
            List<Tuple<double, double, double>> coordinates = new List<Tuple<double, double, double>>();

            coordinates.ToArray();

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    if (line.Contains("="))
                    {
                        string[] parts = line.Split(new char[] { '=', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 7)
                        {
                            double x, y, z;

                            if (double.TryParse(parts[2], out x) && double.TryParse(parts[4], out y))
                            {
                                if (parts.Length >= 6)
                                {
                                    if (double.TryParse(parts[6], out z))
                                    {
                                        coordinates.Add(Tuple.Create(x, y, z));
                                    }
                                    else
                                    {
                                        z = 0.0;
                                        Console.WriteLine("Error al convertir la coordenada Z en línea: " + line);
                                    }
                                }
                                else
                                {
                                    z = 0.0;
                                    coordinates.Add(Tuple.Create(x, y, z));
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error al convertir las coordenadas en línea: " + line);
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
