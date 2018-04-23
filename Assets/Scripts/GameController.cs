using Assets.Scripts.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private Room start;
    private Room finish;

    public enum EndedCause {
        WIN, DIED
    }

    // Enemy Prefabs
    [Header("Enemys")]
    [SerializeField]
    GameObject scorpion;
    [SerializeField]
    GameObject bug;

    [Space(10)]
    // Generation Settings
    [Header("Tile Prefabs")]
    [SerializeField]
    GameObject BorderOuter;
    [SerializeField]
    GameObject BorderInner;
    [SerializeField]
    GameObject BorderSingle;
    [SerializeField]
    GameObject Ground0;
    [SerializeField]
    GameObject Ground1;
    [SerializeField]
    GameObject Ground2;
    [SerializeField]
    GameObject Ground3;
    [SerializeField]
    GameObject DoorInner;
    [SerializeField]
    GameObject DoorOuter;
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
    [SerializeField]
    GameObject Flag;

    private Dictionary<GenerationProcessor.ExtendedTileType, GameObject> genPrefabs;
    private Dictionary<Enemy.Enemys, GameObject> enemyPrefabs;

    [Space(10)]
    [Header("References")]

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject ui;

    [SerializeField]
    private GameObject cam;

    [SerializeField]
    private GameObject mapRoot;

    private bool engineInitDone;
    private Player player;

    public static GameController instance;
    public GameController() {
        instance = this;
    }

    public enum GameState { UNSET, INIT, STARTING, RUNNING, ENDED };
    private EndedCause endCause = EndedCause.DIED;
    private GameState state = GameState.UNSET;

	// Use this for initialization
	void Start () {
        genPrefabs = new Dictionary<GenerationProcessor.ExtendedTileType, GameObject> {
            { GenerationProcessor.ExtendedTileType.BorderOuter, BorderOuter },
            { GenerationProcessor.ExtendedTileType.BorderInner, BorderInner },
            { GenerationProcessor.ExtendedTileType.BorderSingle, BorderSingle },
            { GenerationProcessor.ExtendedTileType.Rock, Rock },
            { GenerationProcessor.ExtendedTileType.RockL, RockL },
            { GenerationProcessor.ExtendedTileType.RockU, RockU },
            { GenerationProcessor.ExtendedTileType.RockR, RockR },
            { GenerationProcessor.ExtendedTileType.RockD, RockD },
            { GenerationProcessor.ExtendedTileType.RockLU, RockLU },
            { GenerationProcessor.ExtendedTileType.RockLR, RockLR },
            { GenerationProcessor.ExtendedTileType.RockLD, RockLD },
            { GenerationProcessor.ExtendedTileType.RockLURD, RockLURD },
            { GenerationProcessor.ExtendedTileType.RockRD, RockRD },
            { GenerationProcessor.ExtendedTileType.RockUR, RockUR },
            { GenerationProcessor.ExtendedTileType.RockUD, RockUD },
            { GenerationProcessor.ExtendedTileType.RockLUD, RockLUD },
            { GenerationProcessor.ExtendedTileType.RockLUR, RockLUR },
            { GenerationProcessor.ExtendedTileType.RockURD, RockURD },
			{ GenerationProcessor.ExtendedTileType.RockLRD, RockLRD },
			{ GenerationProcessor.ExtendedTileType.Ground0, Ground0 },
			{ GenerationProcessor.ExtendedTileType.Ground1, Ground1 },
			{ GenerationProcessor.ExtendedTileType.Ground2, Ground2 },
			{ GenerationProcessor.ExtendedTileType.Ground3, Ground3 },
			{ GenerationProcessor.ExtendedTileType.DoorInner, DoorInner },
			{ GenerationProcessor.ExtendedTileType.DoorOuter, DoorOuter },
            { GenerationProcessor.ExtendedTileType.Flag, Flag }
        };
        enemyPrefabs = new Dictionary<Enemy.Enemys, GameObject> {
            { Enemy.Enemys.SCORPION, scorpion },
            { Enemy.Enemys.BUG, bug }
        };

    }

    // Update is called once per frame
    void Update() {
        if ( !engineInitDone ) {
            engineInitDone = true;
            Debug.Log("First Frame");
            ChangeState(GameState.INIT);
        }
    }

    public void ChangeState(GameState nextState) {
        if ( nextState != state ) {
            state = nextState;
            StateLogic(nextState);
        }
    }

    void StateLogic(GameState nstate) {
        switch ( nstate ) {
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

    private void Init() {
        // Generation
        DungeonGenerator dg = new DungeonGenerator();
        GenerationProcessor gp = new GenerationProcessor(genPrefabs);
        dg.Generate();

        // Start room
        GameObject goStart = gp.ProcessRoom(dg.start.tiles);
        goStart.name = "Start";
        List<Transform> lt = new List<Transform>(goStart.GetComponentsInChildren<Transform>());
        GameObject doorRoot = new GameObject();
        doorRoot.name = "Doors";
        doorRoot.transform.SetParent(goStart.transform);
        start = goStart.AddComponent<Room>();
        start.SetCenter(dg.start.GetCenter());
        lt = lt.FindAll(x => x.tag == "door");
        lt.ForEach(x => {
            x.SetParent(doorRoot.transform);
            x.gameObject.GetComponent<Door>().SetParent(start);
            });
        start.SetDoorsRootObject(doorRoot);

        // Spawnpoint
        GameObject spawnpointRoot = new GameObject();
        spawnpointRoot.name = "Spawnpoints";
        spawnpointRoot.transform.SetParent(goStart.transform);
        spawnpointRoot.transform.position = new Vector3(dg.start.roomPosition.x, dg.start.roomPosition.y, 0);
        GameObject spawn = new GameObject();
        spawn.transform.SetParent(spawnpointRoot.transform);
        spawn.transform.position = new Vector3(dg.start.GetCenter().x, dg.start.GetCenter().y, 0);
        start.SetSpawnPointsRootObject(spawnpointRoot);

        start.Reload();

        start.transform.SetParent(mapRoot.transform);

        // Finish room
        GameObject goFinish = gp.ProcessRoom(dg.end.tiles);
        goFinish.name = "Finish";
        List<Transform> ltf = new List<Transform>(goFinish.GetComponentsInChildren<Transform>());
        GameObject doorRootf = new GameObject();
        doorRootf.name = "Doors";
        doorRootf.transform.SetParent(goFinish.transform);
        ltf = ltf.FindAll(x => x.tag == "door");
        finish = goFinish.AddComponent<Room>();
        finish.SetCenter(dg.end.GetCenter());
        ltf.ForEach(x => {
            x.SetParent(doorRootf.transform);
            x.gameObject.GetComponent<Door>().SetParent(finish);
        });
        finish.SetDoorsRootObject(doorRootf);
        finish.Reload();
        finish.transform.SetParent(mapRoot.transform);
		gp.CreateGOFromType(finish.GetCenter(), 0, Room.TileType.DOOR, GenerationProcessor.ExtendedTileType.Flag, goFinish);

		// Other Rooms
		foreach (GenRoom gr in dg.rooms) {
            GameObject groom = gp.ProcessRoom(gr.tiles);
            List<Transform> ltg = new List<Transform>(groom.GetComponentsInChildren<Transform>());

            // Doors
            GameObject doorRootg = new GameObject();
            doorRootg.name = "Doors";
            doorRootg.transform.SetParent(groom.transform);
            Room grom = groom.AddComponent<Room>();
            grom.SetCenter(gr.GetCenter());
            ltg = ltg.FindAll(x => x.tag == "door");
            ltg.ForEach(x => {
                x.SetParent(doorRootg.transform);
                x.gameObject.GetComponent<Door>().SetParent(grom);
                });

            // Spawnpoints
            GameObject tSpawnpointRoot = new GameObject();
            tSpawnpointRoot.name = "Spawnpoints";
            tSpawnpointRoot.transform.SetParent(groom.transform);
            tSpawnpointRoot.transform.position = new Vector3(gr.roomPosition.x, gr.roomPosition.y, 0);
            foreach(Vector2Int v in gr.spawnpoints) {
                GameObject tspawn = new GameObject();
                tspawn.transform.SetParent(tSpawnpointRoot.transform);
                tspawn.transform.position = new Vector3(v.x, v.y, 0); // is this the center or the top left corner of a block?
            }

            grom.SetSpawnPointsRootObject(tSpawnpointRoot);
            grom.SetDoorsRootObject(doorRootg);
            grom.Reload();
            DungeonGenerator.GenerateObjective(grom);
            groom.transform.SetParent(mapRoot.transform);
        }

        // Hallways
        GameObject goHallways = gp.ProcessRoom(dg.path.tiles);
        goHallways.name = "Hallways";
        goHallways.AddComponent<Room>();
        goHallways.transform.SetParent(mapRoot.transform);
    }

    private void Starting() {

        StartObjective goal = new StartObjective(start, playerPrefab);
        start.SetObjective(goal);
        start.OnPlayerEnter(player);
        player = goal.GetPlayer();
        if ( player != null ) {
            cam.GetComponent<CameraControl>().SetFollow(player.gameObject);
            GetUI().InitHealthController(player);
            GetUI().InitBrakeController(player);
        } else {
            Debug.Log("No Player spawned!");
        }
        finish.SetObjective(new FinishObjective(finish));
        cam.GetComponent<AudioControl>().LevelBgm();
    }

    private void Running() {

    }

    private void Ended() {
        Debug.Log("Game ended");
        //Time.timeScale = 0;
        if ( ui != null ) {
            Debug.Log("show end UI");
            if(endCause == EndedCause.DIED) {
                cam.GetComponent<AudioControl>().GameOverBgm();
                ui.GetComponent<UIController>().ShowGameOverUI();
            } else if(endCause == EndedCause.WIN) {
				//cam.GetComponent<AudioControl>().SfxPlay(2);
				player.InflictDamage(int.MaxValue/2);
                ui.GetComponent<UIController>().ShowWinUI();
            }
        } else {
            Debug.Log("No UI specified");
        }
    }

    public AudioControl GetAudioControl() {
        return cam.GetComponent<AudioControl>();
    }

    public UIController GetUI() {
        return ui.GetComponent<UIController>();
    }

    public Dictionary<Enemy.Enemys, GameObject> GetEnemyPrefabs() {
        return enemyPrefabs;
    }

    public bool GameEnded() {
        return state == GameState.ENDED;
    }

    public void EndGame(EndedCause cause) {
		if (endCause != null && state == GameState.ENDED)
			return; // Already ended game
        endCause = cause;
        ChangeState(GameState.ENDED);
    }
}
