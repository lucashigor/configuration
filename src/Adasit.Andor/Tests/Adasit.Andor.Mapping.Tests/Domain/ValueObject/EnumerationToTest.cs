using AdasIt.Andor.Domain;

namespace Adasit.Andor.Mapping.Tests.Domain.ValueObject;

internal record EnumerationToTest : Enumeration<int>
{
    private EnumerationToTest(int key, string name) : base(key, name)
    {
    }

    public static readonly EnumerationToTest Undefined = new(0, "undefined");
    public static readonly EnumerationToTest MoneyDeposit = new(1, "One");
    public static readonly EnumerationToTest MoneySpending = new(2, "Two");
}
