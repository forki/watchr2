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

Target "Build" (fun _ ->
  MSBuildDebug buildDir "Build" appReferences
  |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
  !! (buildDir + "*.Specs.exe")
  |> Expecto id
  |> ignore
)

Target "Deploy" (fun _ ->
  !! (buildDir + "/**/*.*")
  -- "*.zip"
  |> Zip buildDir (deployDir + "ApplicationName." + version + ".zip")
)

"Clean"
  ==> "Build"
  ==> "Test"
  ==> "Deploy"

RunTargetOrDefault "Test"
