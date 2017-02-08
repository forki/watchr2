module Ansi

open Expecto
open Helper
open Ansi

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

    testCase "unicode characters" <| fun () ->
      convertHtml "a🍻bc"
      |> isEqualTo "a🍻bc"

    testCase "tabs" <| fun () ->
      convertHtml "some\tstring"
      |> isEqualTo "some  string"
  ]

[<Tests>]
let colors =
  let esc = "\u001b"

  testList "colors" [
    testCase "red" <| fun () ->
      convertHtml (esc + "[31mred")
      |> isEqualTo """<span style="color: red">red</span>"""
  ]
