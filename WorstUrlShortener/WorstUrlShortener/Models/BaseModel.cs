using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Update;

namespace WorstUrlShortener.Models
{

	public class BaseModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateTimeOffset? CreatedAt { get; set; }

		public DateTimeOffset? UpdatedAt { get; set; }

		public bool Deleted { get; set; }

		[NotMapped]
		public string CreatedDate
        {
			get
			{
				if (this.CreatedAt.Value != DateTime.MinValue)
				{
					return this.CreatedAt.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
				}
				else
				{
					return "Not set";
				}
            }
        }
	}

}
