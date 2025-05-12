using Presentation.Request;

namespace Domain;

public interface IFieldCommandService
{
    Task<int> Handle(CreateFieldCommand command);
    Task<bool> Handle(UpdateFieldCommand command);
    Task<bool> Handle(DeleteFieldCommand command); 
}