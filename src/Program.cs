
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;

namespace Renamer;

class Program
{
  static void Main(string[] args)
  {
    Stopwatch st = new();
    st.Start();

    var parser = new CommandLine.Parser(with => with.HelpWriter = null);
    var result = parser.ParseArguments<
        RandomOptions,
        NumericalOptions,
        AlphabeticalOptions,
        ReverseOptions,
        ReplaceOptions,
        UpperOptions,
        LowerOptions,
        TitleOptions,
        PatternOptions>(args);
    result
      .WithParsed<RandomOptions>(opts =>
      {
        RenameMethods.Random(opts);
      })
      .WithParsed<NumericalOptions>(opts =>
      {
        RenameMethods.Numerical(opts);
      })
      .WithParsed<AlphabeticalOptions>(opts =>
      {
        RenameMethods.Alphabetical(opts);
      })
      .WithParsed<ReverseOptions>(opts =>
      {
        RenameMethods.Reverse(opts);
      })
      .WithParsed<ReplaceOptions>(opts =>
      {
        RenameMethods.Replace(opts);
      })
      .WithParsed<UpperOptions>(opts =>
      {
        RenameMethods.Upper(opts);
      })
      .WithParsed<LowerOptions>(opts =>
      {
        RenameMethods.Lower(opts);
      })
      .WithParsed<TitleOptions>(opts =>
      {
        RenameMethods.Title(opts);
      })
      .WithParsed<PatternOptions>(opts =>
      {
        RenameMethods.Pattern(opts);
      })
      .WithNotParsed(errs => DisplayHelp(result));

    st.Stop();
    if (result.Errors.ToArray().Length == 0) Console.WriteLine($"Operation Completed. Took {st.ElapsedMilliseconds / 1000.0} second(s)");
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

// var result = CommandLine.Parser.Default
// .ParseArguments<
//     RandomOptions,
//     NumericalOptions,
//     AlphabeticalOptions,
//     ReverseOptions,
//     ReplaceOptions,
//     UpperOptions,
//     LowerOptions,
//     TitleOptions,
//     PatternOptions>(args)
// .MapResult(
//     (RandomOptions opts) => RenameMethods.Random(opts),
//     (NumericalOptions opts) => RenameMethods.Numerical(opts),
//     (AlphabeticalOptions opts) => RenameMethods.Alphabetical(opts),
//     (ReverseOptions opts) => RenameMethods.Reverse(opts),
//     (ReplaceOptions opts) => RenameMethods.Replace(opts),
//     (UpperOptions opts) => RenameMethods.Upper(opts),
//     (LowerOptions opts) => RenameMethods.Lower(opts),
//     (TitleOptions opts) => RenameMethods.Title(opts),
//     (PatternOptions opts) => RenameMethods.Pattern(opts),
//     errs => 1);