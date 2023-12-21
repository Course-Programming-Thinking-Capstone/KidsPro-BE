using Application.Dtos.Request.Curriculum;
using Domain.Entities;

namespace Application.Mappers;

public class CurriculumMapper
{
    public static Curriculum CreateDtoToEntity(CreateCurriculumDto dto)
    {
        return new Curriculum()
        {
            Name = dto.Name,
            Description = dto.Description,
            PictureUrl = dto.PictureUrl,
            OpenDate = dto.OpenDate,
            StartSaleDate = dto.StartSaleDate,
            EndSaleDate = dto.EndSaleDate,
            Price = dto.Price
        };
    }
}