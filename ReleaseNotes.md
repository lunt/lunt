### New in 0.0.1 (Released 2014/05/06)
* First release of Lunt.

### New in 0.0.2 (Released 2014/05/07)
* Added build engine bootstrapper.
* Restructuring of solution.

### New in 0.0.3 (Released 2014/05/08)
* Removed Lunt prefix from class names.

### New in 0.0.4 (Released 2014/05/11)
* Kernel is now resolved as a singleton from container.
* Moved the Lake console build log to Lunt.
* Build log now preserves the provided composite format.
* Asset build result messages are now stored as composites.

## New in 0.0.5
* Renamed the build engine to make place for the convention based engine.
* Nancyfied the bootstrapper to make it more versatile.
* Added convention based build engine that uses the bootstrapper.
* Assembly type scanner now skips dynamic assemblies.
* Added colors to build log.
* Lake exit codes now give more information about executed build.