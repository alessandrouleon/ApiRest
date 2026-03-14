using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace APIRest.API.Common;

public class DefaultValueEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo is null)
            return;

        var defaultAttr = context.MemberInfo.GetCustomAttribute<DefaultValueAttribute>();
        if (defaultAttr is null)
            return;

        schema.Default = new OpenApiString(defaultAttr.Value?.ToString());
    }
}
