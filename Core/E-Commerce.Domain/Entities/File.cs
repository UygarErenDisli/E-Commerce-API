using E_Commerce.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Domain.Entities
{
	public class File : BaseEntity
	{
		public string FileName { get; set; }
		public string Path { get; set; }
		public string Storage { get; set; }
		[NotMapped]
		public override DateTime UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; }
	}
}
