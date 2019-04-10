using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    /*
     Note: The background is a 3d obj (quad). To apply a texture, change the texture type in the 
     inspector of the sprite to default. Change the wrap mode to repeat for scrolling. 
     Drag the texture on to the quad.
    */
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    Material myMaterial;
    Vector2 offSet;


    // Start is called before the first frame update
    void Start()
    {
        // grabs the material from Mesh Renderer component on background
        myMaterial = GetComponent<Renderer>().material;
        offSet = new Vector2(0f, backgroundScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        // scrolls the backgound
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;
    }
}
