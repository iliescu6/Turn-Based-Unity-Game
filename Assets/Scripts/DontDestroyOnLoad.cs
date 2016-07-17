using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {

    public static DontDestroyOnLoad instance;

	void Awake () {
        MakeSingleton();
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
