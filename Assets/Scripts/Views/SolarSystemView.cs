using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolarSystemView : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gc = GameObject.FindObjectOfType<GameController>();
        ShowSolarSystem(0);
    }

    GameController gc;
    SolarSystem solarSystem;

    public Text systemNumText;
    public Text specificSystemNum;

    //public SpaceObject[] Objects;
    public GameObject[] SunGameObjects, PlanetGameObjects, MoonGameObjects, AsteroidGameObjects, MiscOrbitalGameObjects;

    public ulong ZoomLevel = 1000000000; //500m zoom level

    private Dictionary<Orbital, GameObject> orbitalGameObjectMap = null;
    private Dictionary<Asteroid, GameObject> asteroidGameObjectMap = null;

    public List<GameObject> orbitalGOList = new List<GameObject>();

    public void ShowRandomSystem()
    {
        ShowSolarSystem(Random.Range(0, gc.Galaxy.SolarSystems.Count));
    }

    public void ShowSystemInputField()
    {
        //Debug.Log(specificSystemNum.text);
        int sysNum = -1;

        int.TryParse(specificSystemNum.text, out sysNum);

        if (sysNum > 0)
        {
            ShowSolarSystem(sysNum - 1);
        }

        specificSystemNum.transform.parent.GetComponent<InputField>().DeactivateInputField();
    }

    public void ShowSolarSystem(int solarSystemID)
    {
        if(gc.Galaxy.SolarSystems.Count <= solarSystemID)
        {
            Debug.LogError("Could not jump to system " + solarSystemID + ". Are you sure that is the correct ID?");
            return;
        }

        //First, clean up the solar system by deleting any old graphics we don't need anymore.
        while (transform.childCount > 0)
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null); //Become Batman.
            Destroy(c.gameObject);
        }

        orbitalGameObjectMap = new Dictionary<Orbital, GameObject>();
        asteroidGameObjectMap = new Dictionary<Asteroid, GameObject>();

        solarSystem = gc.Galaxy.SolarSystems[solarSystemID];

        //Spawn a graphic for each object in the solar system.

        for (int i = 0; i < solarSystem.Children.Count; i++)
        {
            SetupOrbitalGraphics(this.transform, solarSystem.Children[i]);
        }

        DrawOrbitCircles();

        systemNumText.text = ("#" + (solarSystemID + 1).ToString());
    }

    void SetUpAsteroidGraphics(AsteroidBelt a, GameObject beltContainer)
    {
        beltContainer.name = "Asteroid Belt";

        foreach (Asteroid asteroid in a.AsteroidChildList)
        {
            SetupOrbitalGraphics(beltContainer.transform, asteroid);
        }
    }

    void SetupOrbitalGraphics(Transform transformParent, Orbital o)
    {
        GameObject go = null;

        switch(o.OrbitalObjectType)
        {
            case Orbital.ObjectTypes.Sun:
                go = (GameObject)Instantiate(SunGameObjects[o.GraphicID]);
                go.name = "Sun";
                break;
            case Orbital.ObjectTypes.Planet:
                go = (GameObject)Instantiate(PlanetGameObjects[o.GraphicID]);
                go.name = "Planet";
                break;
            case Orbital.ObjectTypes.Moon:
                go = (GameObject)Instantiate(MoonGameObjects[o.GraphicID]);
                go.name = "Moon";
                break;
            case Orbital.ObjectTypes.Asteroid:
                go = (GameObject)Instantiate(AsteroidGameObjects[o.GraphicID]);
                go.name = "Asteroid";
                float rand = Random.Range(0.5f, 2f);
                Vector3 randVec = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
                go.transform.localScale = new Vector3(rand, rand, rand);
                go.transform.localRotation = Quaternion.Euler(randVec);
                break;
            case Orbital.ObjectTypes.AsteroidBelt:
                go = new GameObject();
                SetUpAsteroidGraphics((AsteroidBelt)o, go);
                break;
            default:
                Debug.LogError("Orbital type is invalid! " + o.OrbitalObjectType);
                go.name = "ERROR";
                break;
        }

        orbitalGameObjectMap[o] = go;
        go.transform.SetParent(transformParent);
        go.transform.position = o.Position / ZoomLevel;

        for (int i = 0; i < o.Children.Count; i++)
        {
            SetupOrbitalGraphics(go.transform, o.Children[i]);
        }
    }

    // Update is called once per frame
    void Update () {
        //Loop through each of our orbital objects and update their unity positions beased on a zoom level (that can change)

        for (int i = 0; i < solarSystem.Children.Count; i++)
        {
            UpdateObjects(solarSystem.Children[i]);
        }
	}

    void UpdateObjects(Orbital o)
    {
        GameObject go = orbitalGameObjectMap[o];
        go.transform.position = o.Position / ZoomLevel;
        go.transform.rotation = Quaternion.Euler(new Vector3(0, o.Rot, 0));

        for (int i = 0; i < o.Children.Count; i++)
        {
            UpdateObjects(o.Children[i]);
        }
    }

    public void SetZoomLevel(ulong zl)
    {
        ZoomLevel = zl;

        //Update planet positions
        //Also consider scaling the graphics up/down -- keep a min size in mind of course.
        //You probably want planets to be at least a few pixels big, regardless of zoom level.
    }

    private void DrawOrbitCircles()
    {
        foreach (GameObject go in orbitalGOList)
        {
            GameObject.Destroy(go);
        }

        orbitalGOList.Clear();

        foreach(KeyValuePair<Orbital, GameObject> o in orbitalGameObjectMap)
        {
            if(o.Key.OrbitalObjectType != Orbital.ObjectTypes.Asteroid && o.Key.OrbitalObjectType != Orbital.ObjectTypes.Sun && o.Key.OrbitalObjectType != Orbital.ObjectTypes.AsteroidBelt && o.Key.OrbitalObjectType != Orbital.ObjectTypes.Moon)
            {
                Debug.Log(o.Key.ToString() + ", " + o.Value.name.ToString());
                //Debug.Log(o.Key + " " + o.Key.OrbitalObjectType.ToString());
                ulong rad = o.Key.OrbitalDistance;

                GameObject path = new GameObject();
                path.name = "Orbit Path";
                orbitalGOList.Add(path);

                ulong circleRad = rad / ZoomLevel;

                float theta_scale = 0.1f;             //Set lower to add more points
                int size = Mathf.CeilToInt((2.0f * Mathf.PI) / theta_scale); //Total number of points in circle.

                LineRenderer lineRenderer = path.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
                lineRenderer.startColor = Color.white;
                lineRenderer.endColor = Color.white;

                if (o.Key.OrbitalObjectType == Orbital.ObjectTypes.Planet)
                {
                    lineRenderer.startWidth = 0.5f;
                    lineRenderer.endWidth = 0.5f;
                }
                else
                {
                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;
                }
                
                lineRenderer.numPositions = size;

                int i = 0;
                for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.1f)
                {
                    float x = circleRad * Mathf.Cos(theta);
                    float y = circleRad * Mathf.Sin(theta);

                    Vector3 pos = new Vector3(x, 0, y);
                    if (o.Key.Parent != null)
                        pos += (o.Key.Parent.Position / ZoomLevel);

                    lineRenderer.SetPosition(i, pos);
                    i += 1;
                }

                lineRenderer.SetPosition(lineRenderer.numPositions - 1, lineRenderer.GetPosition(0));
            }
        }            
    }
}
