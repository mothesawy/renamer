
using System.Globalization;

namespace Renamer;

static class Names
{
    public static Info Random(RandomOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = NamesUtils.GetRandomName(32);
        }
        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = NamesUtils.GetRandomName(32);
        }

        return info;
    }

    public static Info RandomForPattern(RandomPatternOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());

        if (opts.consistent)
        {
            var uuid = NamesUtils.GetRandomName(opts.length);
            for (var i = 0; i < info.NewDirsNames.Length; i++)
            {
                info.NewDirsNames[i] = uuid;
            }
            for (var i = 0; i < info.NewFilesNames.Length; i++)
            {
                info.NewFilesNames[i] = uuid;
            }
            return info;
        }

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = NamesUtils.GetRandomName(opts.length);
        }
        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = NamesUtils.GetRandomName(opts.length);
        }

        return info;
    }

    public static Info Numerical(NumericalOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());
        opts.zeros = Math.Max(opts.zeros, 0);

        var counter = NamesUtils.GetNumericalName(opts.start, opts.increment, opts.zeros).GetEnumerator();
        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            counter.MoveNext();
            info.NewDirsNames[i] = counter.Current;
        }
        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            counter.MoveNext();
            info.NewFilesNames[i] = counter.Current;
        }

        return info;
    }

    public static Info NumericalForPattern(NumericalPatternOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());
        var num = opts.start;
        opts.zeros = Math.Max(opts.zeros, 0);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var numToString = NamesUtils.GetNumericalNamePattern(num, opts.start, opts.range, opts.every).ToString($"D{opts.zeros}");
            info.NewDirsNames[i] = numToString;
            num += opts.increment;
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var numToString = NamesUtils.GetNumericalNamePattern(num, opts.start, opts.range, opts.every).ToString($"D{opts.zeros}");
            info.NewFilesNames[i] = numToString;
            num += opts.increment;
        }

        return info;
    }

    public static Info Alphabetical(AlphabeticalOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());
        var index = NamesUtils.GetIndexFromAlphabetical(opts.start);

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var numToString = NamesUtils.GetAlphabeticalFromIndex(index, opts.upper);
            info.NewDirsNames[i] = numToString;
            index++;
        }
        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var numToString = NamesUtils.GetAlphabeticalFromIndex(index, opts.upper);
            info.NewFilesNames[i] = numToString;
            index++;
        }

        return info;
    }

    public static Info Reverse(ReverseOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());

        var revDirs = info.PrevDirsNames.Reverse().ToArray();
        var revFiles = info.PrevFilesNames.Reverse().ToArray().Select(file => NamesUtils.RemoveExtension(file)).ToArray();
        info.NewDirsNames = revDirs;
        info.NewFilesNames = revFiles;

        return info;
    }

    public static Info Replace(ReplaceOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = info.PrevDirsNames[i].Replace(opts.from, opts.to);
        }
        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = NamesUtils.RemoveExtension(info.PrevFilesNames[i]).Replace(opts.from, opts.to);
        }

        return info;
    }

    public static Info Upper(UpperOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = info.PrevDirsNames[i].ToUpper();
        }
        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = NamesUtils.RemoveExtension(info.PrevFilesNames[i]).ToUpper();
        }

        return info;
    }

    public static Info Lower(LowerOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = info.PrevDirsNames[i].ToLower();
        }
        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = NamesUtils.RemoveExtension(info.PrevFilesNames[i]).ToLower();
        }

        return info;
    }

    public static Info Title(TitleOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());
        TextInfo TC = new CultureInfo("en", false).TextInfo;

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            info.NewDirsNames[i] = $"{TC.ToTitleCase(info.PrevDirsNames[i])}";
        }
        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            info.NewFilesNames[i] = $"{TC.ToTitleCase(NamesUtils.RemoveExtension(info.PrevFilesNames[i]))}"; ;
        }

        return info;
    }

    public static Info Pattern(PatternOptions opts)
    {
        var info = NamesUtils.GetRenameInfo(opts.GetBaseOptions());

        var dirsNamesParts = new List<string[]?>();
        var filesNamesParts = new List<string[]?>();

        if (opts.pattern is not null)
        {
            var pattern = opts.pattern.ToArray();
            for (var i = 0; i < pattern.Length; i++)
            {
                var cmd = pattern[i];
                if (cmd.StartsWith("## "))
                {
                    // regex group
                    continue;
                }
                else if (cmd.StartsWith("# "))
                {
                    // regex
                    var nextCmd = "";
                    if (i + 1 < pattern.Length)
                    {
                        nextCmd = pattern[i + 1];
                    }

                    var regexResult = NamesUtils.HandlePatternRegex(cmd, nextCmd, info.PrevDirsNames, info.PrevFilesNames);
                    dirsNamesParts.Add(regexResult[0]);
                    filesNamesParts.Add(regexResult[1]);
                }
                else if (cmd.StartsWith("$ "))
                {
                    // command
                    var commandResult = NamesUtils.HandlePatternCommand(cmd, opts, opts.path, info.PrevDirsNames, info.PrevFilesNames);
                    dirsNamesParts.Add(commandResult[0]);
                    filesNamesParts.Add(commandResult[1]);
                }
                else if (cmd.StartsWith("% "))
                {
                    // normal text
                    var textResult = NamesUtils.HandlePatternText(cmd, info.PrevDirsNames.Length, info.PrevFilesNames.Length);
                    dirsNamesParts.Add(textResult[0]);
                    filesNamesParts.Add(textResult[1]);
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
