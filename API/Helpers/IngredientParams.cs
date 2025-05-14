using System;

namespace API.Helpers;

public class IngredientParams : PaginationParams
{
    public string? SearchTerm { get; set; }
}
