using Npgsql;

namespace Fluxo.Application.Common;

public static class PostgresConstraintHelper
{
    public static bool IsUniqueViolation(Exception ex) =>
        HasSqlState(ex, PostgresErrorCodes.UniqueViolation);

    public static bool IsForeignKeyViolation(Exception ex) =>
        HasSqlState(ex, PostgresErrorCodes.ForeignKeyViolation);

    private static bool HasSqlState(Exception ex, string sqlState) =>
        (ex as PostgresException ?? ex.InnerException as PostgresException)?.SqlState == sqlState;
}
