﻿namespace Application.Dtos.Response.Game;

public class UserDataResponse
{
    public int UserId { get; set; }
    public string DisplayName { get; set; }
    public int OldGem { get; set; }
    public int OldCoin { get; set; }
    public int UserCoin { get; set; }
    public int UserGem { get; set; }
}