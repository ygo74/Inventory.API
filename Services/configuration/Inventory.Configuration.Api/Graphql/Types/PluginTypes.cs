﻿using HotChocolate.Types;
using Inventory.Configuration.Api.Application.Plugins;
using Inventory.Configuration.Api.Application.Plugins.Dtos;

namespace Inventory.Configuration.Api.Graphql.Types
{
    public class PluginType : ObjectType<PluginDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PluginDto> descriptor)
        {
            descriptor.Name("PluginDto");
            descriptor.Field(e => e.Name).Name("name").Description("Plugin Name");
        }
    }

    public class CreatePluginInputType : ObjectType<CreatePluginRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<CreatePluginRequest> descriptor)
        {
            descriptor.Name("CreatePlugin");
            descriptor.Field(e => e.Name).Name("name").Description("Plugin Name");
            descriptor.Field(e => e.Code).Name("code").Description("Plugin unique code");
            descriptor.Field(e => e.Version).Name("version").Description("Plugin Version");
        }
    }

    //public class CreatePluginPayloadType : ObjectType<CreatePlugin.Payload>
    //{
    //    protected override void Configure(IObjectTypeDescriptor<CreatePlugin.Payload> descriptor)
    //    {
    //        descriptor.Name("CreatePluginPayload");
    //        descriptor.Field(e => e.Plugin).Type<PluginType>();
    //    }
    //}
}
