using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    public Color32 color;
    public GameObject[] prefabs;
}

public class GenerateAsteroidBeltFromFile : MonoBehaviour {

    public Texture2D layout;

    public ColorToPrefab[] colorToPrefab;

    public Color32 backgroundColor;

    public Material[] asteroidMaterials;

    private float spacing = 4f;

    public Vector2 heightRange = Vector2.zero;

    public Vector3 minSize, maxSize;

	// Use this for initialization
	void Start () {
        LoadMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void EmptyMap()
    {
        while(transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }
    }

    void LoadMap()
    {
        EmptyMap();

        //Get the pixels from the level imagemap
        Color32[] allPixels = layout.GetPixels32();
        int width = layout.width;
        int height = layout.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnAsteroidAt(allPixels[(y * width) + x], x, y);
            }
        }
    }

    void SpawnAsteroidAt(Color32 c, int x, int y)
    {
        //If the pixel is black, then nothing should be there.
        if (c.Equals(backgroundColor))
            return;

        //Find the right color in our map

        //Not really optimized. Try a dictionary instead for max performance.
        foreach (ColorToPrefab ctp in colorToPrefab)
        {
            if(ctp.color.Equals(c))
            {
                Quaternion randomRot = Quaternion.Euler(Random.Range(0, 180f), Random.Range(0, 180f), Random.Range(0, 180f));
                GameObject go = (GameObject)Instantiate(ctp.prefabs[Random.Range(0, ctp.prefabs.Length)], new Vector3(x * spacing, Random.Range(heightRange.x, heightRange.y), y * spacing), randomRot);
                go.GetComponent<Renderer>().material = asteroidMaterials[Random.Range(0, asteroidMaterials.Length)];
                go.transform.localScale = new Vector3((int)Random.Range(minSize.x, maxSize.x), (int)Random.Range(minSize.y, maxSize.y), (int)Random.Range(minSize.z, maxSize.z));
                go.transform.SetParent(this.transform);
                return;
            }
        }

        //If we are here, there is not a matching color in the array.
        Debug.LogError("No color to prefab dound for " + c.ToString());
        return;
    }
}
