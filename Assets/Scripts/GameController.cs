using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    private Room start;
    private Room finish;

    // Generation Settings
    [Header("Tile Prefabs")]
    [SerializeField]
    GameObject BorderOuter;
    [SerializeField]
    GameObject BorderInner;
    [SerializeField]
    GameObject BorderSingle;
    [SerializeField]
    GameObject Ground;
    [SerializeField]
    GameObject Door;
    [SerializeField]
    GameObject Rock;
    [SerializeField]
    GameObject RockL;
    [SerializeField]
    GameObject RockU;
    [SerializeField]
    GameObject RockR;
    [SerializeField]
    GameObject RockD;
    [SerializeField]
    GameObject RockLU;
    [SerializeField]
    GameObject RockLR;
    [SerializeField]
    GameObject RockLD;
    [SerializeField]
    GameObject RockUR;
    [SerializeField]
    GameObject RockUD;
    [SerializeField]
    GameObject RockRD;
    [SerializeField]
    GameObject RockLURD;
    [SerializeField]
    GameObject RockLUD;
    [SerializeField]
    GameObject RockLUR;
    [SerializeField]
    GameObject RockURD;
    [SerializeField]
    GameObject RockLRD;

    private Dictionary<GenerationProcessor.ExtendedTileType, GameObject> genPrefabs;

    [SerializeField]
    private GameObject ui;

    private bool engineInitDone;

    public static GameController instance;
    public GameController()
    {
        instance = this;
    }

    public enum GameState { UNSET, INIT, STARTING, RUNNING, ENDED };

    private GameState state = GameState.UNSET;

	// Use this for initialization
	void Start () {
        genPrefabs = new Dictionary<GenerationProcessor.ExtendedTileType, GameObject>();
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.BorderOuter, BorderOuter);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.BorderInner, BorderInner);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.BorderSingle, BorderSingle);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.Rock, Rock);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockL, RockL);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockU, RockU);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockR, RockR);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockD, RockD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLU, RockLU);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLR, RockLR);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLD, RockLD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLURD, RockLURD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockRD, RockRD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockUR, RockUR);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockUD, RockUD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLUD, RockLUD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLUR, RockLUR);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockURD, RockURD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.RockLRD, RockLRD);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.Ground, Ground);
        genPrefabs.Add(GenerationProcessor.ExtendedTileType.Door, Door);

        //ChangeState(GameState.INIT);
    }
	
	// Update is called once per frame
	void Update () {
        if (!engineInitDone)
        {
            engineInitDone = true;
            Debug.Log("First Frame");
            ChangeState(GameState.INIT);
        }
    }

    public void ChangeState(GameState nextState) {
        if(nextState != state) {
            state = nextState;
            StateLogic(nextState);
        }
    }

    void StateLogic(GameState nstate) {
        switch (nstate)
        {
            case GameState.INIT:
                Init();
                ChangeState(GameState.STARTING);
                break;
            case GameState.STARTING:
                Starting();
                ChangeState(GameState.RUNNING);
                break;
            case GameState.RUNNING:
                Running();
                break;
            case GameState.ENDED:
                Ended();
                break;
            default:
                print("Wrong GameState for StateLogic() call!");
                break;
        }

    }

    private void Init()
    {
        List<GameObject> tmp = new List<GameObject>
        {
            playerPrefab
        };
        start.SetObjective(new EntityObjective(start, tmp));
        start.OnPlayerEnter();
    }

    private void Starting()
    {
        
    }

    private void Running()
    {
        
    }

    private void Ended()
    {
        Debug.Log("Game ended");
        //Time.timeScale = 0;
        if (ui != null) {
            Debug.Log("show gameover UI");
            ui.GetComponent<UIController>().ShowGameOverUI();
        } else {
            Debug.Log("No ui specified");
        }
    }

}
