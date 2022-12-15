using Renamer.RenameUtils;
using System.Globalization;
using Renamer;

namespace Renamer.RenameInfo;

class RenamerInfo
{
    public static Info Random(RandomOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            info.NewDirsNames[i] = uuid;
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var uuid = Guid.NewGuid().ToString().Replace("-", "");
            info.NewFilesNames[i] = uuid;
        }

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }

        return info;
    }

    public static Info Numerical(NumericalOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
        var num = opts.start;

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var numToString = num.ToString($"D{opts.zeros}");
            info.NewDirsNames[i] = numToString;
            num++;
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var numToString = num.ToString($"D{opts.zeros}");
            info.NewFilesNames[i] = numToString;
            num++;
        }

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }

        return info;
    }

    public static Info Alphabetical(AlphabeticalOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
        var index = RenamerUtils.ConvertStringToIndex(opts.start);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var numToString = RenamerUtils.ConvertIndexToString(index, opts.upper);
            info.NewDirsNames[i] = numToString;
            index++;
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var numToString = RenamerUtils.ConvertIndexToString(index, opts.upper);
            info.NewFilesNames[i] = numToString;
            index++;
        }

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }

        return info;
    }

    public static Info Reverse(ReverseOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
        var revDirs = info.PrevDirsNames.Reverse().ToArray(); var revFiles = info.PrevFilesNames.Reverse().ToArray();

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
        }

        info.NewDirsNames = revDirs;
        info.NewFilesNames = revFiles;

        return info;
    }

    public static Info Replace(ReplaceOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = info.PrevDirsNames[i].Replace(opts.from, opts.to);
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = RenamerUtils.RemoveExtension(info.PrevFilesNames[i]).Replace(opts.from, opts.to);
        }

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }

        return info;
    }

    public static Info Upper(UpperOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = info.PrevDirsNames[i].ToUpper();
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = RenamerUtils.RemoveExtension(info.PrevFilesNames[i]).ToUpper();
        }

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }

        return info;
    }

    public static Info Lower(LowerOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = info.PrevDirsNames[i].ToLower();
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = RenamerUtils.RemoveExtension(info.PrevFilesNames[i]).ToLower();
        }

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }

        return info;
    }

    public static Info Title(TitleOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = $"{myTI.ToTitleCase(info.PrevDirsNames[i])}";
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = $"{myTI.ToTitleCase(RenamerUtils.RemoveExtension(info.PrevFilesNames[i]))}"; ;
        }

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }

        return info;
    }

    public static Info Pattern(PatternOptions opts, bool fromCmd)
    {
        var info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);

        var dirsNamesParts = new List<string[]?>();
        var filesNamesParts = new List<string[]?>();

        if (opts.pattern is not null)
        {
            var pattern = opts.pattern.ToArray();
            for (var j = 0; j < pattern.Length; j++)
            {
                var cmd = pattern[j];
                if (cmd.StartsWith("## "))
                {
                    continue;
                }
                else if (cmd.StartsWith("# "))
                {
                    // regex
                    var nextCmd = "";
                    if (j + 1 < pattern.Length)
                    {
                        nextCmd = pattern[j + 1];
                    }

                    var regexResult = RenamerUtils.HandlePatternRegex(cmd, nextCmd, info.PrevDirsNames, info.PrevFilesNames);
                    dirsNamesParts.Add(regexResult[0]);
                    filesNamesParts.Add(regexResult[1]);
                }
                else if (cmd.StartsWith("$ "))
                {
                    // command
                    var regexResult = RenamerUtils.HandlePatternCommand(cmd, opts.path, info.PrevDirsNames, info.PrevFilesNames);
                    dirsNamesParts.Add(regexResult[0]);
                    filesNamesParts.Add(regexResult[1]);
                }
                else
                {
                    // normal text
                    var regexResult = RenamerUtils.HandlePatternText(cmd, info.PrevDirsNames, info.PrevFilesNames);
                    dirsNamesParts.Add(regexResult[0]);
                    filesNamesParts.Add(regexResult[1]);
                }
            }

            for (var i = 0; i < info.NewDirsNames.Length; i++)
            {
                var newDirName = "";
                foreach (var part in dirsNamesParts.ToArray())
                {
                    newDirName += part?[i];
                }
                info.NewDirsNames[i] = (newDirName != "") ? newDirName : info.PrevDirsNames[i];
            }

            for (var i = 0; i < info.NewFilesNames.Length; i++)
            {
                var newFileName = "";
                foreach (var part in filesNamesParts.ToArray())
                {
                    newFileName += part?[i];
                }
                info.NewFilesNames[i] = (newFileName != "") ? newFileName : info.PrevFilesNames[i];
            }
        }

        if (opts.newPath == "" && !opts.notSafe && fromCmd)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }

        return info;
    }
}