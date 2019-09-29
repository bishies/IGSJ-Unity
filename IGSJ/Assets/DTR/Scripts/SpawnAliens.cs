using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAliens : MonoBehaviour
{

    public float minSpawnRate;
    public float maxSpawnRate;
    public float SpawnPower;
    public float SpawnScaling;
    public float SpawnScalingRate;
    public int waitTime = 2;

    public List<GameObject> SpawnableAliens;

    private Bounds Bounds;

    private void Start()
    {
        //Starts the game after a small count down
        StartCoroutine(WaitForStart());
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(waitTime);

        //start endless spawn loops
        StartCoroutine(WaitAndSpawn());
        StartCoroutine(IncreaseSpawnRate());
    }

    IEnumerator WaitAndSpawn()
    {
        //wait between a min and max amount of seconds
        yield return new WaitForSeconds(Random.Range(minSpawnRate, maxSpawnRate));

        //spawn a random object and apply force to it
        if(Time.timeScale != 0.0f)
        {
            GameObject spawned = Instantiate(SpawnableAliens[Random.Range(0, SpawnableAliens.Count)], SpawnPoint(), Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360)));
            spawned.GetComponent<Rigidbody>().AddForce(transform.up * SpawnPower);
        }
        

        //loop this function
        StartCoroutine(WaitAndSpawn());
    }

    private Vector3 SpawnPoint()
    {
        //find a random point on the object
        Bounds = this.GetComponent<Collider>().bounds;
        return new Vector3(Random.Range(Bounds.min.x, Bounds.max.x), Random.Range(Bounds.min.y, Bounds.max.y), Random.Range(Bounds.min.z, Bounds.max.z));
    }

    IEnumerator IncreaseSpawnRate()
    {
        //wait between spawn rate increase
        yield return new WaitForSeconds(SpawnScalingRate);

        //increase spawn rate and catch any negatives
        minSpawnRate += SpawnScaling;
        maxSpawnRate += SpawnScaling;
        if (minSpawnRate <= 0) { minSpawnRate = 0; }
        if (maxSpawnRate <= 0) { maxSpawnRate = 0; }

        //loop this function
        StartCoroutine(IncreaseSpawnRate());
    }

}
