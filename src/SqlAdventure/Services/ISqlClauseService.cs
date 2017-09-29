namespace SqlAdventure.Services
{
    public interface ISqlClauseService
    {
        (string predicate, object[] parameters) CreateWhereClause(string columnName, string parameter);

        string CreateOrderClause(string parameter);
    }
}
