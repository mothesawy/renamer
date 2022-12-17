using CommandLine;

namespace Renamer;

// TODO: Recurrsive Renaming
// TODO: Add types flag(s)
// TODO: Photos tags
// TODO: Audio files tags
// TODO: Video files tags

class BaseOptions
{
    [Option('p', "path", Required = true, HelpText = "Path of the files to rename.")]
    public string path { get; set; } = default!;

    [Option('n', "new-path", Required = false, HelpText = "New path to rename the files in.")]
    public string newPath { get; set; } = "";

    [Option("prefix", Required = false, HelpText = "Prefix before filenames.")]
    public string prefix { get; set; } = "";

    [Option("suffix", Required = false, HelpText = "Suffix after filenames.")]
    public string suffix { get; set; } = "";

    [Option("not-safe", Default = false, Required = false, HelpText = "Renaming without checking for naming conflicts (faster but may cause data loss).")]
    public bool notSafe { get; set; }

    [Option("ignore-dirs", Default = false, Required = false, HelpText = "Exclude directories from renaming.")]
    public bool ignoreDirs { get; set; }

    [Option("ignore-hidden-files", Default = false, Required = false, HelpText = "Exclude files from renaming.")]
    public bool ignoreFiles { get; set; }

    [Option("ignore-hidden-dirs", Default = false, Required = false, HelpText = "Exclude directories that starts with '.' from renaming.")]
    public bool ignoreDotDirs { get; set; }

    [Option("ignore-files", Default = false, Required = false, HelpText = "Exclude files that starts with '.' from renaming.")]
    public bool ignoreDotFiles { get; set; }
}

class BaseOptionsWithReverse : BaseOptions
{
    [Option('r', "reverse", Default = false, Required = false, HelpText = "Reverse the order of files.")]
    public bool reverse { get; set; }
}

[Verb("random", HelpText = "Rename items randomly using UUID V4.")]
class RandomOptions : BaseOptionsWithReverse
{
}

[Verb("random", HelpText = "Rename items randomly using UUID V4.")]
class RandomOptionsForPattern : RandomOptions
{
    [Option('l', "length", Default = 32, Required = false, HelpText = "Length of the random name (min: 12, max: 32). (With pattern mode only)")]
    public int length { get; set; }
}

[Verb("numerical", HelpText = "Numerical renaming for items (start, start + 1, start + 2, ...).")]
class NumericalOptions : BaseOptionsWithReverse
{
    [Option('s', "start", Default = 1, Required = false, HelpText = "Start renaming with this value.")]
    public int start { get; set; }

    [Option('i', "increment", Default = 1, Required = false, HelpText = "Increase number by this value.")]
    public int increment { get; set; }

    [Option('z', "zeros", Default = 0, Required = false, HelpText = "Leading zeros of names.")]
    public int zeros { get; set; }
}

[Verb("numerical", HelpText = "Numerical renaming for items (start, start + 1, start + 2, ...).")]
class NumericalOptionsForPattern : NumericalOptions
{
    [Option("range", Required = false, HelpText = "A range to repeat the numbers within")]
    public IEnumerable<int>? range { get; set; }

    [Option("every", Default = 0, Required = false, HelpText = "Increace The number every nth iteration. (With pattern mode only)")]
    public int every { get; set; }
}

[Verb("alphabetical", HelpText = "Alphabetical: Alphabetical renaming for items (a, b, c, ..., z, za, zb, ...).")]
class AlphabeticalOptions : BaseOptionsWithReverse
{
    [Option('u', "upper", Default = false, Required = false, HelpText = "Convert the generated name to upper case.")]
    public bool upper { get; set; }

    [Option('s', "start", Default = "a", Required = false, HelpText = "Start renaming with this value.")]
    public string start { get; set; } = "a";
}

[Verb("reverse", HelpText = "Reverse the order of the items.")]
class ReverseOptions : BaseOptions
{
}

[Verb("replace", HelpText = "Replace part of text in items names with another text.")]
class ReplaceOptions : BaseOptionsWithReverse
{
    [Option("from", Required = true, HelpText = "The text to be replaced.")]
    public string from { get; set; } = "";

    [Option("to", Default = "", Required = false, HelpText = "The text to be replaced.")]
    public string to { get; set; } = "";
}

[Verb("upper", HelpText = "Convert the name of the items to upper case.")]
class UpperOptions : BaseOptionsWithReverse
{
}

[Verb("lower", HelpText = "Convert the name of the items to lower case.")]
class LowerOptions : BaseOptionsWithReverse
{
}

[Verb("title", HelpText = "Convert the name of the items to title case.")]
class TitleOptions : BaseOptionsWithReverse
{
}

[Verb("pattern", HelpText = "Perform renaming with a pattern of text for items' names.")]
class PatternOptions : BaseOptionsWithReverse
{
    [Option("pattern", Required = true, HelpText = "A patern to apply while renaming.")]
    public IEnumerable<string>? pattern { get; set; }
}