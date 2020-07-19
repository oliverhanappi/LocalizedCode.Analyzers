using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LocalizedCode.Analyzers.Utils
{
  public static class VariableDesignationSyntaxExtensions
  {
    public static IEnumerable<SingleVariableDesignationSyntax> GetSingleVariableDesignations(this VariableDesignationSyntax designation)
    {
      switch (designation)
      {
        case DiscardDesignationSyntax _:
          yield break;
        
        case ParenthesizedVariableDesignationSyntax p:
          foreach (var childDesignation in p.Variables)
            foreach (var singleVariableDesignation in childDesignation.GetSingleVariableDesignations())
              yield return singleVariableDesignation;
          break;
        
        case SingleVariableDesignationSyntax singleVariableDesignation:
          yield return singleVariableDesignation;
          break;
        
        default:
          throw new ArgumentOutOfRangeException($"Unknown variable designation: {designation}", nameof(designation));
      }
    }
  }
}
