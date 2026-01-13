using UnityEngine;

public class GameResultsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) || GameMechanics.CountDown() <= 0 || GameManager.Instance.MoveStep <= 0)
        {
            GameManager.Instance.IsGameOver = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) || GameManager.Instance.Score >= 100)
        {
            GameManager.Instance.IsWinGame = true;
        }

    }
}
