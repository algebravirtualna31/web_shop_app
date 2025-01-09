using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_shop_app.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Total price is required.")]
        [Column(TypeName = "decimal(9, 2)")]
        public decimal Total { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public OrderAddress OrderAddress { get; set; }
    }
}
