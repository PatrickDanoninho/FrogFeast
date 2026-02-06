using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public GameManager gameManager;
    public MusicManager musicManager;

    public void Awake()
    {
        Instance = this;
        gameManager = GameManager.Instance;
        musicManager = MusicManager.Instance;
    }

    public BaseState currentBaseState;
    public EndGameStage EndGameStage = new EndGameStage();
    public GameStage GameStage = new GameStage();
    public CountDownStage CountDownStage = new CountDownStage();

    [Header("CountDown")]
    public float timer = 3;

    [Header("In Game Variables")]
    public int gameScore;
    public int playerMultiplier = 1;

    [Header("Spawn Controllers")]
    public List<GameObject> flyList = new List<GameObject>();
    public int flySpawnTimer = 5;
    public int flyNextSpawn = 0;
    public List<GameObject> stingerList = new List<GameObject>();
    public List<GameObject> debrisList = new List<GameObject>();

    [Header("Spwan Area")]
    [SerializeField] float minSpawnRadius = 2.5f;
    [SerializeField] float maxSpawnRadius = 6f;
    [SerializeField] float safeRadius = 2f;
    [SerializeField] float spawnHeight = 0.5f;

    [Header("SpawnOnTimer")]
    public bool waitForString = false;
    private string stringToWaitFor = "MusicBody";

    // Start is called before the first frame update
    void Start()
    {
        currentBaseState = CountDownStage;
        currentBaseState.EnterStage(this);

        CleanUpAndReset();
    }

    // Update is called once per frame
    void Update()
    {
        currentBaseState.UpdateStage(this);
    }

    public void SwitchStage(BaseState newStage)
    {
        MusicManager.OnBeat -= SpawnFly;
        MusicManager.OnMarker -= WaitForMarker;

        currentBaseState = newStage;
        currentBaseState.EnterStage(this);
    }

    //To use on CountDownStage
    /// <summary>
    /// CountDown before game starts
    /// </summary>
    public void CountDown()
    {
        timer -= Time.deltaTime;

        ContDownPanel contPanelScript = gameManager.CountPanelScript;

        int currentCount = Mathf.CeilToInt(timer);

        contPanelScript.SetCount(currentCount);

        if (timer <= 0f)
        {
            contPanelScript.SetCount(0);
            SwitchStage(GameStage);
        }
    }

    public void WaitForMarker(string marker) 
    {
        if (marker == stringToWaitFor)
        {
            waitForString = false;
        }
    }

    //To use on GameStage
    public void SpawnFly(int beat)
    {
        if (waitForString)
            return;

        if (flyList.Count == 0)
            return;

        //Pick first fly from list
        GameObject fly = flyList[0].gameObject;
        //Remove from list
        flyList.Remove(fly);

        //Place it and SetActive true
        fly.transform.position = GetRandomSpawnPosition(Vector3.zero);
        fly.SetActive(true);

        //Make Fly Move
        Fly flyScript = fly.GetComponent<Fly>();
        flyScript.Spawn();
        //Interactable flyInteractable = fly.GetComponent<Interactable>();
        //flyInteractable.InteractableBehaviour();

    }

    /// <summary>
    /// Spawn stingers where needed, using the list of available gameObjects
    /// Increase <param name="spawnSpeed"></param> as player multiplier goes up
    /// </summary>
    public void SpwanStinger(float spawnSpeed)
    {

    }

    /// <summary>
    /// Spawn debris where needed, using the list of available gameObjects
    /// Increase <param name="spawnSpeed"></param> as player multiplier goes up
    /// </summary>
    public void SpwanDebris(float spawnSpeed)
    {

    }

    public Vector3 GetRandomSpawnPosition(Vector3 frogPosition) 
    {
        Vector3 pos;

        int safety = 0;

        do
        {
            float x = Random.Range(-minSpawnRadius, minSpawnRadius + 0.1f);
            float z = Random.Range(-maxSpawnRadius, maxSpawnRadius + 0.1f);

            pos = new Vector3(x, spawnHeight, z);

            safety++;
            if (safety > 20)
                break;

        } while (Vector3.Distance(pos, frogPosition) < safeRadius);

        return pos;
    }

    public void ResetInteractable(List<GameObject> interactableListToJoin, GameObject interactableToAdd) 
    {
        interactableListToJoin.Add(interactableToAdd);
        interactableToAdd.SetActive(false);
    }

    //To use on EndGameStage
    public void CleanUpAndReset()
    {
        foreach (GameObject fly in flyList)
            fly.SetActive(false);
    }

    public void UpdateGameScore() 
    {
        gameScore++;
        Debug.Log("Flies Eaten this game = " + gameScore);
        gameManager.UpdateInGameUI(gameScore);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(
            new Vector3((-minSpawnRadius + minSpawnRadius) * 0.5f, spawnHeight, (-maxSpawnRadius + maxSpawnRadius) * 0.5f),
            new Vector3(minSpawnRadius - (-minSpawnRadius), 0.1f, maxSpawnRadius - (-maxSpawnRadius))
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, safeRadius);
    }
}
