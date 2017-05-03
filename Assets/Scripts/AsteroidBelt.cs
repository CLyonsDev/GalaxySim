using System;
using System.Collections.Generic;
using UnityEngine;

class AsteroidBelt : Asteroid
{
    UInt64 asteroidGap; //The distance between asteroids (meters)

    public void Generate(int numAsteroids, bool debug)
    {
        Dist = 0;

        //ulong distFromAnyOtherObject = 12440000000;
        ulong distFromAnyOtherObject = 67000000000;
        ulong potentialDist = 0;

        while (Dist < 1)
        {
            if(debug)
            {
                Debug.LogWarning("Starting location finding process.");
            }

            float furthestDist = (Parent.furthestObjectDist / 1000000) / 1000;
            //Debug.Log(furthestDist);
            float value = UnityEngine.Random.Range(75f, furthestDist);
            //Debug.Log(value);
            potentialDist = ((ulong)value * 1000000 * 1000);
            bool good = true;

            for (int i = 0; i < Parent.Children.Count; i++)
            {
                if(debug)
                {
                    Debug.Log(Parent.Children[i].OrbitalDistance - potentialDist + " vs " + distFromAnyOtherObject + "(" + (Parent.Children[i].OrbitalDistance - potentialDist >= distFromAnyOtherObject) + ")");
                }

                if ((Parent.Children[i].OrbitalDistance - potentialDist) >= distFromAnyOtherObject && good)
                {

                }
                else
                {
                    good = false;
                }
            }

            if (good)
                Dist = potentialDist;
            else
                return;
                //Debug.LogWarning("No, that's not good... trying again. " + potentialDist);
        }



        if (Dist != 0)
        {
            OrbitalDistance = Dist;
            //Debug.Log("Set dist to " + OrbitalDistance);
        }
        else
            Debug.LogError("Something's fucky...");

        OrbitalObjectType = ObjectTypes.AsteroidBelt;

        for (int i = 0; i < numAsteroids; i++)
        {
            Asteroid a = new Asteroid();

            a.OrbitalDistance = (ulong)UnityEngine.Random.Range(3, 20) * 1000000 * 1000;

            a.TimeToOrbit = 0;
            a.OffsetAngle = UnityEngine.Random.Range(0, 359);


            //Debug.Log("Position set to " + a.Position);
            a.OrbitalObjectType = ObjectTypes.Asteroid;

            a.DecideGraphic();
            AddChild(a);

            //Debug.Log("Added asteroid to the belt. We are now at " + AsteroidChildList.Count + " asteroids.");
        }
    }
}
