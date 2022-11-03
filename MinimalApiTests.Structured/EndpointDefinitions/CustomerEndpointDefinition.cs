using Microsoft.AspNetCore.Mvc;
using MinimalApiTests.Structured.Models;
using MinimalApiTests.Structured.Services;

namespace MinimalApiTests.Structured.EndpointDefinitions;

public class CustomerEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/customersWithAuthorization", GetAllCustomersWithAuthorization);

        app.MapGet("/customers", GetAllCustomers).AllowAnonymous();
        app.MapGet("/customers/{id}", GetCustomerById).AllowAnonymous();
        app.MapPost("/customers", CreateCustomer).AllowAnonymous();
        app.MapPut("/customers/{id}", UpdateCustomer).AllowAnonymous();
        app.MapDelete("/customers/{id}", DeleteCustomerById).AllowAnonymous();
    }

    [ProducesResponseType(200, Type= typeof(Customer))]
    internal List<Customer> GetAllCustomersWithAuthorization(ICustomerService service)
    {
        return service.GetAll();
    }

    internal List<Customer> GetAllCustomers(ICustomerService service)
    {
        return service.GetAll();
    }

    internal IResult GetCustomerById(ICustomerService service, Guid id)
    {
        var customer = service.GetById(id);
        return customer is not null ? Results.Ok(customer) : Results.NotFound();
    }

    internal IResult CreateCustomer(ICustomerService service, Customer customer)
    {
        service.Create(customer);
        return Results.Created($"/customers/{customer.Id}", customer);
    }

    internal IResult UpdateCustomer(ICustomerService service, Guid id, Customer updatedCustomer)
    {
        var customer = service.GetById(id);
        if (customer is null)
        {
            return Results.NotFound();
        }

        service.Update(updatedCustomer);
        return Results.Ok(updatedCustomer);
    }

    internal IResult DeleteCustomerById(ICustomerService service, Guid id)
    {
        service.Delete(id);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<ICustomerService, CustomerService>();
    }
}
