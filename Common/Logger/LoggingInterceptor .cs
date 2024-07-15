using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using System.Data.Common;

namespace ecommerse_api.Common.Logger
{
    public class LoggingInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<int> result)
        {
            Log.Information($"Executing SQL command: {command.CommandText}");
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            Log.Information($"Executing SQL command: {command.CommandText}");
            return base.ReaderExecuting(command, eventData, result);
        }

        public override InterceptionResult<object> ScalarExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<object> result)
        {
            Log.Information($"Executing SQL command: {command.CommandText}");
            return base.ScalarExecuting(command, eventData, result);
        }
    }
}
