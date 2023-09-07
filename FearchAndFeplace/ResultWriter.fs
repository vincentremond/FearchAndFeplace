namespace FearchAndFeplace

open FearchAndFeplace

[<RequireQualifiedAccess>]
module ResultWriter =

    let rec writeContent content =
        for item in content do
            match item with
            | FileEntryItem.File file ->
                let filePath = file.Name
                printfn $"Writing file %s{filePath}"
                File.writeAllText filePath file.Encoding file.Contents
            | FileEntryItem.Directory directory ->
                Directory.ensureExists directory.Name
                writeContent directory.Items
