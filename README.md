##### [GitHub](https://github.com/Kylemc1413/MappingExtensions)
This adds a host of new things you can do with your maps, currently only possible through JSON editing until an editor adds support for the features. If you map with these, they will be auto detected as requirements by Song Loader 6.0.0 or later, and players will not be able to play them without mapping extensions installed. HOWEVER:

#### Having a map with the new features in your library without Song Loader 6.0.0 or later will softlock your game

#### If you use any of these features, be sure to add "Mapping Extensions" as a requirement for your map, you can go [Here](https://github.com/Kylemc1413/BeatSaberSongLoader/blob/master/README.md#difficulty-json-additional-fields-ie-expertjson--etc) to see how adding requirements to the difficulty json works
## Capabilities
### _Extra Lanes/Layers_
- You can simply change the `_lineIndex`  and `_lineLayer` of a note/bomb/(walls can only change Index) to values outside the normal range and they will show up there accordingly, letting you extend your map further outward/vertically, or simply placing things like bombs or walls further out for aesthetic

### _360 Degree note Rotation_
- You can change the cutDirection of notes to a value between 1000 and 1360 to have the note be rotated from 0-360 degrees clockwise instead of the normal 45 degree deviations the game is limited to, 0 degrees is a down slice , 90 degrees is left slice, 180 up slice,  etc etc.

### _Precision note Placement_
- You can also choose to place your notes in lanes/layers *between* normal layers, or simply redefine the space your map takes place in.

- If you change the line index to be 1000 or higher or -1000 and lower, you'll be placing the note on a "precise" lane, in that instead of every increase being a single lane, every increase will be 1/1000 of a lane.

- Do not use values between -1000 and 1000, using a value like 500 will simply move the note nearly 500 regular lanes outside of the normal space

- 1000/-1000 are both equivalent to the normal "leftmost" lane, and going higher than 1000 or lower than -1000 will extend in their respective directions , so 2000 would be equivalent to a line index of 1, -2000 would be the equivalent of a line index of -1, etc etc, with the same logic applying to line layers as well. 

### _Precise Wall Adjustments_
- You can change the width of walls to be a value greater than or equal to 1000 to precisely control the width of the wall, with 1000 being the equivalent of 0 width, 2000 being the equivalent of 1 width, etc etc
- You can change the type of walls to be a value between 1000 and 4000 to control the height of the walls, with 1000 being a wall that has no height and is just flat on the ground, 2000 being a normal full height wall, and going higher than that (Up till 4000) being taller than normal
- You can also use precision line index placement like described for notes above
#### _More Precise Wall Adjustments_
- You can change the type of walls to be a value between 40001 and 4005000 to control how high up the wall starts and how high the wall itself is, the formula for which is below, with wall height ranging from 0 to 4000 and start height ranging from 0 to 999
- Whereas for Regular precision height a height of 1000 would be "0" for 4000+ treat the wall height as the literal value rather than adding 1000 to it when calculating
- Start height corresponds approximately to a wall height 4x as high, so a start height of 250 would equate to just above normal wall height 1000, so the wall would start just above the top of a normal wall
```cs
type = wall Height * 1000 + start height + 4001
//Example for a wall height of 2300 and a start height of 300
type = 2300 * 1000 + 300 + 4001 = 2304301
```
