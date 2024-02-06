using MicroRabbit.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroRabbit.Books.Domain.Models
{
    [Index(nameof(ISBN), IsUnique = true)]
    public class Book : BaseModel
    {
        [MaxLength(20)]
        public string ISBN { get; set; } = null!; //מספר ספר תקני בין-לאומי מסת"ב International Standard Book Number

        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        public int PublicationId { get; set; }//one to many relationship

        public Publication Publication { get; set; } = null!;

        [Required]
        public int AuthorId { get; set; }

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = null!;

        [Required]
        public string CoverImage { get; set; } = null!;

        public int Units { get; set; }

        public ICollection<Category>? Categories { get; set; }//many to many relationship: book can have many categories
        public ICollection<BookCategory>? BookCategory { get; set; }
    }
}