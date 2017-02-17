#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing.Expecto

let buildDir  = "./build/"
let deployDir = "./deploy/"

let appReferences  =
  !! "/**/*.csproj"
  ++ "/**/*.fsproj"

let version = "0.1"  // or retrieve from CI server

Target "Clean" (fun _ ->
  CleanDirs [buildDir; deployDir]
)

let build() =
  MSBuildDebug buildDir "Build" appReferences
  |> Log "AppBuild-Output: "

Target "Build" (fun _ -> build())

let runTests() =

  !! (buildDir + "*.Specs.exe")
  |> Expecto (fun p -> { p with FailOnFocusedTests = false })

Target "Test" (fun _ -> runTests())

type Notification =
  | Info
  | Success
  | Error

Target "Watch" (fun _ ->
  let notify title message notification =
    let message = match notification with
                  | Info -> (sprintf "ℹ️ %s" message, None)
                  | Success -> (sprintf "✅ %s" message, None)
                  | Error -> (sprintf "❌ %s" message, Some "Funk")

    match (tryFindFileOnPath "osascript") with
    | Some exe ->
      let subtitle, sound = match message with
                            | (text, Some sound) ->
                                (sprintf """subtitle "%s" """ text,
                                 sprintf """sound name "%s" """ sound)
                            | (text, None) -> (sprintf """subtitle "%s" """ text, "")

      let args = sprintf """-e 'display notification "" with title "%s" %s %s'""" title subtitle sound
      let result = Shell.Exec(exe, args)

      // If exe does not exist, Shell.Exec does not seem to error, perhaps because of WatchChanged.
      if result <> 0 then failwithf "%s returned with a non-zero exit code" exe
    | _ -> ()

  use spec =
    !! "source/**/*.fs"
    |> WatchChanges (fun changes ->
      notify "FAKE" "Build starting" Notification.Info
      build()

      try
        runTests()
        notify "Expecto" "Successful" Notification.Success
      with
      | err -> notify "Expecto" "Failed" Notification.Error
    )

  notify "FAKE" "Started file system watcher" Notification.Info
  System.Console.ReadLine() |> ignore
  spec.Dispose()
)

"Clean"
  ==> "Build"
  ==> "Test"

RunTargetOrDefault "Test"
