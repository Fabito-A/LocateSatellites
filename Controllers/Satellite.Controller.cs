using LocateSatellites.BusinesLogic;
using LocateSatellites.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LocateSatellites.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        [HttpPost]
        [Route("/sendCoord")]
        public IActionResult GetCoordinate(List<CoordinateDataDto> coordinate)
        {
            LogicaPrincipal calcsatellite =new LogicaPrincipal();  
            var response = calcsatellite.CalcSatellite(coordinate);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("/getDistance")]
        public IActionResult GetDistance(List<CoordinateDataDto> coordinate)
        {   
            LogicaPrincipal logicaPrincipal = new LogicaPrincipal();
            var response = logicaPrincipal.CalcDistance(coordinate);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("/topsecret")]
        public IActionResult ProcesarArchivo(List<SatelliteDTO> satellites)
        {
            LogicaPrincipal logicaPrincipal = new LogicaPrincipal();
            var response = logicaPrincipal.DecodeMessage(satellites);

            if (response != null)
                return Ok(JsonConvert.SerializeObject(response));
            else
                return NotFound();
        }
    }
}
