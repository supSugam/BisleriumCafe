﻿using BisleriumCafe.Helpers;
namespace BisleriumCafe.Services;

internal class SessionService
{
    internal string SessionPath { get; set; } = Explorer.GetDefaultFilePath<Session>(FileExtension.json);

    internal async Task<Session?> LoadSession()
    {
        try
        {
            using FileStream stream = File.OpenRead(SessionPath);
            return await JsonSerializer.DeserializeAsync<Session>(stream) ?? null;
        }
        catch
        {
            return null;
        }
    }

    internal async Task SaveSession(Session data)
    {
        using FileStream stream = File.Create(SessionPath);
        await JsonSerializer.SerializeAsync(stream, data);
    }

    internal void DeleteSession()
    {
        try
        {
            File.Delete(SessionPath);
        }
        catch { }
    }
}