using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LocalizedCode.Analyzers.Utils
{
  public static class NameSyntaxExtensions
  {
    public static IEnumerable<SimpleNameSyntax> GetSimpleNames(this NameSyntax nameSyntax)
    {
      switch (nameSyntax)
      {
        case SimpleNameSyntax simpleNameSyntax:
          yield return simpleNameSyntax;
          break;
        
        case QualifiedNameSyntax qualifiedNameSyntax:
          foreach (var simpleName in qualifiedNameSyntax.Left.GetSimpleNames())
            yield return simpleName;

          foreach (var simpleName in qualifiedNameSyntax.Right.GetSimpleNames())
            yield return simpleName;
          break;
        
        case AliasQualifiedNameSyntax aliasQualifiedNameSyntax:
          foreach (var simpleName in aliasQualifiedNameSyntax.Alias.GetSimpleNames()) 
            yield return simpleName;
          break;
        
        default:
          throw new ArgumentOutOfRangeException($"Unknown name syntax: {nameSyntax} ({nameSyntax.Kind()})", nameof(nameSyntax));
      }
    }
  }
}
