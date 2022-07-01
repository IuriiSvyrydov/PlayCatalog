namespace PlayCatalog.API
{
    public record ItemDto(Guid Id, string Name, string Description,
        decimal Price, DateTimeOffset CreatedDate);

    public record CreateIemDto(string Name, string Description, decimal Price,DateTimeOffset CreateDate);

    public record UpdateItemDto(string Name, string Description, decimal Price,DateTimeOffset CreateDate);
}