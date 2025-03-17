using System.Text;
using AS_2025.Extensions;
using Scriban;
using Scriban.Runtime;

namespace AS_2025;

internal class Generator
{
    private readonly string _fileSystemRoot;

    public Generator(string? fileSystemRoot = null)
    {
        _fileSystemRoot = fileSystemRoot ?? Directory.GetCurrentDirectory();

        if (!Directory.Exists(_fileSystemRoot))
        {
            throw new DirectoryNotFoundException(_fileSystemRoot);
        }
    }

    public void Generate(Dictionary<Feature, Dictionary<string, string>> templates, List<GenerateInfo> generateInfos)
    {
        Console.WriteLine("[Generator.Generate] STARTED");

        foreach (var generateInfo in generateInfos)
        {
            Console.WriteLine($"[Generator.Generate] for model: {generateInfo.ModelName}");

            foreach (var feature in generateInfo.Features)
            {
                if (!templates.ContainsKey(feature))
                {
                    continue;
                }

                foreach (var (path, templateContent) in templates[feature])
                {
                    var processedPath = ProcessPathPlaceholders(path, generateInfo.ModelName);

                    var modelData = new Dictionary<string, object>
                    {
                        { "modelName", generateInfo.ModelName.ToCamelCase() },
                        { "ModelName", generateInfo.ModelName }
                    };

                    var absolutePath = Path.Combine(_fileSystemRoot, processedPath);

                    GenerateFile(templateContent, modelData, absolutePath);
                }
            }
        }

        Console.WriteLine("[Generator.Generate] COMPLETED");
    }

    private static string ProcessPathPlaceholders(string path, string modelName)
    {
        var modelNameCamelCase = modelName.ToCamelCase();

        return path
            .Replace("{ModelName}", modelName)
            .Replace("{{ModelName}}", modelName)
            .Replace("{{ ModelName }}", modelName)
            .Replace("{modelName}", modelNameCamelCase)
            .Replace("{{modelName}}", modelNameCamelCase)
            .Replace("{{ modelName }}", modelNameCamelCase);
    }

    private static void GenerateFile(string templateContent, Dictionary<string, object> modelData, string outputPath)
    {
        try
        {
            var template = Template.Parse(templateContent);
            if (template.HasErrors)
            {
                throw new Exception($"[Generator.GenerateFile] template errors: {string.Join(", ", template.Messages)}");
            }

            var scriptObject = new ScriptObject();
            scriptObject.Import(modelData);

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);

            var render = template.Render(context);

            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Console.WriteLine($"[Generator.GenerateFile] directory created: {directory}");
            }

            File.WriteAllText(outputPath, render, Encoding.UTF8);
            Console.WriteLine($"[Generator.GenerateFile] output file created: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Generator.GenerateFile] error while generating `{outputPath}`: `{ex.Message}`");
        }
    }
}