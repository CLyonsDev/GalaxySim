using System;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : Orbital
{
    public void Generate()
    {
        //Make a single star with a single planet orbiting.

        Orbital myStar = new Orbital();
        myStar.DecideGraphic();
        myStar.OrbitalObjectType = ObjectTypes.Sun;
        this.AddChild(myStar);

        int planetNum = UnityEngine.Random.Range(1, 12);
        for (int i = 0; i < planetNum; i++)
        {
            Planet planet = new Planet();
            planet.OrbitalObjectType = ObjectTypes.Planet;
            planet.Generate(2);
            myStar.AddChild(planet);
        }

        //Asteroid belts
        int beltNum = UnityEngine.Random.Range(5, 13);
        for (int i = 0; i < beltNum; i++)
        {
            //Debug.Log("Generating Belt.");
            AsteroidBelt belt = new AsteroidBelt();
            belt.OrbitalObjectType = ObjectTypes.AsteroidBelt;
            myStar.AddChild(belt);
            belt.Generate(UnityEngine.Random.Range(25, 50), false);
        }
    }
}

