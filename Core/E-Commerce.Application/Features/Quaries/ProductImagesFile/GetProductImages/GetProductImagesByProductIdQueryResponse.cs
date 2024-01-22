namespace E_Commerce.Application.Features.Quaries.ProductImagesFile.GetProductImages
{
	public class GetProductImagesByProductIdQueryResponse
	{
		public Guid Id { get; set; }
		public string Path { get; set; }
		public string FileName { get; set; }
		public bool IsShowCaseImage { get; set; }
	}
}
