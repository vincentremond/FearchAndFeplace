namespace FearchAndFeplace

open System.IO
open FsToolkit.ErrorHandling
open FearchAndFeplace
open FearchAndFeplace.Path

[<RequireQualifiedAccess>]
module Fixer =

    let rec private scanFolder (folder: DirectoryInfo) =
        result {
            let files =
                folder.GetFiles()
                |> List.ofArray
                |> List.map (File.readContents >> FileEntryItem.File)

            let! directories =
                folder.GetDirectories()
                |> List.ofArray
                |> List.filter (fun d -> d.Name <> "obj" && d.Name <> "bin")
                |> List.map scanFolder
                |> List.sequenceResultM

            let directories = directories |> List.map Directory

            return {
                Name = folder.Name
                Items = (directories @ files)
            }
        }

    let rec doFixes targetDirectory fix entries =
        let fixContents c =
            match c with
            | Empty -> Empty
            | Text(text, encoding) -> Text((fix text), encoding)
            | Binary bytes -> Binary bytes

        entries
        |> List.map (
            function
            | FileEntryItem.File file ->
                FileEntryItem.File {
                    file with
                        Contents = fixContents file.Contents
                        Name = (targetDirectory </> (fix file.Name))
                }
            | FileEntryItem.Directory directory ->
                let dir = targetDirectory </> (directory.Name |> fix)

                FileEntryItem.Directory {
                    directory with
                        Name = dir
                        Items = (doFixes dir fix directory.Items)
                }
        )

    let fix args =
        result {
            let fix = String.replace args.SearchPattern.Value args.Replacement.Value

            let! scanResult = args.SourceDirectory.Value |> DirectoryInfo |> scanFolder

            let result = [ FileEntryItem.Directory scanResult ]
            return doFixes args.TargetDirectory.Value fix result
        }
