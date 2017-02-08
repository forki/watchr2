module Watchr.Specs

open Expecto

[<EntryPoint>]
let main args =
  let config = { defaultConfig with parallelWorkers = 1 }
  runTestsInAssembly config args
