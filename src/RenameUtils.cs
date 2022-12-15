using System.Text.RegularExpressions;
using CommandLine;
using Renamer.ComparerUtils;
using Renamer.RenameInfo;

namespace Renamer.RenameUtils;

class RenamerUtils
{
    public static Info PrepareRename(string path, bool reverse, bool ignoreDirs, bool ignoreFiles, bool ignoreDotDirs, bool ignoreDotFiles)
    {
        var srcInfo = new DirectoryInfo(path);
        var dirs = srcInfo.GetDirectories();
        var files = srcInfo.GetFiles();

        if (ignoreDirs) dirs = new DirectoryInfo[0];
        if (ignoreFiles) files = new FileInfo[0];

        if (ignoreDotDirs) dirs = Array.FindAll(dirs, dir => !dir.Name.StartsWith("."));
        if (ignoreDotFiles) files = Array.FindAll(files, file => !file.Name.StartsWith("."));

        dirs = dirs.OrderBy(d => d, new OrderDirsByName()).ToArray();
        files = files.OrderBy(f => f, new OrderFilesByName()).ToArray();

        if (reverse)
        {
            files = files.Reverse().ToArray();
            dirs = dirs.Reverse().ToArray();
        }

        return new Info(dirs, files);
    }

    // ---------------------------------------------------------------------------------------------------------

    public static string ConvertIndexToString(int index, bool upper)
    {
        var alpha = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                                 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};

        var stringIndex = "";
        var mult = index / 26;
        var remainder = index - (mult * 26);

        for (var i = 0; i < mult; i++)
        {
            stringIndex += "z";
        }
        stringIndex += alpha[remainder];

        if (upper) stringIndex = stringIndex.ToUpper();

        return stringIndex;
    }

    public static int ConvertStringToIndex(string stringIndex)
    {
        var alpha = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                                 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
        stringIndex = stringIndex.ToLower();

        var index = 0;

        var len = stringIndex.Length;
        if (len == 1) return Array.IndexOf(alpha, stringIndex.ToCharArray()[0]);

        for (var i = 0; i < stringIndex.Length; i++)
        {
            var c = stringIndex.ElementAt(i);
            if (Array.IndexOf(alpha, c) < 0)
            {
                Console.WriteLine("ERROR: Wrong start value. Please use English letters only.");
                System.Environment.Exit(1);
            }
            if (c == 'z')
            {
                index += 26;
            }
            else
            {
                index += Array.IndexOf(alpha, c);
            }
        }
        return index;
    }

    // ---------------------------------------------------------------------------------------------------------

    public static void CopyDirectory(string src, string dist)
    {
        var srcDirInfo = new DirectoryInfo(src);
        if (!srcDirInfo.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {srcDirInfo.FullName}");

        DirectoryInfo[] dirs = srcDirInfo.GetDirectories();
        Directory.CreateDirectory(dist);

        foreach (FileInfo file in srcDirInfo.GetFiles())
        {
            string targetFilePath = Path.Combine(dist, file.Name);
            file.CopyTo(targetFilePath);
        }

        foreach (DirectoryInfo subDir in dirs)
        {
            string newDestinationDir = Path.Combine(dist, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir);
        }
    }

    public static string CheckExisting(string path, string dist, string right, string baseName, string left)
    {
        var currentInfo = new DirectoryInfo(path);
        var currentDirs = currentInfo.GetDirectories();
        var currentFiles = currentInfo.GetFiles();
        string[] currentNames;
        currentNames = new string[currentDirs.Length + currentFiles.Length];
        currentDirs.Select(item => item.Name).ToArray().CopyTo(currentNames, 0);
        currentFiles.Select(item => item.Name).ToArray().CopyTo(currentNames, currentDirs.Length);

        var extra = 1;
        while (true)
        {
            if (currentNames.Contains(dist))
            {
                dist = $"{right}{baseName}({extra}){left}";
                extra++;
                continue;
            }
            return dist;
        }
    }

    public static string GetExtension(string fileName)
    {
        var splitText = fileName.Split(".");
        var len = splitText.Length;

        if (fileName.StartsWith(".") && len > 2) return splitText.Last();
        if (!fileName.StartsWith(".") && len >= 2) return splitText.Last();
        return fileName;
    }

    public static string RemoveExtension(string fileName)
    {
        var splitText = fileName.Split(".");
        var len = splitText.Length;

        if (fileName.StartsWith(".") && len > 2) return fileName.Replace("." + splitText.Last(), "");
        if (!fileName.StartsWith(".") && len >= 2) return fileName.Replace("." + splitText.Last(), "");
        return fileName;
    }

    static string RemoveDisallowedCharacters(string name)
    {
        var disallowedChars = "@$%&\\/:*?\"'<>|~`#^+={}[];!";

        foreach (var ch in disallowedChars)
        {
            name = name.Replace(ch.ToString(), "");
        }
        return name;
    }
    // ---------------------------------------------------------------------------------------------------------

    public static string[][] HandlePatternRegex(string cmd, string nextCmd, string[] prevDirsNames, string[] prevFilesNames)
    {
        var regexCmd = cmd.Substring(2);
        var dirsNames = new string[prevDirsNames.Length];
        var filesNames = new string[prevFilesNames.Length];
        var index = 1;

        if (nextCmd.StartsWith("## "))
        {
            try
            {
                index = Int32.Parse(nextCmd.Substring(3));
            }
            catch
            {
                Console.WriteLine("Invalid group index");
            }
        }

        for (var i = 0; i < dirsNames.Length; i++)
        {
            var matches = Regex.Matches(prevDirsNames[i], regexCmd);

            if (index == -1)
            {
                dirsNames[i] = string.Join("", matches);

            }
            else if (matches.Count > 0 && matches[0].Groups.Keys.ToArray().Length > index)
            {
                dirsNames[i] = matches[0].Groups[index].Value;
            }
            else
            {
                dirsNames[i] = "";
            }
        }

        for (var i = 0; i < filesNames.Length; i++)
        {
            var matches = Regex.Matches(RemoveExtension(prevFilesNames[i]), regexCmd);
            if (index == -1)
            {
                filesNames[i] = string.Join("", matches);

            }
            else if (matches.Count > 0 && matches[0].Groups.Keys.ToArray().Length > index)
            {
                filesNames[i] = matches[0].Groups[index].Value;
            }
            else
            {
                filesNames[i] = "";
            }
        }

        return new string[][] { dirsNames, filesNames };
    }

    public static string[][] HandlePatternCommand(string cmd, string path, string[] prevDirsNames, string[] prevFilesNames)
    {
        var renameCmd = cmd.Substring(2) + $" -p {path}";

        var partialInfo = CommandLine.Parser.Default.ParseArguments<
            RandomOptions,
            NumericalOptions,
            AlphabeticalOptions,
            ReverseOptions,
            ReplaceOptions,
            UpperOptions,
            LowerOptions>(renameCmd.Split(" "))
        .MapResult(
            (RandomOptions opts) => RenamerInfo.Random(opts, false),
            (NumericalOptions opts) => RenamerInfo.Numerical(opts, false),
            (AlphabeticalOptions opts) => RenamerInfo.Alphabetical(opts, false),
            (ReverseOptions opts) => RenamerInfo.Reverse(opts, false),
            (ReplaceOptions opts) => RenamerInfo.Replace(opts, false),
            (UpperOptions opts) => RenamerInfo.Upper(opts, false),
            (LowerOptions opts) => RenamerInfo.Lower(opts, false),
            errs => new Info(new DirectoryInfo[prevDirsNames.Length], new FileInfo[prevFilesNames.Length]));

        return new string[][] { partialInfo.NewDirsNames, partialInfo.NewFilesNames };
    }

    public static string[][] HandlePatternText(string cmd, string[] prevDirsNames, string[] prevFilesNames)
    {
        var dirsNames = new string[prevDirsNames.Length];
        var filesNames = new string[prevFilesNames.Length];

        for (var i = 0; i < dirsNames.Length; i++)
        {
            dirsNames[i] = cmd;
        }

        for (var i = 0; i < filesNames.Length; i++)
        {
            filesNames[i] = cmd;
        }
        return new string[][] { dirsNames, filesNames };
    }

    // ---------------------------------------------------------------------------------------------------------
    static void RenameDir(string path, string newPath, string src, string distBase, string prefix, string suffix)
    {
        var dist = "";
        var dot = "";
        if (src.StartsWith(".")) dot = ".";
        if (distBase.StartsWith(".")) dot = "";

        distBase = RemoveDisallowedCharacters(distBase);
        prefix = RemoveDisallowedCharacters(prefix);
        suffix = RemoveDisallowedCharacters(suffix);

        dist = $"{dot}{prefix}{distBase}{suffix}";

        if (newPath != "")
        {
            dist = CheckExisting(newPath, dist, $"{dot}{prefix}", distBase, $"{suffix}");
            CopyDirectory(Path.Combine(path, src), Path.Combine(newPath, dist));
        }
        else
        {
            Directory.Move(Path.Combine(path, src), Path.Combine(path, dist));
        }
    }

    static void RenameFile(string path, string newPath, string src, string distBase, string prefix, string suffix)
    {
        var dist = "";
        var dot = "";
        if (src.StartsWith(".")) dot = ".";
        if (distBase.StartsWith(".")) dot = "";
        var ext = GetExtension(src);

        distBase = RemoveDisallowedCharacters(distBase);
        prefix = RemoveDisallowedCharacters(prefix);
        suffix = RemoveDisallowedCharacters(suffix);

        dist = $"{dot}{prefix}{distBase}{suffix}.{ext}";

        if (newPath != "")
        {
            dist = CheckExisting(newPath, dist, $"{dot}{prefix}", distBase, $"{suffix}.{ext}");
            File.Copy(Path.Combine(path, (string)src), Path.Combine(newPath, dist));
        }
        else
        {
            File.Move(Path.Combine(path, (string)src), Path.Combine(path, dist));
        }
    }

    public static void Rename(string path, string newPath, string[] prevDirsNames, string[] prevFilesNames, string[] newDirsNames, string[] newFilesNames, string prefix, string suffix)
    {
        for (var i = 0; i < newDirsNames.Length; i++)
        {
            RenameDir(path, newPath, prevDirsNames[i], newDirsNames[i], prefix, suffix);
        }

        for (var i = 0; i < newFilesNames.Length; i++)
        {
            RenameFile(path, newPath, prevFilesNames[i], newFilesNames[i], prefix, suffix);
        }
    }
}

class Info
{
    public string[] PrevDirsNames;
    public string[] PrevFilesNames;
    public string[] NewDirsNames;
    public string[] NewFilesNames;
    public Info(DirectoryInfo[] dirs, FileInfo[] files)
    {
        PrevDirsNames = dirs.Select(dir => dir.Name).ToArray();
        PrevFilesNames = files.Select(file => file.Name).ToArray();
        NewDirsNames = new string[dirs.Length];
        NewFilesNames = new string[files.Length];
    }
}
