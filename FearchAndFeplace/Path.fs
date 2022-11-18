namespace FearchAndFeplace

module Path =
    let (</>) (a: string) (b: string): string = System.IO.Path.Combine(a, b)
