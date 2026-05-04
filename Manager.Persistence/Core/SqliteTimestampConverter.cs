using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Manager.Persistence.Core;

// Source: https://stackoverflow.com/questions/52684458/updating-entity-in-ef-core-application-with-sqlite-gives-dbupdateconcurrencyexce
public class SqliteTimestampConverter : ValueConverter<byte[], string>
{
    public SqliteTimestampConverter() : base(v => v == null ? null : ToDb(v), v => v == null ? null : FromDb(v))
    {
    }

    private static byte[] FromDb(string v)
    {
        return v.Select(c => (byte)c).ToArray();
        // Encoding.ASCII.GetString(v)
    }

    private static string ToDb(byte[] v)
    {
        return new string(v.Select(b => (char)b).ToArray());
        // Encoding.ASCII.GetBytes(v))
    }
}
