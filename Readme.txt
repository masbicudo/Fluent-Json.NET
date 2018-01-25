Visual Studio does not support openning all
projects at once. You must unload all of them
and reaload only the ones you want to work with.

A problem happens if you load the project
targeting both Lib_v10 and Lib_v9 and try to
build them. Visual Studio gets confused and
load incorrect versions of the Newtonsoft.Json
library from the NuGet. This is related with
this issue: https://github.com/NuGet/Home/issues/4463

Another problem happens if you load more than one
test project from the `Tests.Targeted` folder.
Load only one test project at a time.
Loading all of them will confuse Visual Studio
testing tool. The cause is the same as above,
the issue with multiple projects in one single folder.

To avoid errors when building for multiple versions
of the library, delete the \obj folder between builds.
