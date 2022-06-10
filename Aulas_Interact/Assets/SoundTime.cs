using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTime : MonoBehaviour
{
    AudioSource audioSource;
    GameObject[] cubes = new GameObject[1024];
    GameObject[] bandCubes = new GameObject[7];
   
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < 1024; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = transform;
            cube.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
            cube.transform.Rotate(new Vector3(0, 360f / 1024 * i, 0));
            cube.transform.position += cube.transform.forward * 20;
            cube.name= "Sample "+i;
            cubes[i] = cube;
        }
       
        for(int i = 0; i < 7; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = transform;
            cube.GetComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
            cube.transform.Rotate(new Vector3(0, 360f / 7 * i, 0));
            cube.transform.position += cube.transform.forward * 10;
            cube.name = "Band " + i;
            bandCubes[i] = cube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float meanEnergy = AudioAnalysis.MeanEnergy(audioSource);
        float energyDB = AudioAnalysis.ConvertToDB(meanEnergy);
        transform.localScale = new Vector3(transform.localScale.x, energyDB / 10, transform.localScale.z);
        Grapher.Log(energyDB,"DB");
        float[] samples = AudioAnalysis.GetSpectrum(audioSource);
        for (int i = 0; i < 1024; i++)
        {

            cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, 10f * samples[i], cubes[i].transform.localScale.z);
        }
        float[] bands = AudioAnalysis.FreqBands(audioSource);
        for (int i = 0; i < 7; i++)
        {

            bandCubes[i].transform.localScale = new Vector3(bandCubes[i].transform.localScale.x,bands[i], bandCubes[i].transform.localScale.z);
        }
    }
}
