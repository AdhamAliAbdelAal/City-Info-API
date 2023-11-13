﻿using System.Text.Json.Serialization;

namespace CityInfoApi.DTOs;

public class AuthenticationRequestDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}