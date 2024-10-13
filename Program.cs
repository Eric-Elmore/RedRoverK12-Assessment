using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string input = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";
        var parsedTree = ParseFields(input);

        Console.WriteLine("### Output 1:");
        FormatOutput(parsedTree, new List<string> { "id", "name", "email", "type", "externalId" }, FormatTypeNode.Original);

        Console.WriteLine("\n### Output 2:");
        FormatOutput(parsedTree, new List<string> { "email", "externalId", "id", "name", "type" }, FormatTypeNode.Alphabetically);

    }

    // Enum to control field ordering
    enum FormatTypeNode
    {
        Original,  // Default
        Alphabetically
    }


    // Represents a node in the hierarchy
    class Node
    {
        public string Name { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();
    }

    // Parse the input string into a tree of Nodes
    static List<Node> ParseFields(string input)
    {
        var stack = new Stack<Node>();
        var root = new Node();
        stack.Push(root);

        string buffer = "";
        foreach (var c in input)
        {
            if (c == '(')
            {
                // Create a new node only if there's a buffer to push
                if (!string.IsNullOrWhiteSpace(buffer))
                {
                    var newNode = new Node { Name = buffer.Trim() };
                    stack.Peek().Children.Add(newNode);
                    stack.Push(newNode);
                    buffer = "";
                }
            }
            else if (c == ')')
            {
                if (!string.IsNullOrWhiteSpace(buffer))
                {
                    stack.Peek().Children.Add(new Node { Name = buffer.Trim() });
                    buffer = "";
                }
                stack.Pop();
            }
            else if (c == ',')
            {
                if (!string.IsNullOrWhiteSpace(buffer))
                {
                    stack.Peek().Children.Add(new Node { Name = buffer.Trim() });
                    buffer = "";
                }
            }
            else
            {
                buffer += c;
            }
        }
        // Handle any remaining buffer outside of the loop
        if (!string.IsNullOrWhiteSpace(buffer))
        {
            stack.Peek().Children.Add(new Node { Name = buffer.Trim() });
        }

        // Return the direct children of the root
        return root.Children; 
    }

    // Format the output based on the given field order
    static void FormatOutput(List<Node> parsedTree, List<string> order, FormatTypeNode formatNode = FormatTypeNode.Original)
    {
        var map = parsedTree.ToDictionary(field => field.Name);
        foreach (var key in order)
        {
            if (map.ContainsKey(key))
            {
                var field = map[key];

                // Adjust the child order for the "type" node based on the formatNode option
                if (key.Equals("type", StringComparison.OrdinalIgnoreCase) && field.Children.Count > 0)
                {
                    if (formatNode == FormatTypeNode.Alphabetically)
                    {
                        // Sort the children alphabetically
                        field.Children = field.Children.OrderBy(c => c.Name).ToList();
                    }
                }


                PrintField(field, 0);
            }
        }
    }

    // Print field and its children with indentation
    static void PrintField(Node field, int indentLevel)
    {
        var indent = new string(' ', indentLevel * 2);
        Console.WriteLine($"{indent}- {field.Name}");

        foreach (var child in field.Children)
        {
            PrintField(child, indentLevel + 1);
        }
    }
}
