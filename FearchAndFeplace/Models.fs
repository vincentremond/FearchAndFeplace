namespace FearchAndFeplace

open System
open System.Text

type FolderPath = String

type Arguments = {
    [<CommandLine.Option('s', "sourceFolder", Required = true)>]
    SourceFolder: FolderPath
    [<CommandLine.Option('t', "targetFolder", Required = true)>]
    TargetFolder: FolderPath
    [<CommandLine.Option('p', "pattern", Required = true)>]
    Pattern: String
    [<CommandLine.Option('r', "replace", Required = true)>]
    Replacement: String
}

type FileContents = {
    Name: string
    Encoding: Encoding
    Contents: string
}

and DirectoryContents = {
    Name: string
    Items: FileEntryItem list
}

and FileEntryItem =
    | File of FileContents
    | Directory of DirectoryContents
