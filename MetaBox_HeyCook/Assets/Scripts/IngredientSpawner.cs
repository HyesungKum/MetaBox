using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public IngredData[] Ingredients;

    private List<IngredData> spawnTable = new();

    [field:SerializeField] int Level = 0;
    
    
    private void Awake()
    {
        switch (Level)
        {
            case 0:
                {
                    
                }
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    private void Update()
    {
        _SpawnIngredient();
    }

    private void _SpawnIngredient()
    {
        if (spawnTable.Count < 3)
        {

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if(collision.cou)
    }
}
