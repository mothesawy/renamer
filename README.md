# **Bulk Renamer**

A tool to rename multiple files with different methods, written in C#!

This tools allows you to bulk rename files in a given directory.

You can use different renaming methods.

Run `renamer --help` for more info.

## features:

- This tool can use both relative paths and absolute paths.
- Bulk rename files in place or copy to another path.
- Ordered renaming using mode numbers and assigning the start number
- Random renaming mode using uuidv4
- Start renaming from the last item to the first item (reversed renaming)
- Ability to add leading zeros (padding) to numbers
- Ability to add prefix to filenames
- Ability to add suffix to filenames

#### It's quite stable but needs more testing so use cautiously.

## Installation:
- Use `go install github.com/rvpx367/bulk-renamer`
- Make sure that your `go/bin/` is in your `PATH`

## Usage:

>**bulk-renamer**

- **Usage:** `bulk-renamer [command]`

- **Available Commands:**

    - `completion`
    Generate the autocompletion script for the specified shell

    - `copy-rename`
    This command copies the files from a given path then renames in the new path.

    - `help`
    Help about any command

    - `rename`
    This command renames the files in place in a given path.

- **Flags:**

    - `-h`, `--help` help for bulk-renamer

>**rename**

- **Usage:** `bulk-renamer rename [flags]`

- **Flags:**
    - `-h`, `--help` help for rename

    - `-m`, `--method` The method of renaming: [ord: ordered, rnd: random] (default "ord")

    - `-p`, `--path` The path of the files you want to rename.

    - `--prefix` Prefix before numbers for ordered renaming.

    - `-r`, `--reversed` Start renaming from the last item in the directory. (default: false)

    - `-s`, `--start` Start number for ordered renaming (default: 0, works with method ord only)

    - `--suffix` Suffix after numbers for ordered renaming.

    - `-z`, `--zeros` Number of leading zeros for numbers in ordered renaming. (default: 0, works with method ord only)

>**copy-rename**
- **Usage:** `bulk-renamer rename [flags]`

- **Flags:**
    - `-h`, `--help` help for rename

    - `-m`, `--method` The method of renaming: [ord: ordered, rnd: random] (default "ord")

    - `-n`, `--newPath` The new path of the files you want to rename.

    - `-p`, `--path` The path of the files you want to rename.

    - `--prefix` Prefix before numbers for ordered renaming.

    - `-r`, `--reversed` Start renaming from the last item in the directory. (default: false)

    - `-s`, `--start` Start number for ordered renaming (default: 0, works with method ord only)

    - `--suffix` Suffix after numbers for ordered renaming.

    - `-z`, `--zeros` Number of leading zeros for numbers in ordered renaming. (default: 0, works with method ord only)


Use "bulk-renamer [command] --help" for more information about a command.

## TODO:
- [ ] Add alphapetical ordered renaaming (a, b, c, d, ..., aa, ab,..)
- [ ] Add roman numerals renaaming (I, II. III, IV, V, VI, ....)
- [ ] Add rename by file type and classify file by file type.
- [ ] Add recursive renaming feature.