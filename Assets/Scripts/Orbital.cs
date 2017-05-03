using System;
using System.Collections.Generic;
using UnityEngine;

public class Orbital {

    public Orbital()
    {
        TimeToOrbit = 1;
        TimePerDay = 30000;
        OrbitalObjectType = ObjectTypes.Sun;
        Children = new List<Orbital>();
        objectLoc = new List<ulong>();
        InitAngle = UnityEngine.Random.Range(0, Mathf.PI * 2);
    }

    public Orbital Parent;
    public List<Orbital> Children;


    public enum ObjectTypes
    {
        Sun,
        Planet,
        Moon,
        Asteroid,
        AsteroidBelt
    };

    public ObjectTypes OrbitalObjectType;

    public float InitAngle; //What angle do we start at?
    public float OffsetAngle; //Angle around the parent (Radians)

    //public float OrbitalDistance;
    //public uint OrbitalDistance; //MAYBE in KMs? As long as nothing is further than 4.4bn from the sun...
    //Or...
    //64 BIT WOOOOOO
    public UInt64 OrbitalDistance; //METERRRRRSSSSSSSSSSS -- Probably overkill but WHO CARES 64 BIT WOOOO!!!
    //What's our max value? Well- happy you asked. It's 18,446,744,073,709,551,615. Yeah. That's 18 quintillion. We'll be fine.
    //For reference. Pluto in meters is only...                  4,000,000,000,000 METERS. We have the space (hue).

    public UInt64 TimeToOrbit; //Time to orbit in seconds. Gotta love percision \o/
    public UInt64 TimePerDay; //Time it takes to complete a full rotation (day) in real-world seconds.
    public float Rot;

    public int GraphicID;

    public ulong furthestObjectDist = 0;
    public List<ulong> objectLoc;

    // Internally, floats are represented basically like a scientific notation:
    // eg: 2.5676 * 10^6

    //Unsigned ints can go up to 4 billion (no negatives!)

    //So...

    //How big of a number will we need to represent in our space system?
    //Pluto is ~4,000,000,000 kms from the sun.

    //We could use a float to represent AU -- 1AU is avg distance from sun to earth.

    //We need to be able to get an x, y, and maybe z coord for our location for the purpose of rendering the Orbital on screen.
    public Vector3 Position
    {
        get
        {
            //TODO: Convert our orbit info into a vector we can use to render something as a Gameobject

            //Consider whether or not we should be saving Vector3 in a private var whenever we update our angle, or if it's no slower to just calc on demand.

            Vector3 myOffset = new Vector3(
                Mathf.Sin(InitAngle + OffsetAngle) * OrbitalDistance,
                0, //Y is locked to zero -- but do Inclination later (bc 3d).
                -Mathf.Cos(InitAngle + OffsetAngle) * OrbitalDistance
                );

            if(Parent != null)
            {
                myOffset += Parent.Position;
            }

            return myOffset;
        }
        set
        {
            Position = value;
        }
    }

    public void Update(UInt64 timeSinceStart)
    {
        // Advance our angle by the correct amount of time.
        if(OrbitalObjectType != ObjectTypes.Asteroid && OrbitalObjectType != ObjectTypes.AsteroidBelt)
        {
            OffsetAngle = (2f * Mathf.PI * (float)(timeSinceStart) / TimeToOrbit);
            Rot = (2f * Mathf.PI * (float)(timeSinceStart) / TimePerDay);

            //Update all of our children
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Update(timeSinceStart);
            }
        }
    }

    private ulong DecideFurthestDist()
    {
        ulong dist = 1;
        for (int i = 0; i < objectLoc.Count; i++)
        {
            //Debug.Log("Testing " + objectLoc[i]);
            if (objectLoc[i] > dist)
                dist = objectLoc[i];
        }

        //Debug.LogWarning("Furthest was " + dist);
        return dist;
    }

    public void AddChild(Orbital c)
    {
        c.Parent = this;
        c.Parent.objectLoc.Add(c.OrbitalDistance);
        c.Parent.furthestObjectDist = DecideFurthestDist();
        Children.Add(c);
    }

    public void RemoveChild(Orbital c)
    {
        c.Parent = null;
        c.Parent.objectLoc.Remove(c.OrbitalDistance);
        Children.Remove(c);
    }

    public ulong OrbitTimeForDist(float mass)
    {
        //FIXME: Real math
        //return 365 * 24 * 60 * 60;

        return (ulong)((2 * Mathf.PI) * (Mathf.Sqrt(Mathf.Pow(OrbitalDistance, 3) /(9.807f * (ulong)mass))));
    }

    public void DecideGraphic()
    {
        if ((int)OrbitalObjectType == (int)ObjectTypes.Planet)
        {
            GraphicID = UnityEngine.Random.Range(0, GameObject.Find("SolarSystemView").GetComponent<SolarSystemView>().PlanetGameObjects.Length); //TODO: Various values such as dist from sun.
            //Debug.Log("Planet " + GraphicID);
        }
        else if ((int)OrbitalObjectType == (int)ObjectTypes.Moon)
        {
            GraphicID = UnityEngine.Random.Range(0, GameObject.Find("SolarSystemView").GetComponent<SolarSystemView>().MoonGameObjects.Length); //TODO: Various values such as dist from sun.
            //Debug.Log("Moon " + GraphicID);
        }
        else if ((int)OrbitalObjectType == (int)ObjectTypes.Sun)
        {
            GraphicID = UnityEngine.Random.Range(0, GameObject.Find("SolarSystemView").GetComponent<SolarSystemView>().SunGameObjects.Length);
            //Debug.Log("Sun " + GraphicID);
        }else if((int)OrbitalObjectType == (int)ObjectTypes.Asteroid)
        {
            GraphicID = UnityEngine.Random.Range(0, GameObject.Find("SolarSystemView").GetComponent<SolarSystemView>().AsteroidGameObjects.Length);
        }
        else
            Debug.LogWarning("wtf " + OrbitalObjectType);
    }
}