using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnObjects");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator SpawnObjects(){
        while(true){
            print("Spawning Objects");
            int num = Random.Range(0,prefabsToSpawn.Length);
            Instantiate(prefabsToSpawn[num], transform.position, transform.rotation);
            yield return new WaitForSeconds(1f);
        }
    }
}
