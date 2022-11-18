namespace FearchAndFeplace

open CommandLine

[<RequireQualifiedAccess>]
module CommandLine =
    let parse<'t> argv =
        match Parser.Default.ParseArguments<'t>(argv) with
        | :? Parsed<'t> as parsed -> Result.Ok parsed.Value
        | :? NotParsed<'t> as notParsed -> Result.Error notParsed.Errors
        | _ -> failwith "Unknown ParseResult"
