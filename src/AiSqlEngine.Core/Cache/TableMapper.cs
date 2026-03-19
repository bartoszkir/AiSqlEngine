namespace AiSqlEngine.Core.Cache;

internal interface ITableMapper
{
    IEnumerable<string> FindTables(string question);
    void Register(string alias, string tableName);
}

internal sealed class TableMapper : ITableMapper
{
    public IEnumerable<string> FindTables(string question)
    {
        throw new NotImplementedException();
    }

    public void Register(string alias, string tableName)
    {
        throw new NotImplementedException();
    }
}