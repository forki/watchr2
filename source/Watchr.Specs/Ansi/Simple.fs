module Ansi

open Expecto

let convertHtml (x:string) =
  x.Replace("\t", "    ")

let isEqualTo left right =
  Expect.equal left right "egal"

[<Tests>]
let tests =
  testList "simple values" [
    testCase "empty string" <| fun () ->
      convertHtml ""
      |> isEqualTo ""

    testCase "single string" <| fun () ->
      convertHtml "string"
      |> isEqualTo "string"

    testCase "string with whitespace" <| fun () ->
      convertHtml "some string"
      |> isEqualTo "some string"

    testCase "tabs" <| fun () ->
      convertHtml "some\tstring"
      |> isEqualTo "some    string"
  ]
