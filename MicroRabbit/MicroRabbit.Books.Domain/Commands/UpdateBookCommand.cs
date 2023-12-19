using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Dtos;

namespace MicroRabbit.Books.Domain.Commands
{
    public class UpdateBookCommand : Command
    {
        public UpdateBookCommand(CommonBookData bookData)
        {
            BookData = bookData;
        }

        public CommonBookData BookData { get; set; }
    }
}