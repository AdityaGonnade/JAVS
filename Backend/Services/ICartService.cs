using JWT_Token_Example.Models;
using JWT_Token_Example.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Services;

public interface ICartService
{
    Task<IActionResult> AddtoCart(GetProductDto getProductDto, Guid UserId);
    Task<IActionResult> RemoveFormCart(Guid UserId, CartItems cartItem);

    Task<IEnumerable<CartItems>> GetCartItems(Guid userId);
    Task<IActionResult> RemoveUser(Guid userId);

}