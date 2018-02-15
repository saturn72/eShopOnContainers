namespace Product.API.Models
{
    public class ProductModel : ApiModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MediaApiModel Media { get; set; }
    }
}
