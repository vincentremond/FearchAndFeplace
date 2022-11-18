open FsToolkit.ErrorHandling

module Program =

    open FearchAndFeplace

    [<EntryPoint>]
    let main argv =
        result {
            let args =
                CommandLine.parse<Arguments> argv
                |> Result.defaultWith (fun errs -> failwithf $"Error: %A{errs}")

            let! fixedContent = Fixer.fix args
            ResultWriter.writeContent fixedContent
        }
        |> function
            | Ok _ -> 0
            | Error err ->
                printfn $"Error: %A{err}"
                -1
