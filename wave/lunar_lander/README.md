# Wave environments compilation

## Windows

### Windows Prerequisites

[Microsoft Visual C++ 2010 Redistributable Package x86](https://www.microsoft.com/en-US/download/details.aspx?id=5555)

### Windows Wave package

From visual studio, build in release, copy the contents of `[REPO]\wave\lunar_lander\Launchers\Windows\bin\AnyCPU\Release` to `[REPO]\arlie\arlie\envs\lunar_lander\LunarLander`

## Linux

### Linux Prerequisites

Install monodevelop SDL2 and SDL2_mixer

At least in ArchLinux is necessary to fix Roselyn path:
`sudo ln -s /usr/lib/mono/msbuild/Current/bin/Roslyn /usr/lib/mono/msbuild/15.0/bin/Roslyn`

### Linux Wave package

Use msbuild to build the solution in release:

```bash
nuget restore RLEnvs_Linux.sln
msbuild RLEnvs_Linux.sln /t:Build /p:Configuration=Release
```

Then copy the contents of `[REPO]/wave/lunar_lander/Launchers/Linux/bin/x86/Release` to `[REPO]/arlie/arlie/envs/lunar_lander/LunarLander`

```bash
cd ../..
mkdir -p ./arlie/arlie/envs/lunar_lander/LunarLander
rm -r ./arlie/arlie/envs/lunar_lander/LunarLander/*
cp -r ./wave/lunar_lander/Launchers/Linux/bin/x86/Release/* ./arlie/arlie/envs/lunar_lander/LunarLander
```
