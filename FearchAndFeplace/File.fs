namespace FearchAndFeplace

open System.IO
open System.Text
open UtfUnknown

[<RequireQualifiedAccess>]
module File =
    let writeAllText (path: string) (encoding: Encoding) (contents: string) =
        File.WriteAllText(path, contents, encoding)

    let readAllBytes = File.ReadAllBytes

    let readAllText (fileInfo: FileInfo) =
        let bytes = readAllBytes fileInfo.FullName

        if bytes.Length = 0 then
            let defaultEncoding: Encoding =
                UTF8Encoding(encoderShouldEmitUTF8Identifier = true, throwOnInvalidBytes = true)

            Ok(fileInfo.Name, defaultEncoding, "")
        else
            let detectionResult =
                CharsetDetector
                    .DetectFromBytes(bytes)
                    .Detected
                |> Option.ofObj

            match detectionResult with
            | None -> Error $"Could not detect encoding of file {fileInfo.FullName}"
            | Some detectionResult ->
                let encoding = detectionResult.Encoding
                let memoryStream = new MemoryStream(bytes)
                use stream = new StreamReader(memoryStream, encoding)
                let contents = stream.ReadToEnd()
                Ok(fileInfo.Name, encoding, contents)
