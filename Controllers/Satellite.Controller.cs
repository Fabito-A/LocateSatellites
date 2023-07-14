using LocateSatellites.BusinesLogic;
using LocateSatellites.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LocateSatellites.Controllers
{
    public class ApiController : ControllerBase
    {
        [HttpPost]
        [Route("/api/sendCoord")]
        public IActionResult GetCoordinate(List<CoordinateCtrlDto> coordinate)
        {
            LogicaPrincipal calcsatellite =new LogicaPrincipal();  
            var response = calcsatellite.CalcSatellite(coordinate);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("/api/getDistance")]
        public IActionResult GetDistance(List<CoordinateCtrlDto> coordinate)
        {   
            LogicaPrincipal logicaPrincipal = new LogicaPrincipal();
            var response = logicaPrincipal.CalcDistance(coordinate);
            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("/api/topsecret")]
        public IActionResult ProcesarArchivo(List<SatelliteDTO> satellites)
        {
            var valor = satellites.ToList();
            LogicaPrincipal logicaPrincipal = new LogicaPrincipal();
            var response = logicaPrincipal.DecodeMessage(satellites);

            if (response != null)
                return Ok(JsonConvert.SerializeObject(response));
            else
                return NotFound();
        }
    }
}
