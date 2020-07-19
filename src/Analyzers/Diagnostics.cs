using Microsoft.CodeAnalysis;

namespace LocalizedCode.Analyzers
{
  public static class Diagnostics
  {
    public static readonly DiagnosticDescriptor DeclarationOfNonASCIIIdentifier = new DiagnosticDescriptor
    (
      id: "LC1000",
      title: "Declaration with non-ASCII identifier",
      messageFormat: "{0} contains non ASCII characters in its identifier.",
      category: "Naming",
      defaultSeverity: DiagnosticSeverity.Warning,
      isEnabledByDefault: true,
      description: "Ensures that all declared identifiers contain only ASCII characters."
    );
  }
}
