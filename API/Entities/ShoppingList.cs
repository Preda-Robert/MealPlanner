using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;


    public class ShoppingList
    {
        [Key]
        public int ShoppingListId { get; set; }
        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<ShoppingListItem> Items { get; set; } = [];
    }
