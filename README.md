##### [GitHub](https://github.com/Kylemc1413/MappingExtensions)
##### See Old Readme for legacy maps [HERE](README-Beatmapv2.md)

This adds a host of new things you can do with your maps, currently only possible through JSON editing until an editor adds support for the features. Keep in mind the following:

#### If you use any of these features, you MUST add "Mapping Extensions" as a requirement for your map for them to function, you can go [Here](https://github.com/Kylemc1413/SongCore/blob/master/README.md) to see how adding requirements to the info.dat works
## Capabilities
### _Extra Lanes/Layers_
- You can simply change the `Line Index`  and `Layer` to be an integer value outside of the normal range (-999 to 999) and they will show up in the expected position, letting you extend your map further outward/vertically, or simply placing things like bombs or walls further out for aesthetic
##### Supported By
    Notes
    Bombs
    Walls (Does not support negative layers)
    Chains
    Arcs
### _360 Degree note Rotation_
- You can change the `Cut Direction` of an object to a value between 1000 and 1360 to have the note be rotated from 0-360 degrees clockwise instead of the normal 45 degree deviations the game is limited to, 0 degrees is a down slice , 90 degrees is left slice, 180 up slice,  etc etc.
##### Supported By
    Notes
    Any Direction Notes (Use range of 2000-2360 instead)
    Bombs
    Chains
    Arcs
### _Precision Placement_
- You can also choose to place your objects in lanes/layers *between* normal layers, or simply redefine the space your map takes place in.

- If you change the line index to be 1000 or higher or -1000 and lower, you'll be placing the object on a "precise" lane, in that instead of every increase being a single lane, every increase will be 1/1000 of a lane.

- Do not attempt to use values between -1000 and 1000 for precision placement, using a value like 500 will simply move the object nearly 500 regular lanes outside of the normal space

- 1000/-1000 are both equivalent to the normal "leftmost" lane (Index 0), and going higher than 1000 or lower than -1000 will extend in their respective directions , so 2000 would be equivalent to a line index of 1, -2000 would be the equivalent of a line index of -1, etc etc, with the same logic applying to line layers as well. 
##### Supported By
    Notes
    Bombs
    Walls (Does not support negative layers)
    Chains
    Arcs
### _Additional Wall Adjustments_
- You can change the width of walls to be a value greater than or equal to 1000 to precisely control the width of the wall, with 1000 being the equivalent of 0 width, 2000 being the equivalent of 1 width, etc etc
- You can change the height of walls to be values higher than the normal base game cap
- You can change the height of walls to be a value greater than or equal to 1000 to precisely control the height of the wall, with 1000 being the equivalent of 0 height, 2000 being the equivalent of 1 height, etc etc
### Important Note
   Walls from older maps using an overloaded Type field will have height and start height automatically converted to layer and height values. This may not be entirely accurate at the moment, and walls may look slightly off on maps not made using the new format, improvements to this are welcomed if anybody that knows the format well and is good at math would like to tackle it.

