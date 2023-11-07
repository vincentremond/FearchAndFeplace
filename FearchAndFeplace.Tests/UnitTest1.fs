module FearchAndFeplace.Tests

open FearchAndFeplace
open NUnit.Framework

[<Test>]
let Test1 () =
    let args: Arguments = {
        SourceFolder = @"D:\TMP\2022.11.09-AdventOfCode2021v2\AdventOfCode2021v2\AdventOfCode2021v2.{{{Project}}}"
        TargetFolder = @"D:\TMP\2022.11.09-AdventOfCode2021v2\AdventOfCode2021v2\"
        Pattern = "{{{Project}}}"
        Replacement = "Day01"
    }

    let fileEntryItemsResult =
        match Fixer.fix args with
        | Ok fileEntryItems -> fileEntryItems
        | Error error -> failwith $"%A{error}"

    Assert.Pass()
