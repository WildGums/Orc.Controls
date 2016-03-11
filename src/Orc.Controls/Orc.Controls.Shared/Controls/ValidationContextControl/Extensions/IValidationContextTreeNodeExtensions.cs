// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValidationContextTreeNodeExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    public static class IValidationContextTreeNodeExtensions
    {
        private class Node
        {
            public int Level;
            public IValidationContextTreeNode Value;
        }

        public static string ToText(this IEnumerable<IValidationContextTreeNode> validationContextTreeNodes)
        {
            var result = string.Empty;
            var nodes = validationContextTreeNodes.Where(x => x.IsVisible).OrderBy(x => x.DisplayName).Select(x => new Node {Level = 0, Value = x});
            var stack = new Stack<Node>(nodes);
            while (stack.Any())
            {
                var node = stack.Pop();
                for (var i = 0; i < node.Level; i++)
                {
                    result += "    ";
                }

                result += (node.Value.DisplayName + System.Environment.NewLine);
                var nexLevel = node.Level + 1;
                foreach (var subNode in node.Value.Children.Where(x => x.IsVisible).OrderBy(x => x.DisplayName))
                {
                    stack.Push(new Node {Level = nexLevel, Value = subNode});
                }
            }

            return result;
        }

        public static void ExpandAll(this IEnumerable<IValidationContextTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.IsExpanded = true;

                node.Children.ExpandAll();
            }
        }

        public static void CollapseAll(this IEnumerable<IValidationContextTreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.IsExpanded = false;

                node.Children.CollapseAll();
            }
        }

        public static bool HasAnyCollapsed(this IEnumerable<IValidationContextTreeNode> nodes)
        {
            return nodes?.Any(x => !x.IsExpanded || HasAnyCollapsed(x.Children)) ?? false;
        }

        public static bool HasAnyExpanded(this IEnumerable<IValidationContextTreeNode> nodes)
        {
            return nodes?.Any(x => x.IsExpanded || HasAnyExpanded(x.Children)) ?? false;
        }
    }
}