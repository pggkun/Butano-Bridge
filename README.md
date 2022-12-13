# Butano-Bridge
Tool that exports Unity tilemaps to be used in butano (GValiente)

## Installation
Using Unity package manager, select: **Add package from git URL** with the link:
```
https://github.com/pggkun/Butano-Bridge.git?path=/Assets/Release
```
or download the **.unitypackage** from the last release at [releases page](https://github.com/pggkun/Butano-Bridge/releases)

## How to use
First you need to create a Butano Settings, that will store your project (made with butano) folders.

![creating butano settings](https://github.com/pggkun/Butano-Bridge/blob/main/GitHub/step01.png)
![browsing prooject folders](https://github.com/pggkun/Butano-Bridge/blob/main/GitHub/step02.png)

Then start creating your tilemaps. You will need to attach a **TilemapToBitmap** script to your tilemap gameObject.

This script allows you to choose the tile size (8x8 or 16x16) and export single tilemaps (with its own json file).

![creating tilemaps](https://github.com/pggkun/Butano-Bridge/blob/main/GitHub/step1.png)

![tilemap to bitmap](https://github.com/pggkun/Butano-Bridge/blob/main/GitHub/step2.png)

Now you need to create a empty GameObject and attach a **ButanoManager** script into it and choose your **ButanoSettings** created a few steps ago. This will allows the bitmap script to get info about your project folders.

![creating tilemaps](https://github.com/pggkun/Butano-Bridge/blob/main/GitHub/step3.png)

![creating tilemaps](https://github.com/pggkun/Butano-Bridge/blob/main/GitHub/step4.png)

Finally, to run you just need to press the **Export All** button at the **ButanoManager** gameObject. This will export all the tilemaps founded at the current scene and also will create a header file inside your project include folder with all of them listed in a `bn::span`.

![creating tilemaps](https://github.com/pggkun/Butano-Bridge/blob/main/GitHub/step5.png)
