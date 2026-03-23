using System;
using System.IO;
using System.Linq;
using System.Text;

class BraceFixer
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
            
            if (closes == opens + 1)
            {
                int lastBrace = content.LastIndexOf('}');
                if (lastBrace != -1)
                {
                    string newContent = content.Remove(lastBrace, 1);
                    File.WriteAllText(file, newContent, Encoding.UTF8);
                    Console.WriteLine($"FIXED_STRAY|{file}");
                }
            }
            else if (opens == closes + 1)
            {
                string newContent = content.TrimEnd() + "\n}";
                File.WriteAllText(file, newContent, Encoding.UTF8);
                Console.WriteLine($"FIXED_MISSING|{file}");
            }
        }
    }
}
