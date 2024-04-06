using Application.Dtos.Response.Game;
using Application.Dtos.Response.Paging;
using Domain.Entities;

namespace Application.Mappers;

public static class GameMapper
{
    public static GameShopItem GameItemToGameShopItem(GameItem entity)
        => new GameShopItem
        {
            Id = entity.Id,
            ItemName = entity.ItemName,
            Details = entity.Details,
            SpritesUrl = entity.SpritesUrl,
            ItemRateType = (int)entity.ItemRateType,
            ItemType = (int)entity.ItemType,
            Price = entity.Price
        };

   
}