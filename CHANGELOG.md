# CHANGELOG

## v2.3.2 (DEC 30 2024)
- Fix for backup all locations feature.
  
## v2.3.1 (DEC 28 2024)
- Fix for incorrect json property type.
  
## v2.3 (DEC 25 2024)
- Update for NMS 5.x compatibility, keeping previous NMS 4.0 compatibility.
  
## v2.2 (NOV 4 2022)
- Update for NMS 4.0 compatibility and save names, difficulty
- Added new features for locations management. Merge, delete, open, etc.
- Built NMSSaveManager to handle compression plus obfuscate and deobfuscate save file content using libNOM.map class library

## v2.1 (JULY 23 2022)
- Reworked location json, new naming, added longhex
- More calculations, more efficient lookups
- Few other validations
- Reworked Coordinate Calculator

## v2.0 (JULY 21 2022)
- Major Rework
- Fixed Issue #39 - Save file compression issue
- Updates and internal fixes and organization
- New Colors
- Tested on Endurance

## v1.1.16 (AUG 15 2020)
- Fixed missing space stations in list
- Added (EB) for ExternalBase to list of locations
- Added a few more galaxies

## V1.1.15 (May 5 2020)
- Added Distance to Center calculation.
- Now scales to screen height when maximized.
- Now low resolution friendly to as low as 720 height.
- Automatically detects changes in resolution and adjusts accordingly.

## V1.1.14 (Jan 25 2020)
- Users can now add notes to identify each line in the list of locations accessed from the "Coordinate Share" tab. 

## V1.1.13 (Dec 16 2019)
- Fixed Gamemode not found for NMS 2.24, gamemode now uses ranges instead of specific values to prevent this in the future.

## V1.1.11 (Dec 12 2019)
- Added a New Feature: Users can now trigger a freighter battle on the next warp after save reload. Button on the Manual Travel Tab.
- Changed listbox1 on the bases and space stations tab to only show bases.
- Right click to Open location file from coordinates share tab
- Added a line in the location summary on the coordiantes share tab to show notes added at end of line.
- Added "reload save in game" to popups for clarity

## V1.1.10 (Dec 8 2019)
- Fixed Game mode not found due to synthesis update
- Galaxy type and name are now shown next to combobox on manual travel tab.
- Fixed a rare issue when NMS isn't installed in normal dir

## V1.1.9 (Oct 13 2019)
- NMSCoordinates now validates manually entered, shared, or calculator galactic coordinates.

## V1.1.8 (Sept 30 2019)
- Set Launch NMS button for GoG version to Program Files(x86) path.
		For GoG: Looks for GoG screenshots in PicturesNo Mans Sky and the DocumentsGoG Galaxy directory
		For Export Single Record: Can now add notes at the end of the line when notepad is opened.
		Example:
		DateTime: 09-29-2019 17:11 ## File: save3.hg ## G: 0 - PC: 018704FF4FFE -- GC: 07FD:0083:07F3:0187 -- notes here

## V1.1.7 (Sept 27 2019)
- Fixed a problem in savemanager on < button not backing up to savemanager zip

## V1.1.6 (Sept 25 2019)
- Please get the newest version 1.1.6 - Fixed a path problem that occurred on startup where path to saves was lost and had to be set every startup. 
