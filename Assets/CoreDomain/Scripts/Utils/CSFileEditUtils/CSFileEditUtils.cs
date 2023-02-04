using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utils
{
    public class CSFileEditUtils
    {
        public static void AddLineToFunction(string filePath, string functionName, string lineToAdd)
        {
            // Read in the contents of the file
            List<string> fileLines = File.ReadAllLines(filePath).ToList();
            
            int functionIndex = -1;
            for (int i = 0; i < fileLines.Count; i++)
            {
                if (fileLines[i].Contains(functionName))
                {
                    functionIndex = i;
                    break;
                }
            }

            for (int i = functionIndex; i < fileLines.Count; i++)
            {
                if (fileLines[i].Contains("}"))
                {
                    fileLines.Insert(i,lineToAdd);
                    break;
                }
                    
            }
            File.WriteAllLines(filePath,fileLines);
        }
        
        public static void AddUsingToFile(string filePath, string usingStatement)
        {
            // Read in the contents of the file
            List<string> fileLines = File.ReadAllLines(filePath).ToList();
            fileLines.Insert(0,usingStatement);
            File.WriteAllLines(filePath,fileLines);
        }
        
        public static void CopyFileAndReplaceWildcard(string filePath, string folderDestination,string wildcard, string value,string originalFileExtension = "txt",string newExtension = "cs")
        {
            string destinationFileName = Path.GetFileName(filePath).Replace(originalFileExtension, newExtension);
            destinationFileName = destinationFileName.Replace(wildcard, value);
            string fileContent = File.ReadAllText(filePath);
            fileContent = fileContent.Replace(wildcard, value);
            File.WriteAllText(Path.Combine(folderDestination, destinationFileName), fileContent);
        }
    }
}