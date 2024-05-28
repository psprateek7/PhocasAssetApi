public class PaginatedResponse<T>
{
    public List<T> Items { get; set; }
    public string NextToken { get; set; }

    public PaginatedResponse(List<T> items, string nextToken)
    {
        Items = items;
        NextToken = nextToken;
    }
}