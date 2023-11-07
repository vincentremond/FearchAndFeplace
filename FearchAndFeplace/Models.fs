namespace FearchAndFeplace

open System

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
