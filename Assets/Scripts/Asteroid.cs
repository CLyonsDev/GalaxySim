using System;
using System.Collections.Generic;
using UnityEngine;

class Asteroid : Orbital
{
    public Asteroid()
    {
        AsteroidChildList = new List<Asteroid>();
    }

    public List<Asteroid> AsteroidChildList;

    public ulong Dist;

    public void AddChild(Asteroid asteroid)
    {
        asteroid.Parent = this;
        AsteroidChildList.Add(asteroid);
    }

    public void RemoveChild(Asteroid asteroid)
    {
        asteroid.Parent = null;
        AsteroidChildList.Remove(asteroid);
    }
}

