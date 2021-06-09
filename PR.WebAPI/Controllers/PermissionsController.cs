using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR.Data;
using PR.Entity;
using PR.Entity.Models;
using PR.Service;
using PR.WebAPI.DTOs;

namespace PR.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ModelContext _context;
        public PermissionsController(ModelContext context)
        {
            if (_context == null)
                _context = context;
        }

        [Route("~/[controller]/[action]")]
        [HttpGet]
        public async Task<object> GetPermissions()
        {
            GenericResult<object> _response = new GenericResult<object>();
            try
            {
                var service = new Service<Permission>(_context).GetRepository();
                var query = service.ListAsync(null,
                                                null,
                                                include: source => source
                                                    .Include(a => a.PermissionType)
                                                    ).Result;
                if (query != null)
                {
                    var listPermission = query.Select(x => new PermissionDTO
                    {
                        id = x.Id,
                        name = x.Name,
                        lastName = x.LastName,
                        idPermissionType = x.IdPermissionType,
                        permissionType = x.PermissionType.Description,
                        date = Convert.ToDateTime(x.Date.ToString()).ToString("yyyy-MM-dd")
                    });
                    _response.IsValid = true;
                    _response.GenericObject = listPermission;
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
        [Route("~/[controller]/[action]")]
        [HttpPost]
        public async Task<object> SavePermission([FromBody] PermissionSaveDTO model)
        {
            GenericResult<object> _response = new GenericResult<object>();
            try
            {

                    var service = new Service<Permission>(_context).GetRepository();
                    var findObject = service.ListAsync(x =>
                                                            x.Name == model.name &&
                                                            x.LastName == model.lastName &&
                                                            x.IdPermissionType == model.idPermissionType,
                                                            null,
                                                            null).Result.FirstOrDefault();
                    if (findObject == null)
                    {
                        Permission obj = new Permission();
                        obj.Name = model.name;
                        obj.LastName = model.lastName;
                        obj.IdPermissionType = model.idPermissionType;
                        obj.Date = Convert.ToDateTime(model.date);

                        var response = await service.InsertAsync(obj);

                        if (response != null)
                        {
                            if (response.Id > 0)
                            {
                                _response.IsValid = true;
                                _response.GenericObject = response;
                            }
                            else
                            {
                                _response.IsValid = false;
                                _response.Message = "Error, item could not be saved";
                            }
                        }
                        else
                        {
                            _response.IsValid = false;
                            _response.Message = "Error, item could not be saved";
                        }
                    }
                    else
                    {
                        _response.IsValid = false;
                        _response.Message = "The item already exists, please try another.";
                    }
                
            }
            catch (Exception ex)
            {
                _response.IsValid = false;
                _response.Message = ex.Message;
            }
            return await Task.Run(() => _response);
        }

        [Route("~/[controller]/[action]")]
        [HttpPost]
        public async Task<object> UpdatePermission([FromBody] PermissionUpdateDTO model)
        {
            GenericResult<object> _response = new GenericResult<object>();
            try
            {
                    var service = new Service<Permission>(_context).GetRepository();
                    var findObject = service.ListAsync(x =>
                                                            x.Name == model.name &&
                                                            x.LastName == model.lastName &&
                                                            x.IdPermissionType == model.idPermissionType &&
                                                            x.Date == Convert.ToDateTime(model.date),
                                                            null,
                                                            null).Result.FirstOrDefault();
                    if (findObject == null)
                    {
                        var updateObject = service.ListAsync(x => x.Id == model.id, null, null).Result.FirstOrDefault();
                        if (updateObject != null)
                        {
                            updateObject.Name = model.name;
                            updateObject.LastName = model.lastName;
                            updateObject.IdPermissionType = model.idPermissionType;
                            updateObject.Date = Convert.ToDateTime(model.date);
                            

                            var response = await service.UpdateAsync(updateObject);
                            if (response != null)
                            {
                                _response.IsValid = true;
                                _response.GenericObject = true;
                            }
                            else
                            {
                                _response.IsValid = false;
                                _response.Message = "Error, item could not be update.";
                            }
                        }
                        else
                        {
                            _response.IsValid = false;
                            _response.Message = "The item is does not exist.";
                        }
                    }
                    else
                    {
                        _response.IsValid = false;
                        _response.Message = "The item already exists, please try another.";
                    }
            
            }
            catch (Exception ex)
            {
                _response.IsValid = false;
                _response.Message = ex.Message;
            }
            return await Task.Run(() => _response);
        }
        [Route("~/[controller]/[action]")]
        [HttpPost]
        public async Task<object> DeletePermission(int id)
        {
            GenericResult<object> _response = new GenericResult<object>();
            try
            {
                var service = new Service<Permission>(_context).GetRepository();
                var obj = service.ListAsync(x => x.Id == id,
                                                null,null).Result.FirstOrDefault();

                    if (obj != null)
                    {
                        await service.DeleteAsync(id);
                        
                        _response.IsValid = true;
                        _response.GenericObject = true;
                    }
                    else
                    {
                        _response.IsValid = false;
                        _response.Message = "The item is does not exist.";
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
