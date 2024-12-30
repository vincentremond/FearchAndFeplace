namespace FearchAndFeplace

open System.IO
open System.Text
open UtfUnknown

type FileContents = {
    Name: string
    Contents: Contents
}

and Contents =
    | Empty
    | Text of string * Encoding
    | Binary of byte[]

and DirectoryContents = {
    Name: string
    Items: FileEntryItem list
}

and FileEntryItem =
    | File of FileContents
    | Directory of DirectoryContents

[<RequireQualifiedAccess>]
module File =
    let writeAllText (path: string) (encoding: Encoding) (contents: string) =
        File.WriteAllText(path, contents, encoding)

    let writeAllBytes (path: string) (contents: byte[]) = File.WriteAllBytes(path, contents)

    let readAllBytes = File.ReadAllBytes

    let readContents (fileInfo: FileInfo) =
        let bytes = readAllBytes fileInfo.FullName

        if bytes.Length = 0 then
            {
                Name = fileInfo.Name
                Contents = Empty
            }
        else
            let detectionResult =
                CharsetDetector.DetectFromBytes(bytes).Detected |> Option.ofObj

            match detectionResult with
            | None -> {
                Name = fileInfo.Name
                Contents = Binary bytes
              }
            | Some detectionResult ->
                let encoding =
                    detectionResult.Encoding |> Option.ofObj |> Option.defaultValue Encoding.ASCII

                let memoryStream = new MemoryStream(bytes)
                use stream = new StreamReader(memoryStream, encoding)
                let contents = stream.ReadToEnd()

                {
                    Name = fileInfo.Name
                    Contents = Text(contents, encoding)
                }
