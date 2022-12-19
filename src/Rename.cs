
using Renamer.RenameUtils;
using Renamer.RenameInfo;

namespace Renamer;

class RenameMethods
{
    public static int Random(RandomOptions opts)
    {
        var info = RenamerInfo.Random(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int RandomForPattern(RandomOptionsForPattern opts)
    {
        var info = RenamerInfo.RandomForPattern(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int Numerical(NumericalOptions opts)
    {
        var info = RenamerInfo.Numerical(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int Alphabetical(AlphabeticalOptions opts)
    {
        var info = RenamerInfo.Alphabetical(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int Reverse(ReverseOptions opts)
    {
        var info = RenamerInfo.Reverse(opts);

        var revDirs = (string[])info.NewDirsNames.Clone(); var revFiles = (string[])info.NewFilesNames.Clone();
        if (opts.newPath == "" && !opts.notSafe)
        {
            RenameMethods.Temp(opts.GetBaseOptions().cloneForTempRename());
            info = RenamerUtils.PrepareRename(opts.GetBaseOptions());
            info.NewDirsNames = revDirs;
            info.NewFilesNames = revFiles;
        }

        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int Replace(ReplaceOptions opts)
    {
        var info = RenamerInfo.Replace(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int Upper(UpperOptions opts)
    {
        var info = RenamerInfo.Upper(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int Lower(LowerOptions opts)
    {
        var info = RenamerInfo.Lower(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int Title(TitleOptions opts)
    {
        var info = RenamerInfo.Title(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static int Pattern(PatternOptions opts)
    {
        var info = RenamerInfo.Pattern(opts);
        info = RenamerUtils.CheckSafety(opts.GetBaseOptions(), info);
        RenamerUtils.Rename(opts.GetBaseOptions(), info);
        return 0;
    }

    public static void Temp(BaseOptsObj baseOpts)
    {

        var info = RenamerUtils.PrepareRename(baseOpts);
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

        RenamerUtils.Rename(baseOpts, info);
    }
}

