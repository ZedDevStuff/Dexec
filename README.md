# Dexec

Shebang for windows. That's it.

## Why?

I was bored, and as a programmer there isn't any better reason to do stuff.

## Installation and Usage

Make sure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed along with Visual Studio 2022 with [NativeAOT prerequisites](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=windows%2Cnet8#tabpanel_1_windows).

1- Clone the repository or download the source code and navigate to the project directory.

```
git clone https://github.com/ZedDevStuff/Dexec
cd Dexec
```

2- Compile the project using the .NET CLI:

```
dotnet publish -c Release
```

3- Move the compiled executable at `<project>/bin/Release/net9.0/win-<arch>/publish` to your desired location.

4- Execute the program with administrative privileges (right-click and select "Run as administrator") to ensure it is added to the context menu. (I'm actually not sure this works correctly on Windows 11 since I'm using the old context menu on my machine)

Done. Now just add a shebang to a script file (if the specific language is sensitive to the syntax, put it in a comment) and right-click it to see the "Dexec" option.
