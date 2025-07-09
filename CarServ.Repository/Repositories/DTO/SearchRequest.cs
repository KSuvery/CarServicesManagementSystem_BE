namespace CarServ.Repository.Repositories.DTO
{
    public class SearchRequest
    {
        public int? CurrentPage { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }

    public class SearchMessagesRequest : SearchRequest
    {
        
    }
}
