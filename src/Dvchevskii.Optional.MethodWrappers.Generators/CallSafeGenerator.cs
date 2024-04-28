using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Dvchevskii.Optional.MethodWrappers.Generators
{
    [Generator]
    public class CallSafeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            var sourceCode = GenerateClass();

            context.AddSource(
                "MethodWrapperExtensions.g.cs",
                SourceText.From(sourceCode, Encoding.UTF8)
            );
        }

        private static string GenerateTypeParameters(int count)
        {
            if (count <= 0)
                return string.Empty;

            var typeIndices = Enumerable.Range(0, count).Select(i => $"T{i + 1}");
            return $"<{string.Join(", ", typeIndices)}>";
        }

        private static string GenerateParameters(int count)
        {
            if (count <= 0)
                return string.Empty;

            var paramIndices = Enumerable.Range(0, count).Select(i => $"T{i + 1} arg{i + 1}");
            return ", " + string.Join(", ", paramIndices);
        }

        private static string GenerateCallArguments(int count)
        {
            if (count <= 0)
                return string.Empty;

            var argIndices = Enumerable.Range(0, count).Select(i => $"arg{i + 1}");
            return string.Join(", ", argIndices);
        }

        private string GenerateClass()
        {
            string text =
                $@"
namespace global::Dvchevskii.Optional.MethodWrappers
{{
    public static partial class MethodWrapperExtensions
    {{
";

            for (int i = 0; i <= 10; i++)
            {
                text += GenerateMethod(i);
                text += '\n';
            }

            text +=
                @"
    }
}
";

            return text;
        }

        private string GenerateMethod(int argumentCount)
        {
            string text =
                "       public static global::Dvchevskii.Optional.Option<global::Dvchevskii.Unit.Unit> CallSafe";

            string typeParams = GenerateTypeParameters(argumentCount);
            text += typeParams;
            text += "(this global::System.Action";
            text += typeParams;
            text += " action";
            text += GenerateParameters(argumentCount);
            text +=
                $@")
        {{
            try
            {{
                action.Invoke({GenerateCallArguments(argumentCount)});
                return global::Dvchevskii.Optional.Option.Some<global::Dvchevskii.Unit.Unit>(global::Dvchevskii.Unit.Unit.Default);
            }}
            catch
            {{
                return global::Dvchevskii.Optional.Option.None<global::Dvchevskii.Unit.Unit>();
            }}
        }}
";

            return text;
        }
    }
}
