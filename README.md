# **Renamer**

A tool to batch rename multiple files and directories with different renaming methods, written in C#!

## features:

- This tool can use both relative paths and absolute paths.
- supports both Windows and Unix path separators ("/", "\\").
- Bulk rename files in place or copy to another path.
- Random renaming mode using uuidv4.
- Ordered numerical renaming.
- Ordered alphabetical renaming.
- Reverse the order of items names.
- Replace parts of text with another part.
- Convert names to upper case.
- Convert names to lower case.
- Convert names to title case.
- Advanced renaming using patterns (regex and other renaming types).
- Ability to add leading zeros (padding) to numbers.
- Ability to add prefix to filenames.
- Ability to add suffix to filenames.

#### It's quite stable but needs more testing so use cautiously.

## Installation:
- Download binaries from _Releases_ page or clone and build it yourself.
- You can add the binary to your path to run it from anywhere.
- This tool __does not__ require _.net core_ to be installed. It's self contained.
- Run `renamer --help` for more info.

## Usage:

>**renamer**

- **Usage:** `renamer [command] [flags]`

- **Available Commands:**

    - `random`          Rename items randomly using UUID V4.

    - `numerical`       Numerical renaming for items (start, start + 1, start + 2, ...).

    - `alphabetical`    Alphabetical: Alphabetical renaming for items (a, b, c, ..., z, za, zb, ...).

    - `reverse`         Reverse the order of the items.

    - `replace`         Replace part of text in items names with another text.

    - `upper`           Convert the name of the items to upper case.

    - `lower`           Convert the name of the items to lower case.
    
    - `title`           Convert the name of the items to title case.
    
    - `pattern`         Perform renaming with a pattern of text for items' names.
    
    - `help`            Display more information on a specific command.
    

- **Flags:**

    - `--help`          Display more information on a specific command.


>**random**

Rename items randomly using UUID V4.

- **Usage:** `renamer random [flags]`

- **Flags:**
    - `-r`, `--reverse`          (Default: false) Reverse the order of files.

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Rename items randomly:

    `renamer random -p "path/to/your/files"`
    
    - Rename items randomly to new path with prefix "2022-":

    `renamer random -p "path/to/your/files" -n "new/path/for/your/files" --prefix "2022- "`


>**numerical**

Numerical renaming for items (start, start + 1, start + 2, ...).

- **Usage:** `renamer numerical [flags]`

- **Flags:**
    - `-s`, `--start`            (Default: 1) Start renaming with this value.

    - `-i`, `--increment`        (Default: 1) Increase number by this value.

    - `-z`, `--zeros`            (Default: 0) Leading zeros of names.

    - `-r`, `--reverse`          (Default: false) Reverse the order of files.

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Rename items numerically in the current directory with start = 4, padding-zeros = 3, suffix = "some text" and rename not-safe:

    `renamer numerical -p "." -s 5 -z 3 --not-safe --suffix "some text"`

    - Rename items numerically to a new path with padding-zeros = 2 and ignore files sttarting with ".":

    `renamer numerical -p "." --new-path "../new folder" --zeros 2 --ignore-dot-dirs`


>**alphabetical**

Alphabetical renaming for items (a, b, c, ..., z, za, zb, ...).

- **Usage:** `renamer alphabetical [flags]`

- **Flags:**
    - `-u`, `--upper`            (Default: false) Convert the generated name to upper case.

    - `-s`, `--start`            (Default: a) Start renaming with this value.

    - `-r`, `--reverse`          (Default: false) Reverse the order of files.

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Rename items alphabetically with start = "c", ignoring directories:

    `renamer alphabetical -p "path/to/your/files" -s "c" --ignore-dirs`

    - Rename items alphabetically to a new path and make letters upper case:

    `renamer numerical -p "path/to/your/files" --new-path "new/path/for/your/files" --upper`


>**reverse**

Reverse the order of the items.

- **Usage:** `renamer reverse [flags]`

- **Flags:**

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Reverse the order of items in a specific path:

    `renamer reverse -p "path/to/your/files"`

    - Copy the items in some path to a new path and reverse their order:

    `renamer reverse -p "." --new-path "../new folder" --ignore-files`

>**replace**

Replace part of text in items names with another text.

- **Usage:** `renamer replace [flags]`

- **Flags:**

    - `--from`                   Required. The text to be replaced.

    - `--to`                     (Default: ) The text to be replaced.

    - `-r`, `--reverse`          (Default: false) Reverse the order of files.

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Replace "a" with "aBcD" in items' names:

    `renamer reverse -p "path/to/your/files" --from "a" --to "aBcD"`

    - Replace underscores with space in items' names:

    `renamer reverse -p "path/to/your/files" --from "_" --to " "`

    - Remove hyphens from items' names and copy them to a new path:

    `renamer reverse -p "path/to/your/files" -n "new/path/for/your/files" --from "-"`


>**upper**

Convert names to upper case.

- **Usage:** `renamer upper [flags]`

- **Flags:**

    - `-r`, `--reverse`          (Default: false) Reverse the order of files.

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Convert names to upper case in a path:

    `renamer upper -p "path/to/your/files"`

    - Convert names to upper case and copy to a new path:

    `renamer upper -p "path/to/your/files" -n "new/path/for/your/files"`


>**lower**

Convert names to lower case.

- **Usage:** `renamer lower [flags]`

- **Flags:**

    - `-r`, `--reverse`          (Default: false) Reverse the order of files.

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Convert names to lower case in a path:

    `renamer lower -p "path/to/your/files"`

    - Convert names to lower case and copy to a new path:

    `renamer lower -p "path/to/your/files" -n "new/path/for/your/files"`


>**title**

Convert names to title case.

- **Usage:** `renamer title [flags]`

- **Flags:**

    - `-r`, `--reverse`          (Default: false) Reverse the order of files.

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Convert names to title case in a path:

    `renamer title -p "path/to/your/files"`

    - Convert names to title case and copy to a new path:

    `renamer title -p "path/to/your/files" -n "new/path/for/your/files"`


>**pattern**

Perform renaming with a pattern of text for items' names.

- **Usage:** `renamer patern [flags]`

- **Flags:**

    - `--patern`                 Required. A patern to apply while renaming.

    - `-r`, `--reverse`          (Default: false) Reverse the order of files.

    - `-p`, `--path`             Required. Path of the files to rename.

    - `-n`, `--new-path`         New path to rename the files in.

    - `--prefix`                 Prefix before filenames.

    - `--suffix`                 Suffix after filenames.

    - `--not-safe`               (Default: false) Renaming without checking for naming conflicts (faster but may cause data loss).

    - `--ignore-dirs`            (Default: false) Exclude directories from renaming.

    - `--ignore-dot-files`       (Default: false) Exclude files from renaming.

    - `--ignore-dot-dirs`        (Default: false) Exclude directories that starts with '.' from renaming.

    - `--ignore-files`           (Default: false) Exclude files that starts with '.' from renaming.

    - `--help`                   Display this help screen.

- **examples:**

    - Base usage:

    `renamer pattern -p "path/to/your/files" --pattern "$ command" "# (regex)" "## capture group index (default: 1)" "% normal text"`

    - Command: same as the above commands but don't use (`-p`, `--path`) flags.

    - Available commands:
        - `alphabetical`, `reverse`, `replace`, `upper`, `lower`, `title`. But `random` and `numerical` have more features
        - `random`:
            - `c`, `--consistent`: for a consistent random name for all items.
            - `l`, `length`: for random name length (min: 12, max: 32)
        - `numerical`:
            - `--range`: A range to repeat the numbers within.
            - `--every`: Increace The number every nth iteration.

    - EX1: 
        - New names: "SE01-EP01", "SE01-EP02", ..., "SE01-EP013", "SE02-EP01", ...:
        - The command:

        `renamer patern -p "." --pattern "% SE" "$ numerical -s 1 -z 2 --every 13" "% -EP" "$ numerical -s 1 -z 2 --range 1 13"`

    - EX2:
        - Old names: "albumname_artistname_01_songname.mp3", "albumname_artistname_02_songname.mp3", "albumname_artistname_03_songname.mp3", ....
        - New names: "artistname - songname (01).mp3", "artistname - songname (02).mp3", "artistname - songname (03).mp3", ...
        - The command:

        `renamer patern -p "path/to/your/files" -n "new/path/for/your/files" --pattern "# (_(\w+)_\d)" "## 2" "%  - " "# (\d_(\w+))" "## 2" "% (" "# (\d\d)" "% )"`
    
    - EX3:
        - New names: "a-fc09c97dd1d54f", "b-55e6112456a34e", "c-a1744f997d9c4b", ...
        - The Command:

        `renamer patern -p "path/to/your/files" --pattern "$ alphabetical -s a" "% -" "$ random -l 14"`

    - EX4:
        - New names: "a-55e6112456a34e", "b-55e6112456a34e", "c-55e6112456a34e", ...
        - The Command:

        `renamer patern -p "path/to/your/files" --pattern "$ alphabetical -s a" "% -" "$ random -l 14 -c"`

    - EX5:
        - Old names: "songname.mp3", "songname.mp3", ...
        - New names: "albumname - songname - 01.mp3", "albumname - songname - 02.mp3", "albumname - songname - 03.mp3", ...
        - The Command:

        `renamer patern -p "path/to/your/files" --pattern "% albumname - " "# (.+)" "%  - " "$ numerical -s 1 -z 2"`


## TODO:
- [ ] Code refactoring and performance improvements
- [ ] Recursive renaming
- [ ] Preview before renaming
- [ ] Rename by file type and classify files by file type.
- [ ] Image tags for renaming
- [ ] Audio tags for renaming
- [ ] Video tags for renaming
- [ ] Cross-Platform GUI
