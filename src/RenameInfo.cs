using Renamer.RenameUtils;
using System.Globalization;

namespace Renamer.RenameInfo;

class RenamerInfo
{
    public static Info Random(RandomOptions opts)
    {
        var info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = RenamerUtils.GenerateUUID(32);
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = RenamerUtils.GenerateUUID(32);
        }

        return info;
    }

    public static Info RandomForPattern(RandomOptionsForPattern opts)
    {
        var info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = RenamerUtils.GenerateUUID(opts.length);
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = RenamerUtils.GenerateUUID(opts.length);
        }

        return info;
    }

    public static Info Numerical(NumericalOptions opts)
    {
        var info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
        var num = opts.start;
        opts.zeros = Math.Max(opts.zeros, 0);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var numToString = num.ToString($"D{opts.zeros}");
            info.NewDirsNames[i] = numToString;
            num += opts.increment;
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var numToString = num.ToString($"D{opts.zeros}");
            info.NewFilesNames[i] = numToString;
            num += opts.increment;
        }

        return info;
    }

    public static Info NumericalForPattern(NumericalOptionsForPattern opts)
    {
        var info = RenamerUtils.PrepareRename(opts.path, opts.reverse, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
        var num = opts.start;
        opts.zeros = Math.Max(opts.zeros, 0);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var numToString = RenamerUtils.GenerateNumber(num, opts.start, opts.range, opts.every).ToString($"D{opts.zeros}");
            info.NewDirsNames[i] = numToString;
            num += opts.increment;
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var numToString = RenamerUtils.GenerateNumber(num, opts.start, opts.range, opts.every).ToString($"D{opts.zeros}");
            info.NewFilesNames[i] = numToString;
            num += opts.increment;
        }

        return info;
    }

    public static Info Alphabetical(AlphabeticalOptions opts)
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

        return info;
    }

    public static Info Reverse(ReverseOptions opts)
    {
        var info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
        var revDirs = info.PrevDirsNames.Reverse().ToArray();
        var revFiles = info.PrevFilesNames.Reverse().ToArray().Select(file => RenamerUtils.RemoveExtension(file)).ToArray();
        info.NewDirsNames = revDirs;
        info.NewFilesNames = revFiles;
        return info;
    }

    public static Info Replace(ReplaceOptions opts)
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

        return info;
    }

    public static Info Upper(UpperOptions opts)
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

        return info;
    }

    public static Info Lower(LowerOptions opts)
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

        return info;
    }

    public static Info Title(TitleOptions opts)
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

        return info;
    }

    public static Info Pattern(PatternOptions opts)
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
                else if (cmd.StartsWith("% "))
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

        return info;
    }
}