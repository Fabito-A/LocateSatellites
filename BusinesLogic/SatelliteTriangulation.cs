using LocateSatellites.Dtos;

namespace LocateSatellites.BusinesLogic
{
    public class SatelliteTriangulation
    {/*
        public static void Main()
        {
            // Coordenadas de los satélites
            Coordinate satellite1 = new Coordinate(1, 2, 3);
            Coordinate satellite2 = new Coordinate(4, 5, 6);
            Coordinate satellite3 = new Coordinate(7, 8, 9);

            // Triangulación de posición
            Coordinate position = TriangulatePosition(satellite1, satellite2, satellite3);

            Console.WriteLine($"La posición es: ({position.X}, {position.Y}, {position.Z})");
        }
        */
        public static CoordinateDto TriangulatePosition(CoordinateDto satellite1, CoordinateDto satellite2, CoordinateDto satellite3)
        {
            // Distancias entre los satélites y la posición desconocida
            double distance1 = CalculateDistance(satellite1, new CoordinateDto(0, 0, 0));
            double distance2 = CalculateDistance(satellite2, new CoordinateDto(0, 0, 0));
            double distance3 = CalculateDistance(satellite3, new CoordinateDto(0, 0, 0));

            // Triangulación de posición
            double x = TriangulateCoordinate(distance1, distance2, distance3, satellite1.X, satellite2.X, satellite3.X);
            double y = TriangulateCoordinate(distance1, distance2, distance3, satellite1.Y, satellite2.Y, satellite3.Y);
            double z = TriangulateCoordinate(distance1, distance2, distance3, satellite1.Z, satellite2.Z, satellite3.Z);

            return new CoordinateDto(x, y, z);
        }

        public static double CalculateDistance(CoordinateDto point1, CoordinateDto point2)
        {
            double result;
            // Cálculo de la distancia entre dos puntos en el espacio 3D
            double dx = point2.X - point1.X;
            double dy = point2.Y - point1.Y;
            double dz = point2.Z - point1.Z;
            double value = Math.Sqrt(dx * dx + dy * dy + dz * dz);

            if (double.IsNaN(value))
            {
                result = 0;
            }
            else 
            {
                result = value;            
            }

            return result;
        }

        public static double TriangulateCoordinate(double distance1, double distance2, double distance3, double coordinate1, double coordinate2, double coordinate3)
        {
            // Triangulación de una coordenada en base a las distancias y coordenadas de los satélites
            double result;
            // Se aplica el método de trilateración
            double A = 2 * (coordinate2 - coordinate1);
            double B = 2 * (coordinate2 - coordinate3);
            double C = (distance1 * distance1) - (distance2 * distance2) - (coordinate1 * coordinate1) + (coordinate2 * coordinate2);
            double D = (distance2 * distance2) - (distance3 * distance3) - (coordinate2 * coordinate2) + (coordinate3 * coordinate3);

            double x = (C * B - D * A) / (B * B - A * A);

            if (double.IsNaN(x))
            {
                result = 0;
            }
            else
            {
                result = x;
            }

            return result;
        }
    }
}
