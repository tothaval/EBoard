// <copyright file="ConfigOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EBoardConfigManager.Helper;

using System.Text.Json;
using System.Text.Json.Serialization;

public static class ConfigOptions
{
    public static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
    {
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
    };
}
