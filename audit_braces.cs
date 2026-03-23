using System;
using System.IO;
using System.Linq;

class BraceAudit
{
    static void Main()
    {
        string root = @"c:\EndlessFinal\EndlessRunnerProject\Assets\Scripts";
        var files = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);
        
        foreach (var file in files)
        {
            if (file.Contains("ThirdParty") || file.Contains("Plugins")) continue;
            
            string content = File.ReadAllText(file);
            int opens = content.Count(c => c == '{');
            int closes = content.Count(c => c == '}');
            
            bool hasClassHeader = content.Contains("class ") || content.Contains("struct ") || content.Contains("enum ") || content.Contains("interface ");
            
            if (opens != closes)
            {
                Console.WriteLine($"MISMATCH|{file}|{opens}|{closes}");
            }
            else if (hasClassHeader && opens == 0)
            {
                Console.WriteLine($"NO_BRACES|{file}|{opens}|{closes}");
            }
        }
    }
}
