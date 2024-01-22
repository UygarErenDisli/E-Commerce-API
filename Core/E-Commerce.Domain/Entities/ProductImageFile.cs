namespace E_Commerce.Domain.Entities
{
	public class ProductImageFile : File
	{
		public bool IsShowCaseImage { get; set; }
		public ICollection<Product> Products { get; set; }
	}
}
