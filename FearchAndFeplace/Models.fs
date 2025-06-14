namespace FearchAndFeplace

open System.IO

type FolderPath =
    | FolderPath of string

    static member init(path: string) =
        match System.IO.Directory.Exists path with
        | true -> path |> FolderPath |> Ok
        | false -> Error $"The directory '%s{path}' does not exist"

    member this.Value =
        match this with
        | FolderPath path -> path

type NonEmptyString =
    | NonEmptyString of string

    static member init(value: string) =
        match System.String.IsNullOrWhiteSpace value with
        | true -> Error "The value cannot be empty"
        | false -> Ok(NonEmptyString value)

type SearchPattern =
    | SearchPattern of NonEmptyString

    static member init = NonEmptyString.init >> Result.map SearchPattern

    member this.Value =
        match this with
        | SearchPattern(NonEmptyString value) -> value

type Replacement =
    | Replacement of NonEmptyString

    static member init = NonEmptyString.init >> Result.map Replacement

    member this.Value =
        match this with
        | Replacement(NonEmptyString value) -> value

type CommandLineArguments = {
    SourceDirectory: FolderPath
    TargetDirectory: FolderPath
    SearchPattern: SearchPattern
    Replacement: Replacement
}
