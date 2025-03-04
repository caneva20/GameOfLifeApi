using GameOfLife.Api.Dtos;
using GameOfLife.Models;
using Riok.Mapperly.Abstractions;

namespace GameOfLife.Api.Mapping;

[Mapper]
public static partial class GameMapper {
    public static partial BoardDto ToDto(this Board board);

    public static partial Board ToEntity(this BoardDto board);
}