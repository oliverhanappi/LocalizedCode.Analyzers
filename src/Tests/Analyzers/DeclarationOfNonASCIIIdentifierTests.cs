using System.Collections.Immutable;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using LocalizedCode.Analyzers;
using LocalizedCode.Tests.Analyzers.TestInfrastructure;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace LocalizedCode.Tests.Analyzers
{
  [TestFixture]
  public class DeclarationOfNonASCIIIdentifierTests : AnalyzerTestBase<DeclarationOfNonASCIIIdentifiersAnalyzer>
  {
    [Test]
    public async Task Delegate_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        delegate void Täst();
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Delegate Täst");
    }

    [Test]
    public async Task DelegateParameter_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        delegate void TestDelegate(object täst);
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Parameter täst");
    }

    [Test]
    public async Task Class_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class Täst
        {
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Class Täst");
    }

    [Test]
    public async Task ConstructorParameter_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          TestClass(object täst)
          {
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Parameter täst");
    }

    [Test]
    public async Task Enum_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        enum Täst
        {
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Enum Täst");
    }

    [Test]
    public async Task EnumValue_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        enum TestEnum
        {
          Täst
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Enum value TestEnum.Täst");
    }

    [Test]
    public async Task Event_Field_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          event System.EventHandler Täst;
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Event TestClass.Täst");
    }

    [Test]
    public async Task Event_FieldMultiple_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          event System.EventHandler Täst, Täst2;
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Event TestClass.Täst", "Event TestClass.Täst2");
    }

    [Test]
    public async Task Event_Property_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          event System.EventHandler Täst
          {
            add { }
            remove { }
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Event TestClass.Täst");
    }

    [Test]
    public async Task Field_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          object Täst;
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Field TestClass.Täst");
    }

    [Test]
    public async Task IndexerParameter_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          int this[object täst] => 0;
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Parameter täst");
    }

    [Test]
    public async Task Interface_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        interface Täst
        {
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Interface Täst");
    }

    [Test]
    public async Task LambdaExpressionParameter_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          void TestMethod()
          {
            Func<object, int> f = täst => 1;
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Parameter täst");
    }

    [Test]
    public async Task Method_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          void Täst()
          {
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Method TestClass.Täst");
    }

    [Test]
    public async Task MethodParameter_Class_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          void TestMethod(object täst)
          {
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Parameter täst");
    }

    [Test]
    public async Task MethodParameter_Interface_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        interface TestInterface
        {
          void TestMethod(object täst);
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Parameter täst");
    }

    [Test]
    public async Task Namespace_SingleNamePart_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        namespace Täst
        {
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Namespace Täst");
    }

    [Test]
    public async Task Namespace_MultipleNameParts_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        namespace A.B.Täst
        {
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Namespace A.B.Täst");
    }

    [Test]
    public async Task Property_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          object Täst { get; set; }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Property TestClass.Täst");
    }

    [Test]
    public async Task Struct_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        struct Täst
        {
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Struct Täst");
    }

    [Test]
    public async Task Variable_SingleDeclaration_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          void TestMethod()
          {
            var täst = 1;
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Variable täst");
    }

    [Test]
    public async Task Variable_MultipleDeclarations_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          void TestMethod()
          {
            int täst1 = 1, täst2 = 1;
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Variable täst1", "Variable täst2");
    }

    [Test]
    public async Task Variable_OutDeclaration_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          void TestMethod()
          {
            Method(out var täst);
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Variable täst");
    }

    [Test]
    public async Task Variable_CastDeclaration_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          void TestMethod(object x)
          {
            if (x is string täst)
            {
            }
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Variable täst");
    }

    [Test]
    public async Task Variable_SwitchDeclaration_DetectsIllegalName()
    {
      var diagnostics = await Analyze(@"
        class TestClass
        {
          void TestMethod(object x)
          {
            switch (x)
            {
              case string täst:
                break;
            }
          }
        }
      ");
      
      AssertIllegalIdentifierDetected(diagnostics, "Variable täst");
    }
    
    [Test]
    public async Task IgnoresAtSymbolAtBeginningOfIdentifiers()
    {
      var diagnostics = await Analyze(@"
        namespace @namespace
        {
          class @class
          {
            object @property { get; set; }
            object @field, @field2;
            event System.EventHandler @event, @event2;
            event System.EventHandler @event3 { add { } remove { } }

            void @method(object @param)
            {
              var @var = 1;
            } 
          }

          delegate void @delegate();

          enum @enum
          {
            @object
          }

          interface @interface
          {
          }

          struct @struct
          {
          }
        }
      ");
      
      AssertNoIllegalIdentifierDetected(diagnostics);
    }

    private void AssertNoIllegalIdentifierDetected(ImmutableArray<Diagnostic> diagnostics)
    {
      var actualMessages = diagnostics.Select(FormatDiagnostic).ToList();
      Assert.That(actualMessages, Is.Empty);
    }

    private void AssertIllegalIdentifierDetected(ImmutableArray<Diagnostic> diagnostics, params string[] identiferNames)
    {
      var actualMessages = diagnostics.Select(FormatDiagnostic).ToList();
      var expectedMessages = identiferNames.Select(i => $"Warning LC1000: {i} contains non ASCII characters in its identifier.").ToList();

      Assert.That(actualMessages, Is.EquivalentTo(expectedMessages));
    }

    private string FormatDiagnostic(Diagnostic diagnostic)
    {
      return $"{diagnostic.Severity} {diagnostic.Id}: {diagnostic.GetMessage(CultureInfo.InvariantCulture)}";
    }
  }
}
