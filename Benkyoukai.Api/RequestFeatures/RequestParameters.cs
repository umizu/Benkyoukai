namespace Benkyoukai.Api.RequestFeatures;

public abstract class RequestParameters
{
    const int maxPageSize = 50;
    
    private int pageNumber = 1;
    public int PageNumber
    {
        get => pageNumber;
        set => pageNumber = (value > 0) ? value : 1;
    }

    private int pageSize = 10;

    public int PageSize
    {
        get => pageSize;
        set => pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
}
