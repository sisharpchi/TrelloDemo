﻿namespace Application.Dtos.Auth;

public class RefreshRequestDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}