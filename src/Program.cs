
using System.Diagnostics;
using CommandLine;
namespace Renamer;

class Program
{
    static int Main(string[] args)
    {
        Stopwatch st = new();
        st.Start();
        var result = CommandLine.Parser.Default.ParseArguments<
                RandomOptions,
                NumericalOptions,
                AlphabeticalOptions,
                ReverseOptions,
                ReplaceOptions,
                UpperOptions,
                LowerOptions,
                TitleOptions,
                PatternOptions>(args)
            .MapResult(
                (RandomOptions opts) => RenameMethods.Random(opts),
                (NumericalOptions opts) => RenameMethods.Numerical(opts),
                (AlphabeticalOptions opts) => RenameMethods.Alphabetical(opts),
                (ReverseOptions opts) => RenameMethods.Reverse(opts),
                (ReplaceOptions opts) => RenameMethods.Replace(opts),
                (UpperOptions opts) => RenameMethods.Upper(opts),
                (LowerOptions opts) => RenameMethods.Lower(opts),
                (TitleOptions opts) => RenameMethods.Title(opts),
                (PatternOptions opts) => RenameMethods.Pattern(opts),
                errs => 1);
        st.Stop();
        if (result == 0) Console.WriteLine($"Operation Completed. Took {st.ElapsedMilliseconds / 1000.0} second(s)");
        return result;
    }

}