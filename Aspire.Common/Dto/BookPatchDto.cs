namespace Aspire.Common.Dto;

public record class BookPatchDto(
    string? Title,
    decimal? Price,
    string? Description
)
{ }