using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private Transform player;
    private Animator transition;
    private Movement playerMovement;

    [SerializeField] private float transitionTime = 1f;
    private float levelEndX;

    private int buildIdx;
    private bool isTransitioning = false;

    private void Awake()
    {
        transition = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public int getBuildIdx()
    {
        return buildIdx;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (player != null)
        {
            Debug.Log((player.position.x > levelEndX) + " Level End: " + levelEndX);
            
            if (!isTransitioning && player.position.x >= levelEndX)
            {
                LoadNextLevel();
            }
        }
    }

    public void LoadNextLevel()
    {
        isTransitioning = true;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        if (transition != null)
        {
            transition.SetTrigger("Start");
        }
        
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        buildIdx = scene.buildIndex;
        
        StartCoroutine(InitializeSceneElements(scene.buildIndex));

        isTransitioning = false;
    }

    private IEnumerator InitializeSceneElements(int currentBuildIndex)
    {
        yield return new WaitForEndOfFrame();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerMovement = playerObj.GetComponent<Movement>();
        }

        if (transition != null)
        {
            transition.Play("CrossFade_end", 0, 0f);
        }

        if (currentBuildIndex == 0)
        {
            levelEndX = 86f;
            if (playerMovement != null) playerMovement.SetSpawn(new Vector3(-19, 1, 0));
        }
        if (currentBuildIndex == 1)
        {
            levelEndX = 108.5f;
            if (playerMovement != null) playerMovement.SetSpawn(new Vector3(0, 0, 0));
        }
    }
}
