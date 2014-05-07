#Lunt

Lunt is an asset pipeline and build system for games.

Lunt is an incremental, cross platform build system that allows you to compile asset data in managed code (i.e. C# or C++/CLI). The API for building components resemble the one that can be found in the [XNA Content Pipeline](http://msdn.microsoft.com/en-us/library/ff827626.aspx) or the [MonoGame Content Processing framework](https://github.com/mono/MonoGame/wiki/MonoGame-Content-Processing), but unlike these two, the output file is not tied to a specific format, and the hosting model is interchangeable.

Some reasons to use Lunt:

* Build engine not tied to a specific hosting provider or output format.
* Supports incremental builds.
* Easy to debug the build pipeline by creating a debug project and referencing the Lunt build engine assembly.
* The ease of C++/CLI to call into native libraries, makes it really simple to use existing functionality such as [Assimp](http://assimp.sourceforge.net/) or [FreeType](http://www.freetype.org/) as part of the pipeline.

##NuGet

Both Lunt and Lake (Lunt Make) are available as NuGet packages.

```
PM> Install-Package Lunt
```

```
PM> Install-Package Lunt.Make
```

###Binaries

You can download the latest binaries [here](https://github.com/Lunt/Lunt/releases) which contains the following:  

* Lake.exe
* Lunt.dll
* Lunt.xml

##CI Build Status

####TeamCity (.NET)

[![TeamCity CI Build Status](http://builds.nullreferenceexception.se/app/rest/builds/buildType:id:Lunt_Lunt_Continuous/statusIcon)](http://builds.nullreferenceexception.se/viewType.html?buildTypeId=Lunt_Lunt_Continuous&guest=1)

####TeamCity (Mono)

[![TeamCity CI Build Status](http://builds.nullreferenceexception.se/app/rest/builds/buildType:id:Lunt_Lunt_Continuous_Mono/statusIcon)](http://builds.nullreferenceexception.se/viewType.html?buildTypeId=Lunt_Lunt_Continuous_Mono&guest=1)

##Need more information?

For more information and examples of how to use Lunt, see the [Wiki](https://github.com/Lunt/Lunt/wiki).
