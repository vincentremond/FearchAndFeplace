namespace FearchAndFeplace

open System.IO
open System.Xml.Linq
open FsToolkit.ErrorHandling
open FearchAndFeplace
open FearchAndFeplace.Path

[<RequireQualifiedAccess>]
module Fixer =

    let rec private scanFolder (folder: DirectoryInfo) =
        result {
            let! files =
                folder.GetFiles()
                |> List.ofArray
                |> List.map File.readAllText
                |> List.sequenceResultA

            let files =
                files
                |> List.map (fun (name, encoding, content) ->
                    FileEntryItem.File {
                        Name = name
                        Encoding = encoding
                        Contents = content
                    }
                )

            let! directories =
                folder.GetDirectories()
                |> List.ofArray
                |> List.filter (fun d ->
                    d.Name <> "obj"
                    && d.Name <> "bin"
                )
                |> List.map scanFolder
                |> List.sequenceResultM

            let directories =
                directories
                |> List.map Directory

            return {
                Name = folder.Name
                Items = (directories @ files)
            }
        }

    let rec doFixes targetDirectory fix entries =
        entries
        |> List.map (
            function
            | FileEntryItem.File file ->
                FileEntryItem.File {
                    file with
                        Contents = fix file.Contents
                        Name =
                            (targetDirectory
                             </> (fix file.Name))
                }
            | FileEntryItem.Directory directory ->
                let dir =
                    targetDirectory
                    </> (directory.Name |> fix)

                FileEntryItem.Directory {
                    directory with
                        Name = dir
                        Items = (doFixes dir fix (directory.Items))
                }
        )

    let fix args =
        result {
            let fix = String.replace args.Pattern args.Replacement

            let! scanResult =
                args.SourceFolder
                |> DirectoryInfo
                |> scanFolder

            let result = [ FileEntryItem.Directory scanResult ]
            return doFixes args.TargetFolder fix result
        }
