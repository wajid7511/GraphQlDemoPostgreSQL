namespace GraphQlDemoPostgresQl.Common.Database;

public class DbResult<T>
    where T : class
{
    public bool IsSuccess { get; private set; }

    public T? Data { get; private set; }
    public Exception? Exception { get; set; }
    public bool IsError => Exception != null;

    public DbResult(bool isSuccess, T? data = null)
    {
        IsSuccess = isSuccess;
        Data = data;
    }

    public DbResult(Exception exception)
    {
        Exception = exception;
        IsSuccess = false;
    }
}
