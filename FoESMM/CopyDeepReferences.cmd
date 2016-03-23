@echo off

SET output=%1
SET root=%2
SET configuration=%3

SET modules=libespmsharp SharpCompress
(
  for %%l in (%modules%) do (
    echo Copying %%l
    copy %root%FoESMM.Common\bin\%configuration%\%%l.dll %output%
    if exist %root%FoESMM.Common\bin\%configuration%\%%l.pdb (
      echo Copying %%l debug symbols
      copy %root%FoESMM.Common\bin\%configuration%\%%l.pdb %output%
    )
  )
)
