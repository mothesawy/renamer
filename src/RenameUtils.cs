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

    public static string GenerateUUID(int length)
    {
        var uuid = "";
        var len = Math.Min(Math.Max(length, 12), 32);
        uuid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, len);
        return uuid;
    }

    public static int GenerateNumber(int num, int start, IEnumerable<int>? optsRange, int every)
    {
        if (optsRange is not null)
        {
            var rng = optsRange.ToArray();
            if (rng.Length == 0 && every == 0) return num;
            if (rng.Length != 0 && every != 0)
            {
                System.Console.WriteLine(rng.ToArray().Length);
                Console.WriteLine("ERROR: You can't use both 'range' and 'every'.");
                Environment.Exit(1);
            }
            if (rng.Length != 0 && every == 0)
            {
                int[] range;
                if (rng.Length != 2)
                {
                    Console.WriteLine("ERROR: Range must be between 2 positive integer values.");
                    Environment.Exit(1);
                }
                var rangeStart = Math.Abs(rng[0]);
                var rangeEnd = Math.Abs(rng[1]);
                range = new int[Math.Abs(rangeEnd - rangeStart) + 1];

                if (rangeStart == rangeEnd)
                {
                    Console.WriteLine("ERROR: Range start and end must be different.");
                    Environment.Exit(1);
                }
                else
                {
                    var accum = Math.Min(rangeStart, rangeEnd);
                    for (var i = 0; i < range.Length; i++)
                    {
                        range[i] = accum + i;
                    }
                    if (rangeStart > rangeEnd) range = range.Reverse().ToArray();
                    if (!range.Contains(start))
                    {
                        Console.WriteLine("ERROR: Start value must be within range.");
                        Environment.Exit(1);
                    }
                }

                return range[(num - start) % range.Length];
            }
            else if (rng.Length == 0 && every != 0)
            {
                every = Math.Abs(every);
                if (every == 1) return num;
                return start + ((num - start) / every);
            }
        }
        return num;
    }

    // ---------------------------------------------------------------------------------------------------------

    public static void CopyDirectory(string src, string dist)
    {
        var srcDirInfo = new DirectoryInfo(src);
        if (!srcDirInfo.Exists)
            throw new DirectoryNotFoundException($"ERROR: Source directory not found: {srcDirInfo.FullName}");

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

    public static Info CheckSafety(string path, string newPath, Info info, bool notSafe, bool reverse, bool ignoreDirs, bool ignoreFiles, bool ignoreDotDirs, bool ignoreDotFiles)
    {
        if (newPath == "" && !notSafe)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(path, ignoreDirs, ignoreFiles, ignoreDotDirs, ignoreDotFiles);
            info = RenamerUtils.PrepareRename(path, reverse, ignoreDirs, ignoreFiles, ignoreDotDirs, ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
            return info;
        }
        return info;
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
            catch (System.FormatException)
            {
                Console.WriteLine("ERROR: Invalid group index");
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
            RandomOptionsForPattern,
            NumericalOptionsForPattern,
            AlphabeticalOptions,
            ReverseOptions,
            ReplaceOptions,
            UpperOptions,
            LowerOptions>(renameCmd.Split(" "))
        .MapResult(
            (RandomOptionsForPattern opts) => RenamerInfo.RandomForPattern(opts),
            (NumericalOptionsForPattern opts) => RenamerInfo.NumericalForPattern(opts),
            (AlphabeticalOptions opts) => RenamerInfo.Alphabetical(opts),
            (ReverseOptions opts) => RenamerInfo.Reverse(opts),
            (ReplaceOptions opts) => RenamerInfo.Replace(opts),
            (UpperOptions opts) => RenamerInfo.Upper(opts),
            (LowerOptions opts) => RenamerInfo.Lower(opts),
            errs => new Info(new DirectoryInfo[prevDirsNames.Length], new FileInfo[prevFilesNames.Length]));

        return new string[][] { partialInfo.NewDirsNames, partialInfo.NewFilesNames };
    }

    public static string[][] HandlePatternText(string cmd, string[] prevDirsNames, string[] prevFilesNames)
    {
        var renameText = cmd.Substring(2);
        var dirsNames = new string[prevDirsNames.Length];
        var filesNames = new string[prevFilesNames.Length];

        for (var i = 0; i < dirsNames.Length; i++)
        {
            dirsNames[i] = renameText;
        }

        for (var i = 0; i < filesNames.Length; i++)
        {
            filesNames[i] = renameText;
        }
        return new string[][] { dirsNames, filesNames };
    }

    // ---------------------------------------------------------------------------------------------------------
    static void RenameDir(string path, string newPath, string src, string distBase, string prefix, string suffix, int n)
    {
        var dist = "";
        var dot = "";
        if (src.StartsWith(".")) dot = ".";
        if (distBase.StartsWith(".")) dot = "";

        distBase = RemoveDisallowedCharacters(distBase);
        prefix = RemoveDisallowedCharacters(prefix);
        suffix = RemoveDisallowedCharacters(suffix);

        dist = (n == 0) ? $"{dot}{prefix}{distBase}{suffix}" : $"{dot}{prefix}{distBase}{suffix} ({n})";
        if (newPath != "")
        {
            try
            {
                CopyDirectory(Path.Combine(path, src), Path.Combine(newPath, dist));
            }
            catch (System.IO.IOException)
            {
                RenameDir(path, newPath, src, distBase, prefix, suffix, n + 1);
            }
        }
        else
        {
            try
            {
                Directory.Move(Path.Combine(path, src), Path.Combine(path, dist));
            }
            catch (System.IO.IOException)
            {
                RenameDir(path, newPath, src, distBase, prefix, suffix, n + 1);
            }
        }
    }

    static void RenameFile(string path, string newPath, string src, string distBase, string prefix, string suffix, int n)
    {
        var dist = "";
        var dot = "";
        if (src.StartsWith(".")) dot = ".";
        if (distBase.StartsWith(".")) dot = "";
        var ext = GetExtension(src);

        distBase = RemoveDisallowedCharacters(distBase);
        prefix = RemoveDisallowedCharacters(prefix);
        suffix = RemoveDisallowedCharacters(suffix);

        dist = (n == 0) ? $"{dot}{prefix}{distBase}{suffix}.{ext}" : $"{dot}{prefix}{distBase}{suffix} ({n}).{ext}";
        if (newPath != "")
        {
            try
            {
                File.Copy(Path.Combine(path, (string)src), Path.Combine(newPath, dist));
            }
            catch (System.IO.IOException)
            {
                RenameFile(path, newPath, src, distBase, prefix, suffix, n + 1);
            }
        }
        else
        {
            try
            {
                File.Move(Path.Combine(path, (string)src), Path.Combine(path, dist));
            }
            catch (System.IO.IOException)
            {
                RenameFile(path, newPath, src, distBase, prefix, suffix, n + 1);
            }
        }
    }

    public static void Rename(string path, string newPath, string[] prevDirsNames, string[] prevFilesNames, string[] newDirsNames, string[] newFilesNames, string prefix, string suffix)
    {
        if (path == newPath)
        {
            Console.WriteLine("ERROR: path and new-path must be different");
            Environment.Exit(1);
        }
        for (var i = 0; i < newDirsNames.Length; i++)
        {
            RenameDir(path, newPath, prevDirsNames[i], newDirsNames[i], prefix, suffix, 0);
        }

        for (var i = 0; i < newFilesNames.Length; i++)
        {
            RenameFile(path, newPath, prevFilesNames[i], newFilesNames[i], prefix, suffix, 0);
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
