using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Cuemon.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Cuemon.Data.Assets;
internal class FakeDataManager(Action<DataManagerOptions> setup) : DataManager(setup)
{
    private IDbCommand _command = null;

    public override DataManager Clone()
    {
        return new FakeDataManager(Patterns.ConfigureRevert(Options));
    }

    protected override IDbCommand GetDbCommand(DataStatement statement)
    {
        return Patterns.SafeInvoke(() => new SqliteCommand(statement.Text, new SqliteConnection(Options.ConnectionString)), sc =>
        {
            if (_command == null) { _command = sc; } // we do this in order to always have at least one connection open for the in-mem db (https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/in-memory-databases#shareable-in-memory-databases)
            foreach (var parameter in statement.Parameters)
            {
                sc.Parameters.Add(parameter);
            }
            sc.CommandTimeout = (int)statement.Timeout.TotalSeconds;
            sc.CommandType = statement.Type;
            return sc;
        }, ex => throw ExceptionInsights.Embed(new InvalidOperationException("There is an error when creating a new SqlCommand.", ex), MethodBase.GetCurrentMethod(), Arguments.ToArray(statement)));
    }
}
