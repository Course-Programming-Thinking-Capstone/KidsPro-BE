using Application.Dtos.Request.Game;
using Application.Dtos.Response.Game;
using Application.Dtos.Response.Paging;
using Domain.Entities;
using Domain.Enums;

namespace Application.Mappers;

public static class GameMapper
{
    public static GameItemResponse GameItemToGameItemResponse(GameItem entity)
    
        => new GameItemResponse
        {
            Id = entity.Id,
            ItemName = entity.ItemName,
            Details = entity.Details,
            SpritesUrl = entity.SpritesUrl,
            ItemRateType = (int)entity.ItemRateType,
            ItemType = (int)entity.ItemType,
            Price = entity.Price
        };

    public static GameItem GameItemRequestToGameItem(NewItemRequest newItem)
    {
        return new GameItem
        {
            Id = newItem.GameId,
            GameId = 0,
            ItemName = newItem.ItemName,
            Details = newItem.Details,
            SpritesUrl = newItem.SpritesUrl,
            ItemRateType = (ItemRateType)newItem.ItemRateType,
            ItemType = (ItemType)newItem.ItemType,
            Price = newItem.Price
        };
    }
}