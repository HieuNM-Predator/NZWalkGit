using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            // Get data from Database - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domain Models to DTOs
            //var regionsDto = new List<RegionDto>();   
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl,
            //    });
            //}
            
            // Map Domain Models to DTO user Mapper
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            // retrurn DTOs
            return Ok(regionsDto);
        }

        // GET SINGLE REGION (Get Region By ID)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            //var region = dbContext.Regions.Find(id);

            // Get Region Domain Model from Database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //Map/Convert Region Domain to Region DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl,
            //};


            // return DTO back to client
            //return Ok(regionDto);
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        // POST To create new region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        // Validate custom attribute
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Validation
            //if (ModelState.IsValid)
            //{
                // Map or Convert DTO to Domain Model
                //var regionDomainModel = new Region
                //{
                //    Code = addRegionRequestDto.Code,
                //    Name = addRegionRequestDto.Name,
                //    RegionImageUrl = addRegionRequestDto.RegionImageUrl,
                //};

                //Map by automapper
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                // Use Domain Model to Create Region
                //await dbContext.Regions.AddAsync(regionDomainModel);
                //await dbContext.SaveChangesAsync();
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                // Map Domain model back to Dto
                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};

                //Map by automapper
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetByID), new { id = regionDomainModel.Id }, regionDto);
            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}

        }

        // Update region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        // Validate custom attribute
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //if (ModelState.IsValid)
            //{
                // Map DTO to Domain Model
                //var regionDomainModel = new Region()
                //{
                //    Code = updateRegionRequestDto.Code,
                //    Name = updateRegionRequestDto.Name,
                //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
                //};

                //Map by automapper
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                // Check if region exists
                //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //// Map Dto to Domain Model
                //regionDomainModel.Code = updateRegionRequestDto.Code;
                //regionDomainModel.Name = updateRegionRequestDto.Name;
                //regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

                //await dbContext.SaveChangesAsync();


                // Convert Domain Model to Dto
                //var regionDto = new RegionDto
                //{
                //    Id = regionDomainModel.Id,
                //    Code = regionDomainModel.Code,
                //    Name = regionDomainModel.Name,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};

                //Map by automapper
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return Ok(regionDto);
            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}

        }

        // Delete region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null) { return NotFound(); }

            ////Delete region
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();

            //return deleted Region back
            // map Domain Model to Dto
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};

            //Map by automapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);

        }
    }
}
