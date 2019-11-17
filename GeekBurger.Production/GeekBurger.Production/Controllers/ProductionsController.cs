using AutoMapper;
using GeekBurger.Productions.Contracts;
using GeekBurger.Productions.Models;
using GeekBurger.Productions.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using GeekBurger.Productions.Helper;
using UnprocessableEntityResult = GeekBurger.Productions.Helper.UnprocessableEntityResult;

namespace GeekBurger.Productions.Controllers
{
    [ApiController]
    [Route("api/production")]
    public class ProductionsController : Controller
    {
        List<Production> Productions;
        private IProductionRepository _productionRepository;
        private IMapper _mapper;

        public ProductionsController(IProductionRepository productionRepository)
        {
            _productionRepository = productionRepository;

            this.Productions = new List<Production>();

            this.Productions.Add(new Production() {
                ProductionId = new Guid("abcfa5f5-5af2-44c8-87a0-f58f3a3c6a08"),
                //Restrictions = new List<string>() { "tomatoes", "potatoes", "onions" },
                Restrictions = "tomatoes " + "potatoes " + "onions",
                On = true
            });

            this.Productions.Add(new Production()
            {
                ProductionId = new Guid("101dd3d3-8e85-4e22-a2c4-735f6b9163ee"),
                //Restrictions = new List<string>() { "guten", "milk", "sugar" },
                Restrictions = "tomatoes " + "potatoes " + "onions",
                On = true
            });


            this.Productions.Add(new Production()
            {
                ProductionId = new Guid("832d7db2-d1ca-4b03-85a4-f4d9a7df597e"),
                //Restrictions = new List<string>() { "soy", "dairy", "gluten", "peanut" },
                Restrictions = "tomatoes " + "potatoes " + "onions",
                On = true
            });

        }

        [HttpGet("{guid}")]
        public IActionResult GetProductionByProductionin(Guid guid)
        {
            List<Production> productions = this.Productions.Where(p => p.ProductionId == guid).ToList();

            if (productions.Count == 0)
            {
                return NotFound();
            }

            return Ok(productions);
        }

        [HttpGet("")]
        public IActionResult GetAllProductions()
        {
            List<Production> productions = this.Productions.ToList();

            if (productions.Count == 0)
            {
                return NotFound();
            }

            return Ok(productions);
        }

        [HttpGet("areas")]
        public Production GetAreas()
        {
            var guid = new Guid("8048e9ec-80fe-4bad-bc2a-e4f4a75c834e");
            return new Models.Production
            {
                ProductionId = guid,
                //Restrictions = new List<string>() { "soy", "dairy", "gluten", "peanut" },
                Restrictions = "tomatoes " + "potatoes " + "onions",
                On = false
            };
        }

        [HttpPost()]
        public IActionResult AddProduction([FromBody] Production productionToAdd)
        {
            if (productionToAdd == null)
                return BadRequest();

            var production = productionToAdd;//_mapper.Map<Production>(productionToAdd);

            if (production.ProductionId == Guid.Empty)
                return new UnprocessableEntityResult(ModelState);

            _productionRepository.Add(production);
            _productionRepository.Save();

            var productionToGet = _mapper.Map<Production>(production);

            return CreatedAtRoute("GetProduction",
                new { id = productionToGet.ProductionId },
                productionToGet);
        }
    }
}