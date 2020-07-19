using System;
using System.Collections.Immutable;
using LocalizedCode.Analyzers.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LocalizedCode.Analyzers
{
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class DeclarationOfNonASCIIIdentifiersAnalyzer : DiagnosticAnalyzer
  {
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
      Diagnostics.DeclarationOfNonASCIIIdentifier
    );

    public override void Initialize(AnalysisContext context)
    {
      context.EnableConcurrentExecution();
      context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
      
      context.RegisterSyntaxNodeAction(OnDeclarationExpression, SyntaxKind.DeclarationExpression);
      context.RegisterSyntaxNodeAction(OnDeclarationPattern, SyntaxKind.DeclarationPattern);
      context.RegisterSyntaxNodeAction(OnDelegateDeclaration, SyntaxKind.DelegateDeclaration);
      context.RegisterSyntaxNodeAction(OnTypeDeclaration("Class"), SyntaxKind.ClassDeclaration);
      context.RegisterSyntaxNodeAction(OnTypeDeclaration("Enum"), SyntaxKind.EnumDeclaration);
      context.RegisterSyntaxNodeAction(OnEnumMemberDeclaration, SyntaxKind.EnumMemberDeclaration);
      context.RegisterSyntaxNodeAction(OnEventDeclaration, SyntaxKind.EventDeclaration);
      context.RegisterSyntaxNodeAction(OnFieldDeclaration("Event"), SyntaxKind.EventFieldDeclaration);
      context.RegisterSyntaxNodeAction(OnFieldDeclaration("Field"), SyntaxKind.FieldDeclaration);
      context.RegisterSyntaxNodeAction(OnTypeDeclaration("Interface"), SyntaxKind.InterfaceDeclaration);
      context.RegisterSyntaxNodeAction(OnVariableDeclaration, SyntaxKind.LocalDeclarationStatement);
      context.RegisterSyntaxNodeAction(OnMethodDeclaration, SyntaxKind.MethodDeclaration);
      context.RegisterSyntaxNodeAction(OnNamespaceDeclaration, SyntaxKind.NamespaceDeclaration);
      context.RegisterSyntaxNodeAction(OnParameter, SyntaxKind.Parameter);
      context.RegisterSyntaxNodeAction(OnPropertyDeclaration, SyntaxKind.PropertyDeclaration);
      context.RegisterSyntaxNodeAction(OnTypeDeclaration("Struct"), SyntaxKind.StructDeclaration);
    }

    private void OnDeclarationExpression(SyntaxNodeAnalysisContext context)
    {
      var declarationPattern = (DeclarationExpressionSyntax) context.Node;
      var singleVariableDesignations = declarationPattern.Designation.GetSingleVariableDesignations();

      foreach (var singleVariableDesignation in singleVariableDesignations)
        CheckIsLegalASCIIIdentifier(context, singleVariableDesignation.Identifier, $"Variable {singleVariableDesignation.Identifier}");
    }

    private void OnDeclarationPattern(SyntaxNodeAnalysisContext context)
    {
      var declarationPattern = (DeclarationPatternSyntax) context.Node;
      var singleVariableDesignations = declarationPattern.Designation.GetSingleVariableDesignations();

      foreach (var singleVariableDesignation in singleVariableDesignations)
        CheckIsLegalASCIIIdentifier(context, singleVariableDesignation.Identifier, $"Variable {singleVariableDesignation.Identifier}");
    }

    private void OnDelegateDeclaration(SyntaxNodeAnalysisContext context)
    {
      var @delegate = (DelegateDeclarationSyntax) context.Node;
      CheckIsLegalASCIIIdentifier(context, @delegate.Identifier, $"Delegate {@delegate.Identifier}");
    }

    private void OnEnumMemberDeclaration(SyntaxNodeAnalysisContext context)
    {
      var enumMember = (EnumMemberDeclarationSyntax) context.Node;
      var @enum = (EnumDeclarationSyntax?) enumMember.Parent;
      var enumMemberName = $"{@enum?.Identifier}.{enumMember.Identifier}";

      CheckIsLegalASCIIIdentifier(context, enumMember.Identifier, $"Enum value {enumMemberName}");
    }

    private void OnEventDeclaration(SyntaxNodeAnalysisContext context)
    {
      var @event = (EventDeclarationSyntax) context.Node;
      var type = (TypeDeclarationSyntax?) @event.Parent;
      var eventName = $"{type?.Identifier}.{@event.Identifier}";

      CheckIsLegalASCIIIdentifier(context, @event.Identifier, $"Event {eventName}");
    }

    private Action<SyntaxNodeAnalysisContext> OnFieldDeclaration(string typeName)
    {
      return context =>
      {
        var fields = (BaseFieldDeclarationSyntax) context.Node;
        var type = (BaseTypeDeclarationSyntax?) fields.Parent;

        foreach (var field in fields.Declaration.Variables)
        {
          var fieldName = $"{type?.Identifier}.{field.Identifier}";
          CheckIsLegalASCIIIdentifier(context, field.Identifier, $"{typeName} {fieldName}");
        }
      };
    }
    
    private void OnMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
      var method = (MethodDeclarationSyntax) context.Node;
      var type = (TypeDeclarationSyntax?) method.Parent;
      var methodName = $"{type?.Identifier}.{method.Identifier}";

      CheckIsLegalASCIIIdentifier(context, method.Identifier, $"Method {methodName}");
    }
    
    private void OnNamespaceDeclaration(SyntaxNodeAnalysisContext context)
    {
      var @namespace = (NamespaceDeclarationSyntax) context.Node;
      var namespaceName = @namespace.Name.ToString();

      CheckIsLegalASCIIIdentifier(context, @namespace.Name, $"Namespace {namespaceName}");
    }

    private void OnParameter(SyntaxNodeAnalysisContext context)
    {
      var parameter = (ParameterSyntax) context.Node;
      CheckIsLegalASCIIIdentifier(context, parameter.Identifier, $"Parameter {parameter.Identifier}");
    }

    private void OnPropertyDeclaration(SyntaxNodeAnalysisContext context)
    {
      var property = (PropertyDeclarationSyntax) context.Node;
      var type = (TypeDeclarationSyntax?) property.Parent;
      var propertyName = $"{type?.Identifier}.{property.Identifier}";

      CheckIsLegalASCIIIdentifier(context, property.Identifier, $"Property {propertyName}");
    }

    private Action<SyntaxNodeAnalysisContext> OnTypeDeclaration(string typeName)
    {
      return context =>
      {
        var typeDeclaration = (BaseTypeDeclarationSyntax) context.Node;
        CheckIsLegalASCIIIdentifier(context, typeDeclaration.Identifier, $"{typeName} {typeDeclaration.Identifier}");
      };
    }

    private void OnVariableDeclaration(SyntaxNodeAnalysisContext context)
    {
      var localDeclaration = (LocalDeclarationStatementSyntax) context.Node;
      foreach (var variableDeclaratorSyntax in localDeclaration.Declaration.Variables)
        CheckIsLegalASCIIIdentifier(context, variableDeclaratorSyntax.Identifier, $"Variable {variableDeclaratorSyntax.Identifier}");
    }

    private void CheckIsLegalASCIIIdentifier(SyntaxNodeAnalysisContext context, NameSyntax nameSyntax, string name)
    {
      var simpleNames = nameSyntax.GetSimpleNames();
      foreach (var simpleName in simpleNames) 
        CheckIsLegalASCIIIdentifier(context, simpleName.Identifier, name);
    }

    private void CheckIsLegalASCIIIdentifier(SyntaxNodeAnalysisContext context, SyntaxToken syntaxToken, string name)
    {
      if (!NamingUtility.IsLegalASCIIIdentifier(syntaxToken.Text))
      {
        context.ReportDiagnostic(Diagnostic.Create(Diagnostics.DeclarationOfNonASCIIIdentifier, syntaxToken.GetLocation(), name));
      }
    }
  }
}
