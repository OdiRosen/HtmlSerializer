# HtmlSerializer

This project is a high-performance HTML Parser and Serializer engine built from scratch.
It demonstrates advanced capabilities in string processing, tree-structure navigation,
and the implementation of a custom CSS Selector engine, bypassing the need for heavy external libraries.

Core Features
- Custom HTML Parsing Engine: Implemented a robust parser that utilizes Regular Expressions
  (Regex) and string manipulation to transform raw HTML strings into a hierarchical Tree Data
  Structure of HtmlElement objects.
- CSS Selector Engine (The Challenge): Built a sophisticated search mechanism that allows
  querying the parsed HTML tree using standard CSS selectors (e.g., div.container #main-content).
  This involves a recursive "Matching" algorithm that traverses the tree to find elements by Tag, Id, and Class.
- Dynamic Tag Configuration: The system is "context-aware"—it uses external JSON
  configuration files to distinguish between standard HTML tags and "Void Tags" (self-closing tags),
  ensuring the tree is built with perfect structural integrity.
- Recursive Tree Traversal: Implemented advanced DFS (Depth-First Search) algorithms to
  navigate, search, and flatten the HTML element hierarchy.

Tech Stack
- Backend: .NET 8 Console Application, C#.
- Data Handling: Newtonsoft.Json for dynamic configuration loading.
- Patterns: Singleton Pattern for Helper classes and Recursive Traversal patterns for tree navigation.
- Core Logic: System.Text.RegularExpressions for high-speed HTML tokenization.

How it Works
1. Loading Configuration: The system initializes by reading HtmlTags.json and
   HtmlVoidTags.json to define the rules of the HTML language.
2. Tokenization: Raw HTML is stripped of unnecessary whitespace and split into a stream of tags
   and inner text using specialized Regex patterns.
3. Tree Construction: The serializer iterates through the tokens, creating a parent-child hierarchy.
   It intelligently handles opening tags, closing tags, and self-closing tags to build an accurate
   DOM-like representation.
4. Querying: A Selector object parses a CSS query string and filters the entire HtmlElement
   tree to return only the matching nodes.

Setup
To run this project locally:
1. Clone the repository.
2. Ensure the JSON Files directory contains the required configuration files:
   - HtmlTags.json
   - HtmlVoidTags.json
3. Build the solution and run the console application.
4. Input an HTML source (via URL or string) to see the serialized tree and query results.
