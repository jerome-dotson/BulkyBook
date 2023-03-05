
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string Name { get; set; }
		public string? StreetAddress { get; set; } //Adding question mark makes the properties nullable. 
		public string? City { get; set; }
		public string? State { get; set; }
		public string? PostalCode { get; set; }

		public int? CompanyId { get; set; }
		[ForeignKey("CompanyId")]
		[ValidateNever]
		public Company Company { get; set; } //Does this need to be nullable? add-migration addCompanyIdToUser
	}
}
