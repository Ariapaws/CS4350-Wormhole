using UnityEngine;
using System.Collections;

public class GroundFog : MonoBehaviour {
//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
//
//                                                     GroundFog System 
//
//                                          by Andre "AEG" Bürger / VIS-Games 2012
//
//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------

    public bool show_warnings = true;

    public bool fog_active = false;

    public bool use_fog_position_map = false;

    public Texture2D fog_position_map;

    public enum MAP_ORIENTATION
    {
        TOP_VIEW,
        BOTTOM_VIEW,
    };
    public MAP_ORIENTATION fog_map_orientation;
    
    public float MAP_X_LEFT;
    public float MAP_X_RIGHT;
    public float MAP_Z_TOP;
    public float MAP_Z_BOTTOM;

    public int number_fogplanes = 50;

    public float fog_scale_min;
    public float fog_scale_max;

    public float fog_speed = 10.0f;

    public GameObject player_position;

    public bool using_terrain_engine;
    public bool use_full_terrain_size;

    public Terrain fog_terrain;

    public float fog_view_dist;

    public float ground_y_pos;

    public Color32 fogcolor;

    public enum FOG_MODE
    {
        STAY_IN_PLACE,
        FLOWING,
    };
    public FOG_MODE fog_mode;
    //--------------------------------
    // used objects
    GameObject fogplane;


    //--------------------------------
    // internal data structs
    public Texture2D editor_image;

    float XRANGE;
    float ZRANGE;
    
    int fog_position_map_width;
    int fog_position_map_height;

    GameObject[] fog_obj;
    Vector3[] fog_pos;
    Vector3[] fog_size;
    float[] fog_zrot;

    float[] fog_rot_speed;

    TerrainData terrain_data;

    int frame_index;

//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
void Awake() 
{
    //-----------------------------------------------------------------------------
    //-----------------------------------------------------------------------------
    //-----------------------------------------------------------------------------
    //
    // Destroy System if disabled
    //
    if(fog_active == false)
    {
        DestroyImmediate(gameObject);
        return;
    }
    //-----------------------------------------------------------------------------
    //-----------------------------------------------------------------------------
    //-----------------------------------------------------------------------------
    //
    // Check for missing or wrong parameters that might cause errors
    //

    //-- missing position map
    if( (use_fog_position_map == true) && (fog_position_map == null) )
    {
        Debug.LogError("GroundFog: Position map is enabled but no position map has been assigned. Please add the position map in the inspector. Groundfog will be destroyed.");       
        DestroyImmediate(gameObject);
        return;
    }

    //-- mission viewport camera or object
    if(player_position == null)
    {     
        Debug.LogError("GroudFog: No Camera or GameObject that represents the player/viewport-position has been assigned. Please add a Camera or GameObject in the inspector that represents the player viewport position. Groundfog will be destroyed.");
        DestroyImmediate(gameObject);
        return;
    }

    //-- missing terrain
    if( (using_terrain_engine == true) && (fog_terrain == null) )
    {
        Debug.LogError("GroundFog: Using terrain is enabled but no terrain has been assigned. Please add the terrain to use in the inspector. Groundfog will be destroyed.");
        DestroyImmediate(gameObject);
        return;
    }

    //-- missing map scales
    if((MAP_X_LEFT == 0.0f) && (MAP_X_RIGHT == 0.0f) && (MAP_Z_TOP == 0.0f) && (MAP_Z_BOTTOM == 0.0f))
    {
        if((use_fog_position_map == true) && (use_full_terrain_size == false) && (using_terrain_engine == true))
        {
            Debug.LogError("GroundFog: Missing Boundary-Settings. Please add the Boundary-Values in the inspector. GroundFog will be destroyed");
            DestroyImmediate(gameObject);
            return;
        }

        if(using_terrain_engine == false)
        {
            Debug.LogError("GroundFog: Missing Boundary-Settings. Please add the Boundary-Values in the inspector. GroundFog will be destroyed");
            DestroyImmediate(gameObject);
            return;
        }
    }
    //-----------------------------------------------------------------------------
    //-----------------------------------------------------------------------------
    //-----------------------------------------------------------------------------
    //
    // Check for missing or wrong parameters that might cause unwanted results
    if(show_warnings == true)
    {
        //-- scale min = or < scale max
        if(fog_scale_max <= fog_scale_min)
        {
            fog_scale_max = fog_scale_min + 0.1f;
            Debug.LogWarning("GroundFog: Fog-Scale-Max should be greater than Fog-Scale-Min. Please change this in the inspector. Value has been increased to keep system running.");
        }

        //-- scale min <= 0.0f
        if(fog_scale_min <= 0.0f)
        {
            fog_scale_min = 0.01f;
            Debug.LogWarning("GroundFog: Fog-Scale-Min is less or equal 0.0f. The value should be greater. The Value has been set to 0.01f.");
        }
        //-- scale max <= 0.0f
        if(fog_scale_max <= 0.0f)
        {
            fog_scale_max = 0.01f;
            Debug.LogWarning("GroundFog: Fog-Scale-Max is less or equal 0.0f. The value should be greater. The Value has been set to 0.01f.");
        }

        //-- fog view distance to small
        if(fog_view_dist <= 1.0f)
            Debug.LogWarning("GroundFog: The Fog-View-Distance is set to less than 1m. This value should be higher. Please fix this in the inspector.");

    }
    //----------------------------------------------------------
    int i;

    float xpos;
    float ypos;
    float zpos;

    float size;
    //----------------------------------------------------------
    fogplane = transform.Find("fogplane").gameObject;

    if(using_terrain_engine == true)
    {   
        terrain_data = fog_terrain.terrainData;
        
        if(use_full_terrain_size == true)
        {
            MAP_X_LEFT   = fog_terrain.transform.position.x;
            MAP_Z_BOTTOM = fog_terrain.transform.position.z;

            MAP_X_RIGHT = MAP_X_LEFT + terrain_data.size.x;
            MAP_Z_TOP   = MAP_Z_BOTTOM + terrain_data.size.z;
        }

    }

    //-- Map rotated 180 degrees around the y-axis (LightWave 3D TopView, Unity3D Bottom View)
    if(fog_map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)
    {
        XRANGE = -MAP_X_LEFT + MAP_X_RIGHT;
        ZRANGE = MAP_Z_TOP + (-MAP_Z_BOTTOM);
    }
    else //-- Map unrotated (Unity3D Top View)
    {
        XRANGE = MAP_X_RIGHT - MAP_X_LEFT;        
        ZRANGE = MAP_Z_TOP - MAP_Z_BOTTOM;
    }

    fog_position_map_width = fog_position_map.width;
    fog_position_map_height = fog_position_map.height;

    frame_index = 0;
    //-------------------------------------------------------
    fog_obj = new GameObject[number_fogplanes];    
    fog_pos = new Vector3[number_fogplanes];

    fog_zrot = new float[number_fogplanes];

    fog_size = new Vector3[number_fogplanes];

    fog_rot_speed = new float[number_fogplanes];
    //-------------------------------------------------------
    if(fog_active == true)
    {
        i = 0;
        while(i < number_fogplanes)
        {
            xpos = (float)((UnityEngine.Random.value) * XRANGE);
            zpos = (float)((UnityEngine.Random.value) * ZRANGE);

            if(use_fog_position_map == true)
            {
                //-- check if position is allowed
                int map_xpos = (int)( (xpos / XRANGE ) * fog_position_map_width);
                int map_zpos = (int)( (zpos / ZRANGE ) * fog_position_map_height);
                
                if(fog_map_orientation == MAP_ORIENTATION.BOTTOM_VIEW)    
                {
                    if(fog_position_map.GetPixel(fog_position_map_width - map_xpos, fog_position_map_height - map_zpos).a == 1.0f)
                    {
                        size = (float)(((UnityEngine.Random.value) * (fog_scale_max - fog_scale_min)) + fog_scale_min);
                        fog_size[i] = new Vector3(size, size, size);

                        if(using_terrain_engine == false)
                            ypos = ground_y_pos;
                        else
                            ypos = (terrain_data.GetInterpolatedHeight(xpos / XRANGE, 1 - (zpos / ZRANGE)) ) - 0.2f;

                        xpos = xpos - MAP_X_LEFT - XRANGE;
                        zpos = zpos + MAP_Z_BOTTOM;

                        fog_pos[i] = new Vector3(xpos, ypos, zpos);

                        if(fog_mode == FOG_MODE.STAY_IN_PLACE)
                            fog_rot_speed[i] = Random.Range(-10.0f * fog_speed, 10.0f * fog_speed);
                        else
                            fog_rot_speed[i] = Random.Range( -1.0f * fog_speed, -10.0f * fog_speed);

                        fog_obj[i] = null;

                        i++;
                    }
                }
                else
                {
                    if(fog_position_map.GetPixel(map_xpos, fog_position_map_height - map_zpos).a == 1.0f)
                    {
                        size = (float)(((UnityEngine.Random.value) * (fog_scale_max - fog_scale_min)) + fog_scale_min);
                        fog_size[i] = new Vector3(size, size, size);

                        if(using_terrain_engine == false)
                            ypos = ground_y_pos;
                        else
                            ypos = (terrain_data.GetInterpolatedHeight(xpos / XRANGE, 1 - (zpos / ZRANGE)) ) - 0.2f;

                        xpos = xpos + MAP_X_LEFT;
                        zpos = MAP_Z_TOP - zpos;

                        fog_pos[i] = new Vector3(xpos, ypos, zpos);

                        if(fog_mode == FOG_MODE.STAY_IN_PLACE)
                            fog_rot_speed[i] = Random.Range(-10.0f * fog_speed, 10.0f * fog_speed);
                        else
                            fog_rot_speed[i] = Random.Range( -1.0f * fog_speed, -10.0f * fog_speed);

                        fog_obj[i] = null;

                        i++;
                    }
                }
            }
            else // no position map -> full region
            {
                size = (float)(((UnityEngine.Random.value) * (fog_scale_max - fog_scale_min)) + fog_scale_min);
                fog_size[i] = new Vector3(size, size, size);

                if(using_terrain_engine == false)
                    ypos = ground_y_pos;
                else
                    ypos = (terrain_data.GetInterpolatedHeight(xpos / XRANGE, 1 - (zpos / ZRANGE)) ) - 0.2f;

                xpos = xpos + MAP_X_LEFT;
                zpos = MAP_Z_TOP - zpos;

                fog_pos[i] = new Vector3(xpos, ypos, zpos);

                if(fog_mode == FOG_MODE.STAY_IN_PLACE)
                    fog_rot_speed[i] = Random.Range(-10.0f * fog_speed, 10.0f * fog_speed);
                else
                    fog_rot_speed[i] = Random.Range( -1.0f * fog_speed, -10.0f * fog_speed);

                fog_obj[i] = null;

                i++;
            }
        }
    }
    //-- update all planes
    for(i=0;i<32;i++)
        Update();
}

//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
void Update() 
{
    int i;
    
    float viewport_yrot = player_position.transform.rotation.eulerAngles.y;
    //-------------------------------------------------------
    //-------------------------------------------------------
    //-------------------------------------------------------
    //
    // Update Fogplanes
    //
    if(fog_active == true)
    {
        for(i=0;i<number_fogplanes;i++)
        {   
            if( (i & 31) == frame_index)
            {
                if( Vector3.Distance(player_position.transform.position, fog_pos[i]) < fog_view_dist)
                {
                    //-- Check if object was active last frame
                    if(fog_obj[i] == null)
                    {    
                        //-- Create fogplane
                        fog_obj[i] = (GameObject)Instantiate(fogplane);
                        fog_obj[i].transform.parent = gameObject.transform;
                        fog_obj[i].transform.position   = fog_pos[i];
                        fog_obj[i].transform.localScale = fog_size[i];
                    }
                }
                else //-- out of view distance
                {
                    if(fog_obj[i] == true)
                    {
                        Destroy(fog_obj[i]);  
                        fog_obj[i] = null;
                    }
                }
            }
            //-- rotate fogplane    
            if(fog_obj[i] != null) 
            {
                fog_zrot[i] += ((Time.deltaTime * fog_rot_speed[i]) % 360.0f);
                fog_obj[i].transform.eulerAngles = new Vector3 (0.0f, viewport_yrot, fog_zrot[i]);
            }
        }
    }
    frame_index = (frame_index + 1) & 31;            

    //-- set fogcolor
    fogplane.GetComponent<Renderer>().material.SetColor("_TintColor", fogcolor);
}
//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------
}
