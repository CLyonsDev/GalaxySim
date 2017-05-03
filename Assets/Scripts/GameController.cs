using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    float delay = 0.01f; //This is the real time delay.
    int secondsPerTick = 864;

    public Text yearText;
    public Text dayText;

    public int year = 35;
    public float day;

    private int daysPerYear = 1000;

	// Use this for initialization
	void OnEnable () {
        Galaxy = new Galaxy();
        Galaxy.Generate(100);
	}

    void Start()
    {
        StartCoroutine(TimeTick());
    }

    public Galaxy Galaxy;
	
	// Update is called once per frame
	void Update () {
        //AdvanceTime(8640);
    }

    ulong galacticTime = 0;

    public void AdvanceTime(int numSeconds)
    {
        galacticTime += (uint)numSeconds;

        day += ((secondsPerTick / 60f) / 60f);

        CalculateUI();

        Galaxy.Update(galacticTime);
    }

    public void RewindTime(int numSeconds)
    {
        galacticTime -= (uint)numSeconds;

        Galaxy.Update(galacticTime);
    }

    private void CalculateUI()
    {
        if(day > daysPerYear)
        {
            day = 0;
            year++;
        }

        yearText.text = "M." + year.ToString();
        dayText.text = day.ToString("000.0");
    }

    private IEnumerator TimeTick()
    {
        while (true)
        {
            AdvanceTime(secondsPerTick);
            yield return new WaitForSeconds(delay);
        }
    }
}
