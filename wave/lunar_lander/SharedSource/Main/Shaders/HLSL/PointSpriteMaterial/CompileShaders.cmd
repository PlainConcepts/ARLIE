@echo off
rem Copyright (c) WaveEngine. All rights reserved.

setlocal
set error=0

set fxcpath="..\..\..\..\..\Libraries\fxc.exe"
set outFolder=..\..\..\..\..\Content\Assets\Shaders\PointSprite\
set shaderName=PointSpriteMaterial
goto compile

:compile

call :CompileShader vs vsPointSprite
call :CompileShader vs vsPointSprite NOISE
call :CompileShader vs vsPointSprite APPEARING
call :CompileShader vs vsPointSprite APPEARING NOISE

call :CompileShader vs vsPointSprite COLORTEXTURE
call :CompileShader vs vsPointSprite COLORTEXTURE NOISE
call :CompileShader vs vsPointSprite COLORTEXTURE APPEARING
call :CompileShader vs vsPointSprite COLORTEXTURE APPEARING NOISE

call :CompileShader vs vsPointSprite COLORTEXTURE RANGE
call :CompileShader vs vsPointSprite COLORTEXTURE NOISE RANGE
call :CompileShader vs vsPointSprite COLORTEXTURE APPEARING RANGE
call :CompileShader vs vsPointSprite COLORTEXTURE APPEARING NOISE RANGE

call :CompileShader vs vsPointSprite VELOCITY
call :CompileShader vs vsPointSprite VELOCITY NOISE
call :CompileShader vs vsPointSprite VELOCITY APPEARING
call :CompileShader vs vsPointSprite VELOCITY APPEARING NOISE

call :CompileShader vs vsPointSprite BIAS COLORTEXTURE
call :CompileShader vs vsPointSprite BIAS COLORTEXTURE NOISE
call :CompileShader vs vsPointSprite BIAS COLORTEXTURE APPEARING
call :CompileShader vs vsPointSprite BIAS COLORTEXTURE APPEARING NOISE

call :CompileShader vs vsPointSprite VELOCITY BIAS COLORTEXTURE RANGE
call :CompileShader vs vsPointSprite VELOCITY BIAS COLORTEXTURE NOISE RANGE
call :CompileShader vs vsPointSprite VELOCITY BIAS COLORTEXTURE APPEARING RANGE
call :CompileShader vs vsPointSprite VELOCITY BIAS COLORTEXTURE APPEARING NOISE RANGE

call :CompileShader ps psPointSprite

echo.

if %error% == 0 (
    echo Shaders compiled ok
) else (
    echo There were shader compilation errors!
)

endlocal
exit /b

:CompileShader
if [%3]==[] goto three
if [%4]==[] goto four
if [%5]==[] goto five
if [%6]==[] goto six
if [%7]==[] goto seven
if [%8]==[] goto eight
if [%9]==[] goto nine

set fxc=%fxcpath% /nologo %shaderName%.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /D %6 /D %7 /D %9 /E %2 /Fo %outFolder%%1%shaderName%_%3_%4_%5_%6_%7_%8_%9.fxo
goto end

:nine
set fxc=%fxcpath% /nologo %shaderName%.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /D %6 /D %7 /D %8 /E %2 /Fo %outFolder%%1%shaderName%_%3_%4_%5_%6_%7_%8.fxo
goto end

: eight
set fxc=%fxcpath% /nologo %shaderName%.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /D %6 /D %7 /E %2 /Fo %outFolder%%1%shaderName%_%3_%4_%5_%6_%7.fxo
goto end

:seven
set fxc=%fxcpath% /nologo %shaderName%.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /D %6 /E %2 /Fo %outFolder%%1%shaderName%_%3_%4_%5_%6.fxo
goto end

:six
set fxc=%fxcpath% /nologo %shaderName%.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /D %5 /E %2 /Fo %outFolder%%1%shaderName%_%3_%4_%5.fxo
goto end

:five
set fxc=%fxcpath% /nologo %shaderName%.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /D %4 /E %2 /Fo %outFolder%%1%shaderName%_%3_%4.fxo
goto end

:four
set fxc=%fxcpath% /nologo %shaderName%.fx /T %1_4_0 /I ..\Helpers.fxh /D %3 /E %2 /Fo %outFolder%%1%shaderName%_%3.fxo
goto end

:three
set fxc=%fxcpath% /nologo %shaderName%.fx /T %1_4_0 /I ..\Helpers.fxh /E %2 /Fo %outFolder%%1%shaderName%.fxo

:end
echo.
echo %fxc%
%fxc% || set error=1
exit /b 