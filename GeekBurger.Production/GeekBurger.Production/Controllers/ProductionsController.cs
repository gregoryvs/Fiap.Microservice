using GeekBurger.Productions.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekBurger.Productions.Controllers
{
    [ApiController]
    [Route("api/production")]
    public class ProductionsController : Controller
    {
        List<Production> Productions;

        public ProductionsController()
        {
            this.Productions = new List<Production>();

            this.Productions.Add(new Production() {
                ProductionId = new Guid("abcfa5f5-5af2-44c8-87a0-f58f3a3c6a08"),
                Restrictions = new List<string>() { "tomatoes", "potatoes", "onions" },
                On = true
            });

            this.Productions.Add(new Production()
            {
                ProductionId = new Guid("101dd3d3-8e85-4e22-a2c4-735f6b9163ee"),
                Restrictions = new List<string>() { "guten", "milk", "sugar" },
                On = true
            });


            this.Productions.Add(new Production()
            {
                ProductionId = new Guid("832d7db2-d1ca-4b03-85a4-f4d9a7df597e"),
                Restrictions = new List<string>() { "soy", "dairy", "gluten", "peanut" },
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
                Restrictions = new List<string>() { "soy", "dairy", "gluten", "peanut" },
                On = false
            };
        }
    }
}