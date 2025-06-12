// <copyright file="ConfigOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json;
using System.Text.Json.Serialization;

namespace EBoardConfigManager.Helper
{
    public static class ConfigOptions
    {
        public static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        };
    }
}
