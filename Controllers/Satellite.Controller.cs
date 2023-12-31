﻿using LocateSatellites.BusinesLogic;
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
        public async Task<IActionResult> GetCoordinate(List<CoordinateDataDto> coordinate)
        {
            LogicaPrincipal calcsatellite =new LogicaPrincipal();
            var response = await calcsatellite.CalcSatellite(coordinate);
            return Ok(JsonConvert.SerializeObject(response));
        }
        [HttpPost]
        [Route("/topsecret_split")]
        public IActionResult ProcesarArchivo(List<SatelliteDTO> data)
        {
            LogicaPrincipal logicaPrincipal = new LogicaPrincipal();
            var response = logicaPrincipal.DecodeMessage(data);

            if (response != null)
                return Ok(JsonConvert.SerializeObject(response));
            else
                return NotFound();
        }

        [HttpPost("{Satellite}")]
        public IActionResult GetDistance(string Satellite,List<CoordinateDataDto> coordinate)
        {
            LogicaPrincipal logicaPrincipal = new LogicaPrincipal();
            var response = logicaPrincipal.CalcDistance(Satellite,coordinate);
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
