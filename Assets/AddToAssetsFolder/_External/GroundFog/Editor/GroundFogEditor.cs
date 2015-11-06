using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GroundFog))]

public class GroundFogEditor : Editor  {
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//
//                Ground Fog - Editor Script Version 1.0  
//
//                by Andre "AEG" Bürger / VIS-Games 2012
//
//                       http://www.vis-games.de
//    
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------



//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
public void Start()
{
    GroundFog groundfog = target as GroundFog;

    //-- init parameters
    groundfog.show_warnings = true;
    
    groundfog.fog_active = false;

    groundfog.use_fog_position_map = false;

    groundfog.fog_position_map = null;

    groundfog.fog_map_orientation = GroundFog.MAP_ORIENTATION.TOP_VIEW;
    
    groundfog.MAP_X_LEFT    = 0.0f;
    groundfog.MAP_X_RIGHT   = 0.0f;
    groundfog.MAP_Z_TOP     = 0.0f;
    groundfog.MAP_Z_BOTTOM  = 0.0f;

    groundfog.number_fogplanes = 500;

    groundfog.fog_scale_min = 16;
    groundfog.fog_scale_max = 32;

    groundfog.player_position = null;

    groundfog.using_terrain_engine = false;
    groundfog.use_full_terrain_size = false;

    groundfog.fog_speed = 10.0f;

    groundfog.fog_terrain = null;

    groundfog.fog_view_dist = 150.0f;

    groundfog.ground_y_pos = 0.0f;

    groundfog.fogcolor = new Color( (8 / 256), (15 / 256), (14 / 256) , (31 / 256) );

    groundfog.fog_mode = GroundFog.FOG_MODE.STAY_IN_PLACE;

    groundfog.editor_image = null;
}
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
public override void OnInspectorGUI()
{
	EditorGUIUtility.LookLikeControls(200, 50);

    GroundFog groundfog = target as GroundFog;

   	if(!groundfog.editor_image)
    {
		groundfog.editor_image = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/GroundFog/Textures/editor_logo.png", typeof(Texture2D));
	}
    EditorGUILayout.Separator();
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // draw image
    //
	Rect imageRect = EditorGUILayout.BeginHorizontal();
	imageRect.x = imageRect.width / 2 - 160;
	if (imageRect.x < 0) {
		imageRect.x = 0;
	}
	imageRect.width = 320;
	imageRect.height = 140;
	GUI.DrawTexture(imageRect, groundfog.editor_image);
	EditorGUILayout.EndHorizontal();

    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();
    EditorGUILayout.Separator();

    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Enable / Disable Warnings
    //
    groundfog.show_warnings     = (bool)EditorGUILayout.Toggle("Show Warnings in Console", groundfog.show_warnings);
    EditorGUILayout.Separator();
   
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // Enable / Disable Groundfog
    //
    groundfog.fog_active = (bool)EditorGUILayout.Toggle("Enable Groundfog", groundfog.fog_active);
    EditorGUILayout.Separator();
    
    if(groundfog.fog_active == true)
    {
        //-- Fog Color
        groundfog.fogcolor = (Color)EditorGUILayout.ColorField("Fog Color", groundfog.fogcolor);
        EditorGUILayout.Separator();

        //-- Fog Mode
        groundfog.fog_mode = (GroundFog.FOG_MODE)EditorGUILayout.EnumPopup("Fog Mode", groundfog.fog_mode);
        EditorGUILayout.Separator();

        EditorGUILayout.PrefixLabel("Fog Speed");
        groundfog.fog_speed = EditorGUILayout.Slider(groundfog.fog_speed, 1.0f, 15.0f);
        EditorGUILayout.Separator();
    
        //-- Player Position
        groundfog.player_position = (GameObject)EditorGUILayout.ObjectField("Viewport Camera or Object", groundfog.player_position, typeof(GameObject), true);    
        EditorGUILayout.Separator();

        //-- Fog View Distance
        groundfog.fog_view_dist = (float)EditorGUILayout.FloatField("Fog View Distance", groundfog.fog_view_dist);
        EditorGUILayout.Separator();

        //-- Fogplane scale min/max
        groundfog.fog_scale_min = (float)EditorGUILayout.FloatField("Scale Min", groundfog.fog_scale_min);
        groundfog.fog_scale_max = (float)EditorGUILayout.FloatField("Scale Max", groundfog.fog_scale_max);
        EditorGUILayout.Separator();

        //-- Number Fogplanes
        EditorGUILayout.PrefixLabel("Fog Density");
        groundfog.number_fogplanes = EditorGUILayout.IntSlider(groundfog.number_fogplanes, 50, 5000);
        EditorGUILayout.Separator();

        //-- Using Unity Terrain
        groundfog.using_terrain_engine = (bool)EditorGUILayout.Toggle("Use Unity Terrain", groundfog.using_terrain_engine);
        EditorGUILayout.Separator();
        if(groundfog.using_terrain_engine == true)
        {
            groundfog.fog_terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", groundfog.fog_terrain, typeof(Terrain), true);                      
            EditorGUILayout.Separator();

            groundfog.use_full_terrain_size = (bool)EditorGUILayout.Toggle("Use Terrain Boundaries", groundfog.use_full_terrain_size);    
            EditorGUILayout.Separator();

            //-- Map Boundaries
            if(groundfog.use_full_terrain_size == false)
            {
                groundfog.MAP_X_LEFT   = (float)EditorGUILayout.FloatField("Border X Left",   groundfog.MAP_X_LEFT);
                groundfog.MAP_X_RIGHT  = (float)EditorGUILayout.FloatField("Border X Right",  groundfog.MAP_X_RIGHT);
                groundfog.MAP_Z_TOP    = (float)EditorGUILayout.FloatField("Border Z Top",    groundfog.MAP_Z_TOP);
                groundfog.MAP_Z_BOTTOM = (float)EditorGUILayout.FloatField("Border Z Bottom", groundfog.MAP_Z_BOTTOM);
                EditorGUILayout.Separator();
            }

        }
        else // Using a plane underground
        {
            //-- Ground YPos
            groundfog.ground_y_pos = (float)EditorGUILayout.FloatField("Ground Y Position", groundfog.ground_y_pos);
            EditorGUILayout.Separator();
        
            //-- Map Boundaries
            groundfog.MAP_X_LEFT   = (float)EditorGUILayout.FloatField("Border X Left",   groundfog.MAP_X_LEFT);
            groundfog.MAP_X_RIGHT  = (float)EditorGUILayout.FloatField("Border X Right",  groundfog.MAP_X_RIGHT);
            groundfog.MAP_Z_TOP    = (float)EditorGUILayout.FloatField("Border Z Top",    groundfog.MAP_Z_TOP);
            groundfog.MAP_Z_BOTTOM = (float)EditorGUILayout.FloatField("Border Z Bottom", groundfog.MAP_Z_BOTTOM);
            EditorGUILayout.Separator();
        
        }
        
        //-- Position Map
        groundfog.use_fog_position_map = (bool)EditorGUILayout.Toggle("Use Position Map", groundfog.use_fog_position_map);    
        EditorGUILayout.Separator();
        if(groundfog.use_fog_position_map == true)
        {
            groundfog.fog_position_map = (Texture2D)EditorGUILayout.ObjectField("Fog Position Map", groundfog.fog_position_map, typeof(Texture2D), true);            
            EditorGUILayout.Separator();

            groundfog.fog_map_orientation = (GroundFog.MAP_ORIENTATION)EditorGUILayout.EnumPopup("Map Orientation", groundfog.fog_map_orientation);
            EditorGUILayout.Separator();
        }
    }     
    //----------------------------------------------------
    //----------------------------------------------------
    //----------------------------------------------------
    //
    // end
    //
	if (GUI.changed)
        EditorUtility.SetDirty (groundfog);
}
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
//-------------------------------------------------------------------------------------
}
