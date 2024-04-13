"Tools\quickbms.exe" "Tools\wavescan.bms" "Game Files" "Tools\Decoding"
FOR %%b IN ("Game Files\*.BNK") DO ("Tools\bnkextr.exe" "%%b" & MOVE *.wav "Tools\Decoding")
FOR %%c IN (Tools\Decoding\*.WAV) DO ("Tools\ww2ogg.exe" "%%c" --pcb Tools\packed_codebooks_aoTuV_603.bin & DEL "%%c")
FOR %%d IN (Tools\Decoding\*.OGG) DO ("Tools\revorb.exe" "%%d" & MOVE "%%d" "MP3")
FOR %%e IN (MP3\*.OGG) DO ("Tools\ffmpeg.exe" -i "%%e" -acodec libmp3lame -q:a 0 -y "MP3\%%~ne.mp3" & DEL "%%e")

@echo off

echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo    ((  '####@@!!$$    ))
echo        `#####@@!$$`  ))
echo     ((  '####@!!$:
echo    ((  ,####@!!$:   ))
echo        .###@!!$:
echo        `##@@!$:
echo         `#@!!$
echo   !@#    `#@!$:       @#$
echo    #$     `#@!$:       !@!
echo             '@!$:
echo         '`\   '!$: /`'
echo            '\  '!: /'
echo              '\ : /'
echo   -.'-/\\\-.'//.-'/:'\.'-.JrS'.'-=_\\
echo ' -.'-.\\'-.'//.-'.`-.'_\\-.'.-\'.-//
echo      Watch out, it's the tornado!

echo.
echo -------------------------------------------------------------

echo Unpack finished! Files should be in the 'MP3' folder

echo -------------------------------------------------------------
echo.