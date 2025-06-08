using UnityEngine;

public class StorageOfEmptyCells : MonoBehaviour
{
    [SerializeField] GameObject[] EmptyCells;

    public void FindAllEmptyCells()
    {
        EmptyCells = GameObject.FindGameObjectsWithTag("EmptyCell");
    }

    public GameObject[] emptycells 
    { 
        get { return EmptyCells; }
    }
}
