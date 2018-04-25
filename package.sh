#!/bin/bash

#unity-editor-beta -executeMethod Editor.BuildScript.BuildAll
#unity-editor-beta -quit -batchmode -buildLinux64Player ./DungeonDrifter_Linux/DungeonDrifter
#unity-editor-beta -quit -batchmode -buildWindows64Player ./DungeonDrifter_Windows/DungeonDrifter

zip -r Build/DungeonDrifter_Windows.zip DungeonDrifter_Windows
zip -r Build/DungeonDrifter_Linux.zip DungeonDrifter_Linux
zip -r Build/DungeonDrifter_Web.zip DungeonDrifter_Web
