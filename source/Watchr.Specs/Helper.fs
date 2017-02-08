module Helper

open Expecto

let isEqualTo expected actual =
  Expect.equal actual expected ""
