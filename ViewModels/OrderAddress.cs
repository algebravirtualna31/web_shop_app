using System.ComponentModel.DataAnnotations;

namespace web_shop_app.ViewModels
{
    public class OrderAddress
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        public string BillingFirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        public string BillingLastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string BillingEmail { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [StringLength(100)]
        public string BillingPhone { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(150)]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50)]
        public string BillingCity { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50)]
        public string BillingCountry { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        [StringLength(20)]
        public string BillingZip { get; set; }
    }
}
