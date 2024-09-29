using System.Data.Common;
using System.Reflection;

namespace EmployeeApp.Infrastructure.Database.Extensions;

public static class DataReaderExtensions
{
    public static T MapToObject<T>(this DbDataReader reader) where T : new()
    {
        var obj = new T();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!reader.HasColumn(property.Name) || reader.IsDBNull(reader.GetOrdinal(property.Name)))
                continue;

            var value = reader.GetValue(reader.GetOrdinal(property.Name));
            property.SetValue(obj, value);
        }

        return obj;
    }

    private static bool HasColumn(this DbDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}