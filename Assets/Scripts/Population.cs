using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Population : MonoBehaviour
{
	public GameObject botPrefab;
	public int populationSize = 50;
	public static float elapsed = 0;
	public static float trialTime = 5;

	private List<GameObject> population = new List<GameObject>();
	private int generation = 1;
	private float mutationChance = 0.05f;
	private float averageDistance = 0.0f;
	private float bestDistance = 0.0f;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 20;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0, 0, 140, 140), "Stats", guiStyle);
		GUI.Label(new Rect (10, 25, 200, 30), "Gen: " + generation, guiStyle);
		GUI.Label(new Rect (10, 50, 200, 30), string.Format("Time: {0:0.00}",elapsed), guiStyle);
		GUI.Label(new Rect (10, 75, 200, 30), "Average: " + averageDistance, guiStyle);
		GUI.Label(new Rect (10, 100, 200, 30), "Best: " + bestDistance, guiStyle);
		GUI.EndGroup ();
	}
	
	void Start ()
	{
		for(int i = 0; i < populationSize; i++)
		{
			Vector3 startingPos = new Vector3(transform.position.x + Random.Range(-2,2),
											  transform.position.y,
										      transform.position.z + Random.Range(-2,2));

			GameObject b = Instantiate(botPrefab, startingPos, transform.rotation);
			b.GetComponent<Brain>().Init();
			population.Add(b);
		}
	}

	GameObject Breed (GameObject parent1, GameObject parent2)
	{
		Vector3 startingPos = new Vector3(transform.position.x + Random.Range(-2,2),
										  transform.position.y,
										  transform.position.z + Random.Range(-2,2));
		GameObject offspring = Instantiate(botPrefab, startingPos, transform.rotation);
		Brain b = offspring.GetComponent<Brain>();
		b.Init();
		b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna, mutationChance);
		return offspring;
	}

	void BreedNewPopulation()
	{
		List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<Brain>().distanceMoved).ToList();
		averageDistance = population.Average(o => o.GetComponent<Brain>().distanceMoved);
		bestDistance = sortedList[0].GetComponent<Brain>().distanceMoved;

		population.Clear();
		for (int i = 0; i < sortedList.Count / 2; i++)
		{
    		population.Add(Breed(sortedList[i], sortedList[i + 1]));
    		population.Add(Breed(sortedList[i + 1], sortedList[i]));
		}
		// Destroy previous population
		for(int i = 0; i < sortedList.Count; i++)
			Destroy(sortedList[i]);
		generation++;
	}
	
	void Update ()
	{
		elapsed += Time.deltaTime;
		if(elapsed >= trialTime)
		{
			BreedNewPopulation();
			elapsed = 0;
		}
	}
}

