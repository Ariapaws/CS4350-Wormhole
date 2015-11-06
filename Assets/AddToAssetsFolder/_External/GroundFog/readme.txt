GroundFog   version 1.1   by VIS-Games
--------------------------------------------------
           http://www.vis-games.de
--------------------------------------------------


Version 1.1 Changelog:
----------------------
-Approved for Unity 3.5






--------------------------------------------------
--------------------------------------------------
--------------------------------------------------
Get started:

1) Import the package into your project.

2) Drag one of the "FXSystemGroundFog"-Prefabs (placed in subfolder "Prefabs") into your scene.

3) Configure the system as you like
   (If you need a position map to define the area in which the fog should be created, please
    have a look at the folder named "HowToCreatePositionMaps").

4) Import Position Maps: (Texture should be 256Colors Greyscale and 512x512 Pixels)

   Select the following Options in the Texture-Import-Dialog:
   -Texture Type: Advance
   -Read/Write Enabled: Yes
   -Alpha from Grayscale: Yes
   -Generate Mip Maps: No
   -Texture Format: Alpha 8


---------------------------------------------------
Configurable Values:

-Show Warnings in Console: If this is enabled, the system will show warning in the console if 
                           something has not been defined or defined in a strange way what might 
                           cause strange things.
		           You can disable this function when you are sure that everything works fine.
			
			   Notice: The shown warning are no(!) script errors!
			           The messages will created by the script when something strange has
                                   been configured that won´t cause errors but may create strange things.

-Enable GroundFog:      Enables/Disables the fog system
-Fog Color (Color32):   Defines the color (RGB) and the alpha of the fog
-Fog Mode:              -Stay in place (fog will animate in place)
 			-Flowing (fog will flow through your scene, over your mountains etc

-Fog Speed:             Defines the speed of the fog animation
-Viewport Camera or
 Object:		(needed for Distance Culling) A Camera or GameObject that represents  
                        the player or viewport position in your scene.	

-Fog View Distance:     (needed for Distance Culling) Defines the maximum view distance 
                        of the fog

-Scale Min/Scale Max:   Single Fog-Planes will be created randomly between those values		

-Fog Density:           The higher this value is set, the tighter gets the fog (higher values 
                        will cause more drawcalls).

-Use Unity Terrain:     If you want to create groundFog placed on an Unity Terrain, then
                        enable this button.
                        If this button is disabled, the fog will be created for a plane underground

-Terrain:		Add the terrain here, that should be used in the system

-Use Terrain Boundaries: If this button is enabled, the system will automaticly take the widht and 
                         lenght (X/Z) of your terrain as reference-coords.
                         Please notice, in this case, the terrain should be centered in your scene.
                         For example, if your terrain size is 500mx500m, the terrain should be located 
                         a the position -250.0, 0.0, -250,0
		         If the terrain is placed otherwise, please disable this flag and edit the
                         boundaries by hand in the fields below.

-Border X Left:		 The Border Values define the size and position of your terrain or 
 Border X Right:	 the range of your terrain you want to create fog at.
 Border Z Top:		 (Have a look at the Folder "HowToCreatePositionMaps") 
 Border Z Bottom:

-Use Position Map:	 If you want the fog only to be created at defined areas, you can 
			 use a positionmap which defines the areas. White color means "create fog"
                         and black means "no fog".

-Map Orientation:	 If you created your position map using a top down screenshot from your
                         scene, this option should set to "TOP VIEW". The Same for the bottom View.

-Ground Y Position:	 When you are using a plane underground, this value defines your ground height.


---------------------------------------------------
Hints:
-It is possible to use more than one fogsystem in one scene!
 (if you need some different looking systems in your scene)

-Don´t change Values in the running application!
-Have a look at the example scenes and check out the functionality.

-If the console shows errors, then please check the message. Some important user settings will be 
 missing. Fix it, and start again.
 When errors appear, the system will be terminated, so your application will continue running.

-if the console shows warnings, then something strange has been configured. This will not cause
 crahes or a script termination, but strange things might occure. So please read the warning
 and fix the problem.
 Those warning can be disabled in the inspector by disabling the "Show warnings in Console"-option.






		   		

