namespace LocateSatellites.Dtos
{
    public class CoordinateDto
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public CoordinateDto(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
