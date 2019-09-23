# NMSCoordinates
This a Coordinate Tool for No Man's Sky!

|               NMSC                        |
|                :---:                      |
|       <img src= "Cover_Photo.png"/>       |

* [More Screenshots - Here](./images)

# NMSCoordinates

NMSCoordinates is an app for No Man’s Sky which allows you to view all your discovered space station and base locations between all your saves.
You can clear portal interference while traveling through a portal, fast travel to known locations, or manually enter Galactic Coordinates and travel to them.
Backup locations to txt file and and access and travel to these saved coordinates.

## Youtube Videos

|Example Videos|  |
|:---:|:---:|
|Portal Interference|Travel Mode|
|<a href="http://www.youtube.com/watch?feature=player_embedded&v=-kipUjXrnOc" target="_blank"><img src="http://img.youtube.com/vi/-kipUjXrnOc/0.jpg" alt="Video" width="520" height="300" border="10" /></a>|<a href="http://www.youtube.com/watch?feature=player_embedded&v=LB7yqgLsW_0" target="_blank"><img src="http://img.youtube.com/vi/LB7yqgLsW_0/0.jpg" alt="Video" width="520" height="300" border="10" /></a>|
|Manual Travel|Fast Travel|
|<a href="http://www.youtube.com/watch?feature=player_embedded&v=ceSFVl9LFtI" target="_blank"><img src="http://img.youtube.com/vi/ceSFVl9LFtI/0.jpg" alt="Video" width="520" height="300" border="10" /></a>|<a href="http://www.youtube.com/watch?feature=player_embedded&v=Vx1xGk2TMIo" target="_blank"><img src="http://img.youtube.com/vi/Vx1xGk2TMIo/0.jpg" alt="Video" width="520" height="300" border="10" /></a>|


## Download - NEW UPDATE!!! 

<!--* Download will be available later today. -->
* Major Optimization Update - NMSC now is 100% faster on MovePlayer and ClearPortal Interference!

<pre>  <a href="https://github.com/Kevin0M16/NMSCoordinates/releases/latest/download/NMSCoordinates-v1.1.3.zip"><img src="https://img.shields.io/badge/dynamic/json.svg?label=download&url=https://api.github.com/repos/Kevin0M16/NMSCoordinates/releases/latest&query=$.assets[0].name&style=for-the-badge" alt="download"/></a></pre>

## Share - Upload locations to Google Drive

* [Google Drive Folder - NMSCoordinates](https://drive.google.com/open?id=0B0Tsv8SX6_GtR2hKNlhVcnBvMmc) Add your locbackup.txt files here to share. Adding your name at the end of the filename is ok.


## Features

* View all space station and base locations discovered across all save slots and see their Glyphs, Galactic Coordinates, Portal Code, and even Voxel X,Y,Z,SSI
* Fast travel to any selected location in the list of discoveries.
* Enable Manual Travel and manually enter Galactic Coordinates and Galaxy that you need to travel to.
* Enable Travel Mode and save a record of all your locations the game deletes when traveling through a terminus or when hitting the limit.
* Portal interference can be cleared with a button, so base building and the galaxy map are allowed after traveling through a portal.
* Save a record of the players current location in a .txt and access it later on the Coordinate Share tab.
* Can Backup all locations to a .txt file. You can access these on the Coordinate Share tab or share with others. You can fast travel to these also.
* NMSCoordinates backs up your entire save folder on startup, the .zip is located in the .\backup folder.
* Manage these NMSC .zip backups in the Save Manager.
* You can view the last 4 screenshots taken in the game within the tool. It displays 1 on front and all 4 can be accessed in Tools --> Screenshot Page.
* NMSCoordinates shows all calculation results in the textbox, if you like HEX and DEC.
* NMSCoordinates Calculator is a useful tool when messing with coordinates. This converts Portal->Galactic, Galactic->Portal, Voxel to Portal/Galactic. Check it out!

## Getting Started

Here is an overview of what you need to get started with NMSCoordinates

### Requirements

The current version of NMSCoordinates requires No Man's Sky Beyond 2.11 or 2.12.

:exclamation: **Always back up all your game data and saves before any mods**

### Installation and setup

	1. (Optional NMSC backups up saves on startup) Backup all you save files at Location: C:\Users\[Name]\AppData\Roaming\HelloGames\NMS\st_xxxxxxxxx
	2. Download the latest release .zip file.
	3. Extract the .zip to your desired location.
	4. Create a shortcut to NMSCoordinate.exe for your desktop.
	5. Run NMSCoordinate.exe. 6. Select Save slot, and have fun!

### Instructions

* Select a Save Slot. This loads all space station and base locations on that slot/save on the Base and Space Station tab.
* Click a location in one of the Listboxes to view the location info. Glyphs and Galactic Coordinates, Portal Code, and Voxel will be displayed.
* Move player to that location by clicking the Move Player Here button, then reload your save in NMS.
* If you traveled through a portal and want to clear the Portal Interference, click the Clear Interference button, then reload your save in NMS.
* Go to File --> Backups --> backup discoveries to .txt and save all your locations to a txt file in .\backup. These are accessed on the Coordinate Share Tab.
* View location files on the Coordinate Share tab by double-clicking the file or the load lockbackup button. Click a location to view a location summary, and click the Move Player Here button to fast travel to the selected location.
* Right-click a location in the top Listbox on the Coordinate Share tab to create a one record location file to share with others or delete single records.
* Right-click the lower Listbox to delete unwanted location files.
* Use the Coordinate Calculator by clicking the Coordinate Calculator button and entering (1) coordinate at a time in the textboxes and clicking the button below it. View all converted coordinates at the bottom.
* On the Coordinate Calculator, clicking the glyphs will populate the Portal Code field. Then click calculate to see the coordinates.
* Enable Travel Mode to start tracking your Terminus locations before traveling through a terminus or when hitting the limit. See video above.


## Built With

* [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) - For the Windows 10 UI
* [Notepad++](https://notepad-plus-plus.org/) - For viewing Json
* [WinMerge](https://winmerge.org/) - Awesome tool for file comparison

## Authors

* **Kevin Lozano** - *Initial work* - [Kevin0M16](https://github.com/Kevin0M16)

## License

This project is licensed under the MIT License - see the [LICENSE](/LICENSE.txt) file for details

## Acknowledgments

* [nmssavetool](https://github.com/matthew-humphrey/nmssavetool) - Used the save file classes - This tool is still awesome!
* [NMSSaveEditor](https://github.com/goatfungus/NMSSaveEditor) - For viewing raw Json and testing my tool, also shout out to goatfungus for answering a few questions!
* [Swiss-Selector](https://kevin0m16.github.io/Swiss-Selector/) - For some code and examples. (My Car Mechanic tool)
* [r/NoMansSkyMods](https://www.reddit.com/r/NoMansSkyMods/) - For coordinate conversion info
* [nmsportals](https://nmsportals.github.io/) - For Glyph images
* [NMSGamepedia](https://nomanssky.gamepedia.com/Galaxy) - for galaxy numbers to names
