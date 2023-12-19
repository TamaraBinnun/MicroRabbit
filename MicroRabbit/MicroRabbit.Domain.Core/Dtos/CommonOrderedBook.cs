namespace MicroRabbit.Domain.Core.Dtos
{
    public class CommonOrderedBook
    {
        public int OrderId { get; set; }

        public int OrderItemId { get; set; }
        public int BookId { get; set; }
        public int OrderedUnits { get; set; }

        public bool IsDeleted { get; set; }
    }
}