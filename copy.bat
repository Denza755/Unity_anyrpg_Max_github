@echo off
REM ============================================================
REM  Копирование проекта Unity для анализа
REM  Исключаются: текстуры, модели, аудио, видео, библиотеки, кэш
REM  Источник: C:\L2_Unity\Gawric_3\Gawric_3\l2-unity
REM  Назначение: C:\L2_Unity_Github
REM ============================================================

rem  set "SOURCE=C:\L2_Unity\Gawric_3\Gawric_3\l2-unity"
set "SOURCE=J:\Unity_anyrpg_Max\Anyrpg"
set "DEST=J:\Unity_anyrpg_Max_github "

REM Проверка существования исходной папки
if not exist "%SOURCE%" (
    echo ОШИБКА: Исходная папка не найдена: %SOURCE%
    pause
    exit /b 1
)

REM Создание папки назначения, если её нет
if not exist "%DEST%" mkdir "%DEST%"

echo Копирование проекта из "%SOURCE%" в "%DEST%"...
echo Исключаются текстуры, модели, аудио, видео, библиотеки и кэш.
echo.

REM Robocopy:
REM /E       - копировать все подпапки, включая пустые
REM /XD      - исключить указанные папки
REM /XF      - исключить файлы по маскам
REM /NFL /NDL - не выводить списки файлов и папок (уменьшает шум)
REM /NP       - не показывать процент выполнения

robocopy "%SOURCE%" "%DEST%" /E /NFL /NDL /NP ^
 /XD "Library" "Temp" "Obj" "Build" "Logs" "UserSettings" ".vs" ".git" ".idea" ".vscode" ".vsconfig" ^
 /XF *.png *.jpg *.jpeg *.tga *.psd *.bmp *.gif *.tif *.tiff *.iff *.pict *.svg *.webp *.raw ^
     *.fbx *.obj *.max *.ma *.mb *.dae *.3ds *.dxf *.dwg *.stl *.ply *.gltf *.glb *.abc *.usd *.usda *.usdc *.usdz *.blend* ^
     *.dds *.exr *.hdr *.cubemap *.rendertexture ^
     *.mp3 *.wav *.ogg *.aiff *.aif *.mod *.it *.xm *.s3m *.mp4 *.mov *.avi *.wmv *.webm ^
     *.dll *.so *.dylib *.exe *.zip *.7z *.rar *.unitypackage *.apk *.aab *.ipa *.app *.bundle *.assets *.resS *.resource *.bytes ^
     *.ttf *.otf *.shadervariants *.compute *.raytrace

echo.
echo Копирование завершено.
echo Папка назначения: %DEST%
pause