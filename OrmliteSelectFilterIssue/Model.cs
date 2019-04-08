using ServiceStack.DataAnnotations;

namespace OrmLiteIssueWithAliases
{
    public class FirstTable
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
        public bool Deleted { get; set; }
    }
    
    public class SecondTable : IJoinFilter
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
        public int FirstTableId { get; set; }
        public bool Deleted { get; set; }
    }
}