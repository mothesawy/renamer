using System.Diagnostics;
using CommandLine;
using CommandLine.Text;

namespace Renamer;


// pattern
// public and private
// testing
class Program
{
    public static void Main(string[] args)
    {
        var stw = new Stopwatch();
        stw.Start();
        var info = Parser.ParseAndGetNames(args);
        info = Renamer.ApplySafety(info);
        Renamer.ApplyRenaming(info);
        Console.WriteLine($"Operation Completed in {stw.ElapsedMilliseconds / 1000.0} second(s).");
    }
}

class Parser
{
    public static Info ParseAndGetNames(string[] args)
    {
        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<RandomOptions, NumericalOptions, AlphabeticalOptions,
                                           ReverseOptions, ReplaceOptions, UpperOptions,
                                           LowerOptions, TitleOptions, PatternOptions>(args);
        var data = new Info();

        result.WithParsed<RandomOptions>(opts => { data = Names.Random(opts); });
        result.WithParsed<NumericalOptions>(opts => { data = Names.Numerical(opts); });
        result.WithParsed<AlphabeticalOptions>(opts => { data = Names.Alphabetical(opts); });
        result.WithParsed<ReverseOptions>(opts => { data = Names.Reverse(opts); });
        result.WithParsed<ReplaceOptions>(opts => { data = Names.Replace(opts); });
        result.WithParsed<UpperOptions>(opts => { data = Names.Upper(opts); });
        result.WithParsed<LowerOptions>(opts => { data = Names.Lower(opts); });
        result.WithParsed<TitleOptions>(opts => { data = Names.Title(opts); });
        result.WithParsed<PatternOptions>(opts => { data = Names.Pattern(opts); });
        result.WithNotParsed(errs => DisplayHelp(result));

        return data;
    }

    public static Info ParseAndGetNamesPattern(string[] args, PatternOptions parentOpts)
    {
        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<RandomPatternOptions, NumericalPatternOptions, AlphabeticalOptions,
                                           ReverseOptions, ReplaceOptions, UpperOptions,
                                           LowerOptions, TitleOptions, PatternOptions>(args);
        var data = new Info();

        result.WithParsed<RandomPatternOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.RandomForPattern(opts); });
        result.WithParsed<NumericalPatternOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.NumericalForPattern(opts); });
        result.WithParsed<AlphabeticalOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.Alphabetical(opts); });
        result.WithParsed<ReverseOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.Reverse(opts); });
        result.WithParsed<ReplaceOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.Replace(opts); });
        result.WithParsed<UpperOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.Upper(opts); });
        result.WithParsed<LowerOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.Lower(opts); });
        result.WithParsed<TitleOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.Title(opts); });
        result.WithParsed<PatternOptions>(opts => { ResetOpts(parentOpts, opts); data = Names.Pattern(opts); });
        result.WithNotParsed(errs => DisplayHelp(result));

        return data;
    }

    static void ResetOpts(PatternOptions parentOpts, BaseOptions opts)
    {
        if (parentOpts.ignoreDirs) opts.ignoreDirs = true;
        if (parentOpts.ignoreDotDirs) opts.ignoreDirs = true;
        if (parentOpts.ignoreFiles) opts.ignoreDirs = true;
        if (parentOpts.ignoreDotFiles) opts.ignoreDirs = true;
    }

    static void DisplayHelp<T>(ParserResult<T> result)
    {
        var helpText = HelpText.AutoBuild(result, h =>
        {
            h.AdditionalNewLineAfterOption = false;
            h.Heading = "renamer -- A tool to batch rename multiple files and directories with different renaming methods";
            h.Copyright = "";
            return HelpText.DefaultParsingErrorsHandler(result, h);
        }, e => e);
        Console.WriteLine(helpText);
    }
}