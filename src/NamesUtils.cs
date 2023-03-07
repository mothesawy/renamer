
using System.Globalization;
using System.Text.RegularExpressions;

namespace Renamer;

class NamesUtils
{
    public static Info GetRenameInfo(BaseOptsObj baseOpts)
    {
        var srcInfo = new DirectoryInfo(baseOpts.path);
        var dirs = srcInfo.GetDirectories().Select(dir => dir.Name).ToArray();
        var files = srcInfo.GetFiles().Select(file => file.Name).ToArray();

        if (baseOpts.ignoreDirs) dirs = new string[] { };
        if (baseOpts.ignoreFiles) files = new string[] { };

        if (baseOpts.ignoreDotDirs) dirs = Array.FindAll(dirs, dir => !dir.StartsWith("."));
        if (baseOpts.ignoreDotFiles) files = Array.FindAll(files, file => !file.StartsWith("."));

        var dirsList = dirs.ToList<string>();
        var filesList = files.ToList<string>();
        dirsList.Sort((a, b) => CompareNatural(a, b));
        filesList.Sort((a, b) => CompareNatural(a, b));
        dirs = dirsList.ToArray<string>();
        files = filesList.ToArray<string>();

        if (baseOpts.reverse)
        {
            files = files.Reverse().ToArray();
            dirs = dirs.Reverse().ToArray();
        }

        var info = new Info();
        info.PrevDirsNames = dirs;
        info.PrevFilesNames = files;
        info.NewDirsNames = new string[dirs.Length];
        info.NewFilesNames = new string[files.Length];
        info.BaseOpts = baseOpts;

        return info;
    }

    // --------------------------------------------------------------------------------------------------------
    public static string GetRandomName(int length)
    {
        var uuid = "";
        var len = Math.Min(Math.Max(length, 12), 32);
        uuid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, len);
        return uuid;
    }

    public static IEnumerable<string> GetNumericalName(int start, int increment, int zeros)
    {
        var value = start;
        while (true)
        {
            yield return value.ToString($"D{zeros}");
            value += increment;
        }
    }

    public static string GetAlphabeticalFromIndex(int index, bool upper)
    {
        var alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        var text = "";
        var mult = index / 26;
        var remainder = index - (mult * 26);

        for (var i = 0; i < mult; i++)
        {
            text += "z";
        }
        text += alpha[remainder];

        if (upper) text = text.ToUpper();

        return text;
    }

    public static int GetIndexFromAlphabetical(string text)
    {
        var alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        text = text.ToLower();

        var index = 0;

        var len = text.Length;
        if (len == 1) return Array.IndexOf(alpha, text.ToCharArray()[0]);

        for (var i = 0; i < text.Length; i++)
        {
            var c = text.ElementAt(i);
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

    public static string[][] HandlePatternRegex(string cmd, string nextCmd, string[] prevDirsNames, string[] prevFilesNames)
    {
        var regexCmd = cmd.Substring(2);
        var dirsNames = new string[prevDirsNames.Length];
        var filesNames = new string[prevFilesNames.Length];
        var index = -1;

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

    public static string[][] HandlePatternCommand(string cmd, PatternOptions parentOpts)
    {
        var renameCmd = (cmd.Substring(2) + $" -p {parentOpts.path}").Split(" ");
        var partialInfo = Parser.ParseAndGetNamesPattern(renameCmd, parentOpts);
        return new string[][] { partialInfo.NewDirsNames, partialInfo.NewFilesNames };
    }

    public static string[][] HandlePatternText(string cmd, int dirsArrLen, int filesArrLen)
    {
        var renameText = cmd.Substring(2);
        var dirsNames = new string[dirsArrLen];
        var filesNames = new string[filesArrLen];

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

    public static int GetNumericalNamePattern(int num, int start, IEnumerable<int>? optsRange, int every)
    {
        if (optsRange is not null)
        {
            var rng = optsRange.ToArray();
            if (rng.Length == 0 && every == 0) return num;
            if (rng.Length != 0 && every != 0)
            {
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

    // --------------------------------------------------------------------------------------------------------
    public static string RemoveExtension(string fileName)
    {
        var splitText = fileName.Split(".");
        var len = splitText.Length;

        if (fileName.StartsWith(".") && len > 2) return fileName.Replace("." + splitText.Last(), "");
        if (!fileName.StartsWith(".") && len >= 2) return fileName.Replace("." + splitText.Last(), "");
        return fileName;
    }

    static int CompareNatural(string strA, string strB)
    {
        var comparer = CultureInfo.CurrentCulture.CompareInfo;
        var options = CompareOptions.IgnoreCase;
        int iA = 0;
        int iB = 0;
        int softResult = 0;
        int softResultWeight = 0;
        while (iA < strA.Length && iB < strB.Length)
        {
            bool isDigitA = Char.IsDigit(strA[iA]);
            bool isDigitB = Char.IsDigit(strB[iB]);
            if (isDigitA != isDigitB)
            {
                return comparer.Compare(strA, iA, strB, iB, options);
            }
            else if (!isDigitA && !isDigitB)
            {
                int jA = iA + 1;
                int jB = iB + 1;
                while (jA < strA.Length && !Char.IsDigit(strA[jA])) jA++;
                while (jB < strB.Length && !Char.IsDigit(strB[jB])) jB++;
                int cmpResult = comparer.Compare(strA, iA, jA - iA, strB, iB, jB - iB, options);
                if (cmpResult != 0)
                {
                    // Certain strings may be considered different due to "soft" differences that are
                    // ignored if more significant differences follow, e.g. a hyphen only affects the
                    // comparison if no other differences follow
                    string sectionA = strA.Substring(iA, jA - iA);
                    string sectionB = strB.Substring(iB, jB - iB);
                    if (comparer.Compare(sectionA + "1", sectionB + "2", options) ==
                        comparer.Compare(sectionA + "2", sectionB + "1", options))
                    {
                        return comparer.Compare(strA, iA, strB, iB, options);
                    }
                    else if (softResultWeight < 1)
                    {
                        softResult = cmpResult;
                        softResultWeight = 1;
                    }
                }
                iA = jA;
                iB = jB;
            }
            else
            {
                char zeroA = (char)(strA[iA] - (int)Char.GetNumericValue(strA[iA]));
                char zeroB = (char)(strB[iB] - (int)Char.GetNumericValue(strB[iB]));
                int jA = iA;
                int jB = iB;
                while (jA < strA.Length && strA[jA] == zeroA) jA++;
                while (jB < strB.Length && strB[jB] == zeroB) jB++;
                int resultIfSameLength = 0;
                do
                {
                    isDigitA = jA < strA.Length && Char.IsDigit(strA[jA]);
                    isDigitB = jB < strB.Length && Char.IsDigit(strB[jB]);
                    int numA = isDigitA ? (int)Char.GetNumericValue(strA[jA]) : 0;
                    int numB = isDigitB ? (int)Char.GetNumericValue(strB[jB]) : 0;
                    if (isDigitA && (char)(strA[jA] - numA) != zeroA) isDigitA = false;
                    if (isDigitB && (char)(strB[jB] - numB) != zeroB) isDigitB = false;
                    if (isDigitA && isDigitB)
                    {
                        if (numA != numB && resultIfSameLength == 0)
                        {
                            resultIfSameLength = numA < numB ? -1 : 1;
                        }
                        jA++;
                        jB++;
                    }
                }
                while (isDigitA && isDigitB);
                if (isDigitA != isDigitB)
                {
                    // One number has more digits than the other (ignoring leading zeros) - the longer
                    // number must be larger
                    return isDigitA ? 1 : -1;
                }
                else if (resultIfSameLength != 0)
                {
                    // Both numbers are the same length (ignoring leading zeros) and at least one of
                    // the digits differed - the first difference determines the result
                    return resultIfSameLength;
                }
                int lA = jA - iA;
                int lB = jB - iB;
                if (lA != lB)
                {
                    // Both numbers are equivalent but one has more leading zeros
                    return lA > lB ? -1 : 1;
                }
                else if (zeroA != zeroB && softResultWeight < 2)
                {
                    softResult = comparer.Compare(strA, iA, 1, strB, iB, 1, options);
                    softResultWeight = 2;
                }
                iA = jA;
                iB = jB;
            }
        }
        if (iA < strA.Length || iB < strB.Length)
        {
            return iA < strA.Length ? 1 : -1;
        }
        else if (softResult != 0)
        {
            return softResult;
        }
        return 0;
    }
}

struct Info
{
    public string[] PrevDirsNames { get; set; }
    public string[] PrevFilesNames { get; set; }
    public string[] NewDirsNames { get; set; }
    public string[] NewFilesNames { get; set; }
    public BaseOptsObj BaseOpts { get; set; }
}