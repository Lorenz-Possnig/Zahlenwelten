using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSpawner : MonoBehaviour
{
    public GameObject[] Digits = new GameObject[9];

    public Transform[] SpawnLocations = new Transform[3];
    public ParticleSystem[] particles = new ParticleSystem[3];

    private List<GameObject> spawns = new List<GameObject>();
    public Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DespawnNumbers()
    {
        foreach (var go in spawns)
        {
            DestroyImmediate(go);
        }
    }

    public void SpawnNumber(byte[] digits)
    {
        DespawnNumbers();
        for (int i = 0; i < digits.Length; i++)
        {
            var digit = digits[i];
            var goToSpawn = Digits[digit - 1];
            var location = SpawnLocations[i];
            var mat = materials[Random.Range(0, materials.Length)];
            var go = Instantiate(goToSpawn, location);
            go.GetComponent<Renderer>().material = mat;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(20, 20, 20);
            go.transform.rotation = Quaternion.Euler(0, 180, 0);
            spawns.Add(go);
            particles[i].startColor = mat.color;
            particles[i].Emit(10);
        }
    }
}
