using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controller;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly AuctionDbContext context;
    private readonly IMapper mapper;

    public AuctionController(AuctionDbContext context, IMapper mapper)
{
        this.context = context;
        this.mapper = mapper;
    }
[HttpGet]
public async Task<ActionResult<List<AuctionDTO>>> GetAllActions(string date)
{

    var query = context.Auctions.OrderBy(x=>x.Item.Make).AsQueryable();

    if(!string.IsNullOrEmpty(date)){
        query = query.Where(x=>x.UpdateAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
    }

return await query.ProjectTo<AuctionDTO>(mapper.ConfigurationProvider).ToListAsync();
}
[HttpGet("{id}")]
public async Task<ActionResult<AuctionDTO>> GetAuctionById(Guid id)
{
    var auction= await context.Auctions
    .Include(x=>x.Item)
    .FirstOrDefaultAsync(x=>x.Id==id);

    if(auction==null) return NotFound();

    return mapper.Map<AuctionDTO>(auction);
}
[HttpPost]
public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDto auctionDto)
{
var auction=mapper.Map<Auction>(auctionDto);
auction.Seller="test";
context.Auctions.Add(auction);
var result = await context.SaveChangesAsync() > 0;
if(!result) return BadRequest("Could not save changes to thr DB");

return CreatedAtAction(nameof(GetAuctionById),new {auction.Id},mapper.Map<AuctionDTO>(auction));
}

[HttpPut("{id}")]
public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto auctionDto){
    var auction = await context.Auctions
    .Include(x=>x.Item)
    .FirstOrDefaultAsync(x=>x.Id==id);

    if(auction == null)return NotFound();   

    auction.Item.Make=auctionDto.Make ?? auction.Item.Make;
    auction.Item.Model=auctionDto.Model ?? auction.Item.Model;
    auction.Item.Color=auctionDto.Color ?? auction.Item.Color;
    auction.Item.Mileage=auctionDto.Mileage ?? auction.Item.Mileage;
    auction.Item.Year=auctionDto.Year ?? auction.Item.Year;

    var result = await context.SaveChangesAsync() > 0;

    if(result) return Ok();

    return BadRequest("Problem saving changes");
}

[HttpDelete("{id}")]

public async Task<ActionResult> DeleteAuction(Guid id)
{
var auction = await context.Auctions.FindAsync(id);
if (auction == null)return NotFound();

context.Auctions.Remove(auction);

var result=await context.SaveChangesAsync()>0;

if(result)return Ok();
return BadRequest("Could not update DB");
}
}