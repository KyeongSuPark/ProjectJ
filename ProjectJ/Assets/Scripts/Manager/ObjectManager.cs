using UnityEngine;
using System.Collections;

public class ObjectManager : MonoBehaviour {

    public static ObjectManager Instance = null;
    public GameObject m_Player;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Object manager has two instance");
    }

    public GameObject PlayerObject
    {
        get { return m_Player; }
    }
}
