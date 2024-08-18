using System;
using Application.DTO.Response;

namespace Infrastructure.Repository.Products;

public static class GeneralDbResponses
{
    public static ServiceResponse ItemAlreadyExists(string itemName) => new (false, $"{itemName} already created");
    public static ServiceResponse ItemNotFound(string itemName) => new (false, $"{itemName} not found");
    public static ServiceResponse ItemCreated(string itemName) => new (true, $"{itemName} created successfully");
    public static ServiceResponse ItemUpdated(string itemName) => new (true, $"{itemName} updated successfully");
    public static ServiceResponse ItemDeleted(string itemName) => new (true, $"{itemName} deleted successfully");
   

}
