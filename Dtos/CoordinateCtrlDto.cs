namespace LocateSatellites.Dtos
{
    public class CoordinateCtrlDto
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    }

    public class CoordinateDataDto
    {
        public List<CoordinateCtrlDto>? coordinate { get; set; }
    }

}