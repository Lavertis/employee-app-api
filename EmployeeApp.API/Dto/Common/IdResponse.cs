namespace EmployeeApp.API.Dto.Common;

public class IdResponse<T>
{
    public T Id { get; }

    public IdResponse(T id)
    {
        Id = id;
    }
}