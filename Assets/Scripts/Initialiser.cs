using UnityEngine;

public static class Initialiser 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void Execute()
    {
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PersistenceObjects")));
    }
}
