using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikiLibs.Core
{
    public class ModelRequiredBinding : IBindingMetadataProvider
    {
        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            if (context != null
                && context.PropertyAttributes != null
                && context.PropertyAttributes.OfType<RequiredAttribute>().Any())
                context.BindingMetadata.IsBindingRequired = true;
        }
    }
}
