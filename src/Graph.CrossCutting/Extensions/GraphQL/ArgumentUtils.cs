using GraphQL.Language.AST;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Graph.CrossCutting.Extensions.GraphQL
{
    public static class ArgumentUtils
    {
        public static string[] ParseSubFields(this IDictionary<string, Field> subFields)
        {
            var fields = new List<string>();

            foreach (var field in subFields)
            {
                var selections = field.Value.SelectionSet.Selections;

                if (selections.Any())
                {
                    var complexFieldType = field.Key.CapitalizeFirst();

                    var childFields = new List<string>();
                    childFields.AddRange(selections.Select(sel => $"{complexFieldType}.{(sel as Field).Name}".ToLower()));

                    fields.AddRange(childFields);
                }
                else
                {
                    fields.Add(field.Key);
                }

            }

            return fields.ToArray();
        }

        public static IEnumerable<string> GetRelatedEntities(this IDictionary<string, Field> subFields)
        {
            return subFields.Where(i => i.Value.SelectionSet.Selections.Any()).Select(i => i.Key);
        }

        private static string CapitalizeFirst(this string field)
        {
            var complexFieldType = new StringBuilder();
            complexFieldType.Append(field.Substring(0, 1).ToUpper());
            complexFieldType.Append(field.Substring(1));

            return complexFieldType.ToString();
        }

        private static string[] SplitCamelCase(this string source)
        {
            return Regex.Split(source, @"(?<!^)(?=[A-Z])");
        }
    }
}
