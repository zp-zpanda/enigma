await Docfx.Dotnet.DotnetApiCatalog.GenerateManagedReferenceYamlFiles("docfx.json");
await Docfx.Docset.Build("docfx.json");
