using ServiceStack.DataAnnotations;

namespace OrmLiteIssueWithAliases
{
    public interface IJoinFilter
    {
        int FirstTableId { get; set; }
    }
}