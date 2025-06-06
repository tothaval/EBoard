﻿// <copyright file="BasicConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EBoardConfigManager.Models;

public class BasicConfig
{
    public int EBoardCount { get; set; } = 1;

    public int EBoardIndex { get; set; } = 0;

    public bool EBoardBrowserSwitch { get; set; } = true;
}
