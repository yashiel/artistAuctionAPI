using Mapster;

namespace api.Models.DTOs;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Artist, ArtistDTO>();
        config.NewConfig<CreateArtistDTO, Artist>();
        config.NewConfig<Category, CategoryDTO>();
        config.NewConfig<CreateCategoryDTO, Category>();
        config.NewConfig<Event, EventDTO>();
        config.NewConfig<CreateEventDTO, Event>();
        config.NewConfig<Order, OrderDTO>();
        config.NewConfig<CreateOrderDTO, Order>()
            .Map(dest => dest.OrderItems, src => src.OrderItems ?? new List<OrderItem>());
        //config.NewConfig<OrderItem, OrderItemDTO>();
        //config.NewConfig<CreateOrderItemDTO, OrderItem>()
        //    .Map(dest => dest.Quantity, src => src.Quantity > 0 ? src.Quantity : 1)
        //    .Map(dest => dest.Price, src => src.Price >= 0 ? src.Price : 0.0m);
        config.NewConfig<Product, ProductDTO>();
        config.NewConfig<CreateProductDTO, Product>()
            .Map(dest => dest.Price, src => src.Price >= 0 ? src.Price : 0.0m);
        config.NewConfig<UpdateProductDTO, Product>();
        config.NewConfig<Review, ReviewDTO>();
        config.NewConfig<CreateReviewDTO, Review>()
            .Map(dest => dest.Rating, src => src.Rating >= 1 && src.Rating <= 5 ? src.Rating : 1)
            .Map(dest => dest.Comment, src => string.IsNullOrWhiteSpace(src.Comment) ? "No comment" : src.Comment);
    }
}