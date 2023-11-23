open System.IO
open System.Threading.Tasks
open FearchAndFeplace
open FsToolkit.ErrorHandling
open Fargo

let commandLineParser =

    fargo {
        let! sourceDirectory =
            (opt "source-directory" null "dir" "The directory to search for files to fix")
            |> reqOpt
            |> parse FolderPath.init

        and! targetDirectory =
            opt "target-directory" null "dir" "The directory to write the fixed files to"
            |> reqOpt
            |> parse FolderPath.init

        and! searchPattern =
            opt "search-pattern" null "pattern" "The search pattern to use when searching for files to fix"
            |> reqOpt
            |> parse SearchPattern.init

        and! replacement =
            opt "replacement" null "replacement" "The replacement to use when fixing files"
            |> reqOpt
            |> parse Replacement.init

        return {
            SourceDirectory = sourceDirectory
            TargetDirectory = targetDirectory
            SearchPattern = searchPattern
            Replacement = replacement
        }
    }

[<EntryPoint>]
let main commandLineArguments =
    run
        "FearchAndFeplace"
        commandLineParser
        commandLineArguments
        (fun _cancellationToken parsedArguments ->
            let fixedContent = Fixer.fix parsedArguments

            Task.FromResult
            <| match fixedContent with
               | Ok fixedContent ->
                   ResultWriter.writeContent fixedContent
                   0
               | Error error ->
                   printfn $"Error: %s{error}"
                   -1
        )
