namespace MicroRabbit.Domain.Core.Dtos
{
    public class CommonOrder
    {
        public int Id { get; set; }

        //always passing all order items
        //pass null for delete items
        //will find by book id if new (add) or existing book (check if has changes to update)
        //will delete not existing
        public IEnumerable<CommonOrderedBook>? OrderItems { get; set; } = null!;
    }
}