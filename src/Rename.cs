
using Renamer.RenameUtils;
using Renamer.RenameInfo;

namespace Renamer;

class RenameMethods
{
    public static int Random(RandomOptions opts)
    {
        var info = RenamerInfo.Random(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int RandomForPattern(RandomOptionsForPattern opts)
    {
        var info = RenamerInfo.RandomForPattern(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int Numerical(NumericalOptions opts)
    {
        var info = RenamerInfo.Numerical(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int Alphabetical(AlphabeticalOptions opts)
    {
        var info = RenamerInfo.Alphabetical(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int Reverse(ReverseOptions opts)
    {
        var info = RenamerInfo.Reverse(opts);

        var revDirs = (string[])info.NewDirsNames.Clone(); var revFiles = (string[])info.NewFilesNames.Clone();
        if (opts.newPath == "" && !opts.notSafe)
        {
            RenameMethods.Temp(opts.path, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info = RenamerUtils.PrepareRename(opts.path, false, opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreDotFiles);
            info.NewDirsNames = revDirs;
            info.NewFilesNames = revFiles;
        }

        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int Replace(ReplaceOptions opts)
    {
        var info = RenamerInfo.Replace(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int Upper(UpperOptions opts)
    {
        var info = RenamerInfo.Upper(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int Lower(LowerOptions opts)
    {
        var info = RenamerInfo.Lower(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int Title(TitleOptions opts)
    {
        var info = RenamerInfo.Title(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, opts.prefix, opts.suffix);
        return 0;
    }

    public static int Pattern(PatternOptions opts)
    {
        var info = RenamerInfo.Pattern(opts);
        info = RenamerUtils.CheckSafety(opts.path, opts.newPath, info, opts.notSafe, opts.reverse,
            opts.ignoreDirs, opts.ignoreFiles, opts.ignoreDotDirs, opts.ignoreFiles);
        RenamerUtils.Rename(opts.path, opts.newPath, info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, "", "");
        return 0;
    }

    public static void Temp(string path, bool ignoreDirs, bool ignoreFiles, bool ignoreDotDirs, bool ignoreDotFiles)
    {
        var info = RenamerUtils.PrepareRename(path, false, ignoreDirs, ignoreFiles, ignoreDotDirs, ignoreDotFiles);
        var num = 1;
        var zeros = (info.PrevDirsNames.Length + info.PrevFilesNames.Length).ToString().Length;
        var prefix = Guid.NewGuid().ToString().Replace("-", "") + "-";

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var numToString = num.ToString($"D{zeros}");
            info.NewDirsNames[i] = numToString;
            num++;
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var numToString = num.ToString($"D{zeros}");
            info.NewFilesNames[i] = numToString;
            num++;
        }

        RenamerUtils.Rename(path, "", info.PrevDirsNames, info.PrevFilesNames, info.NewDirsNames, info.NewFilesNames, prefix, "");
    }
}
