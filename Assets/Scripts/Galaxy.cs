using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Galaxy{

	public Galaxy()
    {
        //Don't do procedural generation in any kind of constructor.
        SolarSystems = new List<SolarSystem>();
    }

    public List<SolarSystem> SolarSystems;

    public void Generate(int numStars)
    {
        //Let's set a seed for the random number generator so we can test consistent results.
        UnityEngine.Random.InitState(181818);

        for (int i = 0; i < numStars; i++)
        {
            SolarSystem ss = new SolarSystem();
            ss.Generate();

            SolarSystems.Add(ss);
        }
        //Let's make a solar system with a single star and planet for now!
    }

    public void LoadFromFile(string fileName)
    {
        //Load an already generated system.
    }

    public void Update(UInt64 timeSinceStart)
    {
        //TODO: Consider only updating part of the galaxy if you have a TONNNNN of solar systems.

        foreach (SolarSystem ss in SolarSystems)
        {
            ss.Update(timeSinceStart);
        }
    }
}
