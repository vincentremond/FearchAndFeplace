namespace FearchAndFeplace

[<RequireQualifiedAccess>]
module Directory =
    let ensureExists path =
        if not (System.IO.Directory.Exists path) then
            System.IO.Directory.CreateDirectory path |> ignore
