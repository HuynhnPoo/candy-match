using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootstrapManager : MonoBehaviour
{
    private static bool isBootstrapped = false;
    private static BootstrapManager instance;

    private GameObject allManagerGO;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (isBootstrapped)
            return;

        // Instantiate the GameManager directly from Resources
        GameObject managerPrefab = Resources.Load<GameObject>("GameManager");
        if (managerPrefab == null)
        {
            Debug.LogError("GameManager prefab not found in Resources!");
            return;
        }

        GameObject managerInstance = Instantiate(managerPrefab);
        instance = managerInstance.GetComponent<BootstrapManager>();
        if (instance == null)
        {
            Debug.LogError("BootstrapManager component not found on GameManager prefab!");
            Destroy(managerInstance);
            return;
        }

        isBootstrapped = true;
        DontDestroyOnLoad(managerInstance);
        Debug.Log("Bootstrap complete. GameManager initialized in current scene.");
    }

    private void Awake()
    {
        // Prevent duplicate instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Load any additional setup if needed
        allManagerGO = gameObject; // If you need to reference this GameObject
    }

    public static bool IsBootstrapped()
    {
        return isBootstrapped;
    }


}

