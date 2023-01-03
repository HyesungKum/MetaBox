using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    [SerializeField] Animal animalPref = null;
    int animalNumber;
    float runAwayRange;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.Level == 1)
        {
            animalNumber = 4;
            runAwayRange = 2f;
            
        }
        else if (GameManager.Instance.Level == 2)
        {
            animalNumber = 6;
            runAwayRange = 1.8f;
        }
        else if (GameManager.Instance.Level == 3)
        {
            animalNumber = 8;
            runAwayRange = 1.5f;
        }
        else
        {
            animalNumber = 10;
            runAwayRange = 1.2f;
        }

        UIManager.Instance.AnimalNumber = animalNumber;
        for (int i = 0; i < animalNumber; i++)
        {
            Animal animal = Instantiate<Animal>(animalPref, new Vector3(Random.Range(-8f, 8f), Random.Range(-3.5f, 2f), 0), Quaternion.identity, this.transform);
            animal.RunAwayRange = runAwayRange;
        }
    }

}
