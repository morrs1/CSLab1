﻿namespace CSLabs;

using System.Text.Json;
using Microsoft.AspNetCore.Http;

public static class SessionExtensions
{
    public static T? Get<T>(this ISession session, string key)
    {
        var data = session.GetString(key);
        return data == null ? default : JsonSerializer.Deserialize<T>(data);
    }

    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }
}