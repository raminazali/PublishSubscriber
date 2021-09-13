using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        //swaggerDoc.Servers.Add(new OpenApiServer() { Url = "/api/core" });
    }
}

public class CustomHeaderSwaggerAttribute : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        // operation.Parameters.Add(new OpenApiParameter
        // {
        //     Name = "X-Database",
        //     In = ParameterLocation.Header,
        //     Required = true,
        //     Schema = new OpenApiSchema
        //     {
        //         Type = "string"
        //     }
        // });
    }

}
