using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWall : MonoBehaviour
{
    public GameObject Wall;

    public void remove()
    {
        Wall.SetActive(false);

    }

}
