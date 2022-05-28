# CustomKinightRandom 
A Hollow Knight mod to load custom texture sheets for the player

### Installing the Mod Manually:

1. Extract the .zip file.
2. Place the output folder under the Mods/CustomKinightRandom directory:

``` 
Windows		C:\Program Files (x86)\Steam\steamapps\common\Hollow Knight\hollow_knight_Data\Managed\Mods\CustomKinightRandom\
Mac		~/Library/Application Support/Steam/steamapps/common/Hollow Knight/hollow_knight.app/hollow_knight_Data/Resources/Data/Managed/Mods/CustomKinightRandom/
Linux		~/.local/share/Steam/steamapps/common/Hollow Knight/hollow_knight_Data/Managed/Mods/CustomKinightRandom/
```

3. Open the game (may take a while) and load a save.
4. Pause the game and go to Settings > Mods > CustomKinightRandom & choose your desired skin.

### Installing Skins:

1. Download the skin 
2. Place the skin's images in a directory that is the same name as the skin
3. Copy this directory to the Skins directory under the game's Mods/CustomKinightRandom directoy (mentioned earlier)

Make sure that the folder you copy opens directly to the image files themselves. That is, when you double click on the folder, it opens up to the PNGs themselves and not to another folder or zip. 

### Creating your own skins:

1. Edit the existing skin images in your photo editing software of choice.
2. Save each skin under the exact same name as the PNG file that it is overriding in the Defaults folder, e.g. if you 
   are creating a skin for Grimmchild, save the image as "Grimm.png".
3. Put the edited images in a folder with the name that you want your custom skin to have.
4. To use your custom skin, follow steps 2 - 3 in the "Installing Skins" section.

Note that, You do not need to have all the images that Default has in your folder, just the ones you edit will suffice.


### Other Features:

#### CustomKinightRandom API

- adds ability to add more items that can be skinned (even custom items)
- adds ability to extend or modify existing skins at runtime 
- adds ability to provide a skin directly at runtime ( allows taking control of the skin without breaking compatibility)
- adds ability to set the current skin

See example mods under `AddonExample` directory in this repo for more, xml docs are added as they felt needed.

#### Swapper
- adds ability to skin bosses and any other objects that use a tk2dsprite 
- adds ability to skin any other non animated objects that use a sprite
- adds ability to "skin" text in the game 
- ability to dump the sprites / text to make it easier to figure out what to edit


Using Swapper as a skin author : 

 - to replace a sprite : `mods/CustomKinightRandom/Skins/<skin>/Swap/<scene-name>/<gameobjectname>.png`
(will only update once encountered in this scene, refer to the dumped png)

-- to replace a sprite globally look for the sprite in the `Global` directory it will allow you to replace enemies / npcs / objects globally if the file exists in this folder

- to replace a text using it's in-game key :  `mods/CustomKinightRandom/Skins/<skin>/Swap/<scene-name>/<key>.txt`
(scene-name here is just for organisation, the text will update globally)

- to replace a text without it's key (case insensitive find & replace) : add the text in `mods/CustomKinightRandom/Skins/<skin>/Swap/replace.txt`

for example :
```
elDer=>Cool Dude
geo=>Ca$hMoney
```
- to Dump assets (very laggy):  enable Swapper Dump in settings : enter the room you want to dump objects /text from and wait till that object or text is on screen and change scene.
( Dump is triggered by new object creation & scene change) 

Use Swapper to swap things across skins : 
 - create a Swap folder inside CustomKinightRandom directory : `mods/CustomKinightRandom/Swap/`
 - treat this as a global "skin" that applies regardless of the skin being selected.


