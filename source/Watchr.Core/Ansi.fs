module Ansi

let convertHtml input =
  let tryFind (header:string) (text:string) =
    if text.StartsWith header then
      Some <| text.Substring(header.Length)
    else
      None

  let tryFindRegex pattern (text:string) =
    let matches = System.Text.RegularExpressions.Regex.Matches(text, pattern)
    if matches.Count > 0 then
      let matched = matches.[0].Value
      let unmatched = text.Substring(matches.[0].Value.Length)
      Some <| (matched, unmatched)
    else
      None

  let (|Tab|_|) = tryFind "\t"
  let (|Escape|_|) = tryFind "\u001b["
  let (|Text|_|) = tryFindRegex "^[\S ]+"

  // type GraphicsSetting =
  //   | Reset
  //   | Foreground of color : int
  //   | Background of color : int
  // let (|Graphics|_|) =
  //   let r = tryFindRegex "^(\d+(;\d+)*)m"
  //   match r with
  //   | Some matched, rest ->
  //     matched.Split(';')
  //   | _ -> _

  // let (|AnsiColor|_|) (input) =
  //   printfn "col %s" input
  //   match input with
  //   | StartsWith "\u001b[" input ->
  //     printfn "rest %s" input
  //     match input with
  //     | Match "^\d+?m" code-> Some code
  //     | _ -> failwithf "Unknown ANSI escape"
  //   | _ -> printfn "none";None

  let convertEscape acc input =
    printfn "convertEscape %A %A" acc input
    match input with
    // | Graphics(code, tail) ->
    //   printfn "code: %A" code
    //   tail
    | _ -> input

  let rec convert acc input =
    printfn "convert %A %A" acc input
    match input with
    | Escape tail ->
        convertEscape acc tail |> convert acc
    | Tab tail ->
        // TODO # of spaces depends on cursor position
        let acc = acc + "  "
        convert acc tail
    | Text(text, tail) ->
        let acc = acc + text
        convert acc tail
    | _ -> acc

  convert "" input
