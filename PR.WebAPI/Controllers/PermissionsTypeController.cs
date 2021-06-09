using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PR.Data;
using PR.Entity;
using PR.Entity.Models;
using PR.Service;
using PR.WebAPI.DTOs;

namespace PR.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsTypeController : ControllerBase
    {
        private readonly ModelContext _context;
        public PermissionsTypeController(ModelContext context)
        {
            if (_context == null)
                _context = context;
        }

        [Route("~/[controller]/[action]")]
        [HttpGet]
        //[Authorize]
        //[Permission]
        public async Task<object> GetPermissionsType()
        {
            GenericResult<object> _response = new GenericResult<object>();
            try
            {
                var serviceCrew = new Service<TypePermission>(_context).GetRepository();
                
                var query = serviceCrew.ListAsync(null,
                                                null,null).Result;
                if (query != null)
                {
                    var listPermissionsType = query.Select(x => new PermissionTypeDTO
                    {
                        id = x.Id,
                        description = x.Description
                    });
                    _response.IsValid = true;
                    _response.GenericObject = listPermissionsType;
                }
                else
                {
                    _response.IsValid = true;
                    _response.GenericObject = null;
                }

            }
            catch (Exception ex)
            {
                _response.IsValid = false;
                _response.Message = ex.Message;
            }
            return await Task.Run(() => _response);
        }

    }
}
