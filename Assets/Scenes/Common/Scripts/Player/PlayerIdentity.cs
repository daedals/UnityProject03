using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentity : MonoBehaviour
{
    private static int uniqueID = 0;

    [SerializeField] private int _value;
    public int Value 
    { 
        get { return _value; }
        private set { _value = value; }
    }

    private void OnEnable()
    {
        Value = uniqueID++;
    }
}
