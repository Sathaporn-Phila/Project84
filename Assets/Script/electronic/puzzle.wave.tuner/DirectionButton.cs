using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionButton : MonoBehaviour
{
    public enum Direction {
        Left,Right
    }
    public enum ButtonBehaviour{
        Amplitude,Position
    }
    public Direction direction = new();
    public ButtonBehaviour buttonBehaviour = new();
    private void OnTriggerEnter(Collider other) {
        this.transform.parent.parent.Find("graph").GetComponent<GraphDisplay>().ChangeVal(direction,buttonBehaviour);
    }
}
