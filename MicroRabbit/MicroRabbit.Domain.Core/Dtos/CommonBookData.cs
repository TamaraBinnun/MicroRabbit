namespace MicroRabbit.Domain.Core.Dtos
{
    public class CommonBookData
    {
        public int BookId { get; set; }

        public string Title { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}