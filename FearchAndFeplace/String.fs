namespace FearchAndFeplace

open System

[<RequireQualifiedAccess>]
module String =
    let replace (pattern: string) (replacement: string) (input: string) =
        input.Replace(pattern, replacement, StringComparison.InvariantCultureIgnoreCase)
