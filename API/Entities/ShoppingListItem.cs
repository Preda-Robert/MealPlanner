using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

  public class ShoppingListItem
    {
        [Key]
        public int ShoppingListItemId { get; set; }
        public int Quantity { get; set; }
        public required string Unit { get; set; }
        public bool IsPurchased { get; set; }

        public int ShoppingListId { get; set; }
        [ForeignKey("ShoppingListId")]
        public virtual ShoppingList ShoppingList { get; set; } = null!;
        public int IngredientId { get; set; }  
        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient { get; set; } = null!;
    }
