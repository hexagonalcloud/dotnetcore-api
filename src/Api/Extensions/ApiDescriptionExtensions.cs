using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

// ReSharper disable once CheckNamespace
namespace Api
{
    public static class ApiDescriptionExtensions
    {
        public static string FormatForSwaggerActionGroup(this ApiDescription apiDescription)
        {
            if (string.IsNullOrWhiteSpace(apiDescription.RelativePath))
            {
                return apiDescription.RelativePath;
            }

            return apiDescription.RelativePath.Replace("/{id}", string.Empty).Replace("api/", string.Empty).ToLowerInvariant();
        }
    }
}
