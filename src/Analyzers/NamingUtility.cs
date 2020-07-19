using System;

namespace LocalizedCode.Analyzers
{
  public static class NamingUtility
  {
    public static bool IsLegalASCIIIdentifier(string identifier)
    {
      if (String.IsNullOrEmpty(identifier))
        return false;

      var skip = identifier[0] == '@' ? 1 : 0;
      for (var i = skip; i < identifier.Length; i++)
      {
        if (!IsLegalASCIIIdentifierCharacter(identifier[i]))
          return false;
      }

      return true;
    }
    
    public static bool IsLegalASCIIIdentifierCharacter(char c)
    {
      return (c >= '0' && c <= '9') ||
             (c >= 'A' && c <= 'Z') ||
             (c >= 'a' && c <= 'z') ||
             (c == '_');
    }
  }
}
