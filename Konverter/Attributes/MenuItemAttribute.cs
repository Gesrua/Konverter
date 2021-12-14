﻿// Konverter - Plain text file style converter
// Copyright (C) 2021 Witches Banquet
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License,
// or any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

namespace Konverter.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class MenuItemAttribute : Attribute
{
    public string Description { get; init; }
    public int Index { get; init; }

    public MenuItemAttribute(int index, string description)
    {
        Description = description;
        Index = index;
    }
}
