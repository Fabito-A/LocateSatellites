namespace LocateSatellites.Controllers
{
    public class SatelliteDto
    {
        public string Name { get; set; }
        public Tuple<double, double, double>? Position { get; set; }
        public double Distance { get; set; }
        public List<string> Message { get; set; }
    }
}
