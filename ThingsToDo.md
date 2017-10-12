1. Organize code
   - Finish reading up on framework design guidelines, create checklist 
   - Enforce as many conventions as possible using stylecop and fxcop
2. Try activating CSXXXX documentation rules using ruleset so no redundant 
   documentation file is produced
## Document
- ServiceCollection tests improvement
   - Create provider, call GetService on each type that should be retrievable
   - This way it can be verified that all dependencies are hooked up
- solution level stylecop.json 
  - https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2072
    - essentially, right click project > add existing item > add as link
- solution level ruleset (for enabling and disabling rules, ie warnings etc)
  - Within project, expand dependencies > analyzers > StyleCop.Analyzers
  - Change severity for a rule
  - VS creates a ruleset file in the project
  - move the file to the solution root, update projects CodeAnalysisRuleSet
    elements to point to file at solution root.
  - now changes to any rule in any project will update the ruleset for all projects
- a ruleset can be included in another ruleset using 
 \<Include Path="..\..\Shared.ruleset" Action="Default">\</Include>
- Visual studio: showing warnings and errors for closed files
    - Tools > Options > Text Editor > C# > Advanced > Enable full solution code analysis
- Creating a custom roslyn analyzer: 
  - http://roslyn-analyzers.readthedocs.io/en/latest/config-analyzer.html
  - https://github.com/dotnet/roslyn/tree/master/docs/analyzers
  - https://github.com/dotnet/roslyn/wiki/How-To-Write-a-C%23-Analyzer-and-Code-Fix
