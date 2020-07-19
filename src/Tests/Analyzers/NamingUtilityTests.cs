using LocalizedCode.Analyzers;
using NUnit.Framework;

namespace LocalizedCode.Tests.Analyzers
{
  [TestFixture]
  public class NamingUtilityTests
  {
    [TestCase('a', ExpectedResult = true)]
    [TestCase('b', ExpectedResult = true)]
    [TestCase('z', ExpectedResult = true)]
    [TestCase('A', ExpectedResult = true)]
    [TestCase('B', ExpectedResult = true)]
    [TestCase('Z', ExpectedResult = true)]
    [TestCase('0', ExpectedResult = true)]
    [TestCase('1', ExpectedResult = true)]
    [TestCase('9', ExpectedResult = true)]
    [TestCase('_', ExpectedResult = true)]
    [TestCase('.', ExpectedResult = false)]
    [TestCase('@', ExpectedResult = false)]
    [TestCase(' ', ExpectedResult = false)]
    public bool IsLegalASCIIIdentifierCharacter(char c)
    {
      return NamingUtility.IsLegalASCIIIdentifierCharacter(c);
    }

    [TestCase("a", ExpectedResult = true)]
    [TestCase("a1", ExpectedResult = true)]
    [TestCase("@a", ExpectedResult = true)]
    [TestCase("@@a", ExpectedResult = false)]
    [TestCase("a@", ExpectedResult = false)]
    [TestCase("", ExpectedResult = false)]
    [TestCase(null, ExpectedResult = false)]
    public bool IsLegalASCIIIdentifier(string value)
    {
      return NamingUtility.IsLegalASCIIIdentifier(value);
    }
  }
}
