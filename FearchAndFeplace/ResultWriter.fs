namespace FearchAndFeplace

open System.Text
open FearchAndFeplace

[<RequireQualifiedAccess>]
module ResultWriter =

    let rec writeContent content =
        for item in content do
            match item with
            | FileEntryItem.File file ->
                let filePath = file.Name
                printfn $"Writing file %s{filePath}"

                match file.Contents with
                | Text(text, encoding) -> File.writeAllText filePath encoding text
                | Binary binary -> File.writeAllBytes filePath binary
                | Empty -> File.writeAllText filePath Encoding.ASCII ""
            | FileEntryItem.Directory directory ->
                Directory.ensureExists directory.Name
                writeContent directory.Items
