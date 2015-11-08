using UnityEngine;
using System.Collections;

public class MinimapReveal : MonoBehaviour {

    private Texture2D tex;
    public int brushSize = 8; // Half size of brush -1

    private int width;
    private int height;

    private Color32[] texPixels;
    private RaycastHit hit;
    private Ray shootRay;

    private int count;
    private float POS_TO_TEXTURE_RATIO = 2.05f;
    private float widthCorrection;
    private float heightCorrection;
    private float starting_x = 17f;
    private float starting_y = 23f;

    public GameObject player;

    void Start()
    {
        tex = Instantiate(GetComponent<Renderer>().material.mainTexture as Texture2D); // duplicate original texture
        //tex = GetComponent<Renderer>().material.mainTexture as Texture2D;
        // Make changes to a copy 
        texPixels = tex.GetPixels32();
        width = tex.width;
        height = tex.height;
        widthCorrection = starting_x * (POS_TO_TEXTURE_RATIO - 1.0f) + starting_x + width / 2;
        heightCorrection = starting_y * (POS_TO_TEXTURE_RATIO - 1.0f) + starting_y + height / 2;
        GetComponent<Renderer>().material.mainTexture = tex;
        //renderer.material.mainTexture = tex; 
//        player = GameObject.FindGameObjectWithTag("Player");
        count = 0;
    }

    void Update ()
    {
        if (count == 10)
        {
            count = 0;
            shootRay.origin = player.transform.position; //should be player's position
            shootRay.direction = new Vector3(0,1,0);
            //Debug.Log("entered if count==10, origin: "+shootRay.origin);
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawLine(shootRay.origin, shootRay.GetPoint(30),Color.green,3.0f);
            if (GetComponent<MeshCollider>().Raycast(shootRay, out hit, Mathf.Infinity))
            {
                //Debug.Log(hit.transform.gameObject);
                Vector2 coord = hit.textureCoord;
                coord = new Vector2(hit.point.x * POS_TO_TEXTURE_RATIO + widthCorrection, 
                    hit.point.z* POS_TO_TEXTURE_RATIO + heightCorrection);

                //Debug.Log("Coord: "+coord);
                //Debug.Log("Point hit: " + hit.point);
                //Debug.Log("Height: " +height+", Width: "+width);
                //int w = (int)coord.x * width;
                //int h = (int)coord.y * height;
                int w = (int)coord.x;
                int h = (int)coord.y;

                for (int i = w - brushSize; i <= w + brushSize; i++)
                {
                    for (int j = h - brushSize; j <= h + brushSize; j++)
                    {
                        if (i >= 0 && j >= 0 && i <= width && j <= height)
                        {
                            if (Mathf.Sqrt(Mathf.Pow(i - w, 2) + Mathf.Pow(j - h, 2)) <= brushSize)
                            {
                                int k = j * width + i;

                                if (k < texPixels.Length)
                                    texPixels[k].a = 0;
                            }
                        }
                    }
                }

                tex.SetPixels32(texPixels);
                //tex.Apply(false);
                tex.Apply();
            }
        } else
        {
            count++;
        }
    }


}
