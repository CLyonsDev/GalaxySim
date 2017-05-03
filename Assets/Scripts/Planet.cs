using UnityEngine;

class Planet : Orbital
{
    public Planet()
    {

    }

    public void Generate(int maxMoons)
    {
        //Randomize our values
        OrbitalDistance = (ulong)Random.Range(100, 1000) * 1000000 * 1000; // 5-1000, million, KM
        TimeToOrbit = OrbitTimeForDist(5.972f * Mathf.Pow(10, 24));

        OrbitalObjectType = ObjectTypes.Planet;
        DecideGraphic();

        int m = Random.Range(0, maxMoons + 1);
        for (int i = 0; i < m; i++)
        {
            Orbital mun = new Orbital();
            mun.OrbitalObjectType = ObjectTypes.Moon;
            this.AddChild(mun);
            ulong dist = (ulong)Random.Range(500, 1000) * 1000000 * 10; //7500000000
            mun.OrbitalDistance = dist;
            mun.TimeToOrbit = mun.OrbitTimeForDist(7.348f * Mathf.Pow(10, 22));
        }
    }

    public void MakeEarth()
    {
        OffsetAngle = 0; // "North" of the sun.
        OrbitalDistance = 150000000000; // 150 MILLION KM! (150 BILLION M :v)
        TimeToOrbit = OrbitTimeForDist(5.972f * Mathf.Pow(10, 24));
    }
}