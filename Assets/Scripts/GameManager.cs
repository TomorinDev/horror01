
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public GameObject ui;
    public GameObject player;
    public GameObject player2;
    public GameObject cameras;
    public GameObject lights;
    public GameObject timer;

    public string previousLevel = "RootScene";
    public string nextLevel = "level2";
    public string[] fakePlayerNames = { "Dice Laserbeam", "Joe", "Sally Supernova", "Quang Quantum", "Caroline Cosmic", "Blackhole Barry", "Carl Comet", "Adam Atomic", "Greg Galaxy", "Debby Dwarf Galaxy" };
    public float[] fakeTimes = { 16.5f, 30.7f, 34.2f, 32.3f, 90.4f, 50.6f, 70.1f, 105.9f, 25.8f, 140.2f };



    void Awake()
    {
        bool execute = SetSingleton();
        if (!execute)
            return;
    }

    bool SetSingleton()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }
    void Start()
    {
        
    }
    public void MakeChildOfGameManager(GameObject obj)
    {
        obj.transform.SetParent(this.transform);
    }
}
