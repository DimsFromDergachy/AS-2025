﻿namespace AS_2025.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string value)
    {
        return System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(value);
    }
}