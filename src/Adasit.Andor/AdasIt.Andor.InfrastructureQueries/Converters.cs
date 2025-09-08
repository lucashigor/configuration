using AdasIt.Andor.Domain.ValuesObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdasIt.Andor.InfrastructureQueries;

public static class Converters
{
    public static ValueConverter<Name, string> GetNameConverter()
        => new(id => id!.Value, value => value);
    
    public static ValueConverter<Description, string> GetDescriptionConverter()
        => new(id => id!.Value, value => value);
}