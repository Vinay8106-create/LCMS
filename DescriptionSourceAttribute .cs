namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DescriptionSourceAttribute : Attribute
    {
        public string TableName { get; }
        public string IdPropertyName { get; }

        public DescriptionSourceAttribute(string tableName, string idPropertyName)
        {
            TableName = tableName;
            IdPropertyName = idPropertyName;
        }
    }
}