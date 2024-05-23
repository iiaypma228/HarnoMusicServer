using Microsoft.AspNetCore.Mvc;
using Server.BLL.Interfaces;

namespace Server.API.Controllers;

public abstract class BaseCRUDController<T, S> : ControllerBase where T : class where S : ICRUDService<T> 
{
    protected S Service;
    
    public BaseCRUDController(S service)
    {
        Service = service;
    }
    
    [HttpGet( Joint.Data.Constants.ControllerConstants.GetById)]
    public T GetById(int id)
    {
        return Service.Read(id);
    }
    
    [HttpGet( Joint.Data.Constants.ControllerConstants.GetAll)]
    public IList<T> GetAll()
    {
        return Service.Read();
    }
    
}