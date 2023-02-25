
namespace Renamer;
class Renamer
{
    public static Info ApplySafety(Info info)
    {
        if (info.BaseOpts.newPath == "" && !info.BaseOpts.notSafe)
        {
            var newDirsNames = (string[])info.NewDirsNames.Clone(); var newFilesNames = (string[])info.NewFilesNames.Clone();
            Temp(info.BaseOpts.cloneForTempRename());
            info = NamesUtils.GetRenameInfo(info.BaseOpts);
            info.NewDirsNames = newDirsNames;
            info.NewFilesNames = newFilesNames;
        }
        return info;
    }

    public static void ApplyRenaming(Info info)
    {
        if (info.BaseOpts.path == info.BaseOpts.newPath)
        {
            Console.WriteLine("ERROR: path and new-path must be different");
            Environment.Exit(1);
        }
        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            RenameDir(info.BaseOpts, info.PrevDirsNames[i], info.NewDirsNames[i], 0);
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            RenameFile(info.BaseOpts, info.PrevFilesNames[i], info.NewFilesNames[i], 0);
        }
    }

    static void RenameDir(BaseOptsObj baseOpts, string src, string distBase, int n)
    {
        var dist = "";
        var dot = "";
        if (src.StartsWith(".")) dot = ".";
        if (distBase.StartsWith(".")) dot = "";

        distBase = RemoveDisallowedCharacters(distBase);
        baseOpts.prefix = RemoveDisallowedCharacters(baseOpts.prefix);
        baseOpts.suffix = RemoveDisallowedCharacters(baseOpts.suffix);

        dist = (n == 0) ? $"{dot}{baseOpts.prefix}{distBase}{baseOpts.suffix}" : $"{dot}{baseOpts.prefix}{distBase}{baseOpts.suffix} ({n})";
        if (baseOpts.newPath != "")
        {
            try
            {
                CopyDirectory(Path.Combine(baseOpts.path, src), Path.Combine(baseOpts.newPath, dist));
            }
            catch (System.IO.IOException)
            {
                RenameDir(baseOpts, src, distBase, n + 1);
            }
        }
        else
        {
            try
            {
                Directory.Move(Path.Combine(baseOpts.path, src), Path.Combine(baseOpts.path, dist));
            }
            catch (System.IO.IOException)
            {
                RenameDir(baseOpts, src, distBase, n + 1);
            }
        }
    }

    static void RenameFile(BaseOptsObj baseOpts, string src, string distBase, int n)
    {
        var dist = "";
        var dot = "";
        if (src.StartsWith(".")) dot = ".";
        if (distBase.StartsWith(".")) dot = "";
        var ext = GetExtension(src);

        distBase = RemoveDisallowedCharacters(distBase);
        baseOpts.prefix = RemoveDisallowedCharacters(baseOpts.prefix);
        baseOpts.suffix = RemoveDisallowedCharacters(baseOpts.suffix);

        dist = (n == 0) ? $"{dot}{baseOpts.prefix}{distBase}{baseOpts.suffix}.{ext}" : $"{dot}{baseOpts.prefix}{distBase}{baseOpts.suffix} ({n}).{ext}";
        if (baseOpts.newPath != "")
        {
            try
            {
                File.Copy(Path.Combine(baseOpts.path, (string)src), Path.Combine(baseOpts.newPath, dist));
            }
            catch (System.IO.IOException)
            {
                RenameFile(baseOpts, src, distBase, n + 1);
            }
        }
        else
        {
            try
            {
                File.Move(Path.Combine(baseOpts.path, (string)src), Path.Combine(baseOpts.path, dist));
            }
            catch (System.IO.IOException)
            {
                RenameFile(baseOpts, src, distBase, n + 1);
            }
        }
    }
    static void Temp(BaseOptsObj baseOpts)
    {

        var info = NamesUtils.GetRenameInfo(baseOpts);
        var num = 1;
        var zeros = (info.PrevDirsNames.Length + info.PrevFilesNames.Length).ToString().Length;
        var prefix = Guid.NewGuid().ToString().Replace("-", "") + "-";

        for (var i = 0; i < info.NewDirsNames.Length; i++)
        {
            var numToString = num.ToString($"D{zeros}");
            info.NewDirsNames[i] = prefix + numToString;
            num++;
        }

        for (var i = 0; i < info.NewFilesNames.Length; i++)
        {
            var numToString = num.ToString($"D{zeros}");
            info.NewFilesNames[i] = prefix + numToString;
            num++;
        }

        Renamer.ApplyRenaming(info);
    }

    // --------------------------------------------------------------------------------------------------------
    static string RemoveDisallowedCharacters(string name)
    {
        var disallowedChars = "\\/:*?\"'<>|";

        foreach (var ch in disallowedChars)
        {
            name = name.Replace(ch.ToString(), "");
        }
        return name;
    }

    static string GetExtension(string fileName)
    {
        var splitText = fileName.Split(".");
        var len = splitText.Length;

        if (fileName.StartsWith(".") && len > 2) return splitText.Last();
        if (!fileName.StartsWith(".") && len >= 2) return splitText.Last();
        return fileName;
    }

    static void CopyDirectory(string src, string dist)
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
}