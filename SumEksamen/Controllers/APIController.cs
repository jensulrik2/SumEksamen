using Microsoft.AspNetCore.Mvc;
using SumEksamen.Models;
using SumEksamen.Services;

namespace SumEksamen.Controllers;

[Route("api/[controller]")]
[ApiController]
public class APIController : ControllerBase
{
    public List<Venteliste> HentVentelister()
    {
        return Storage.HentVentelister();
    }
}