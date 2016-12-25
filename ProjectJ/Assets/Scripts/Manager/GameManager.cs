using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;
    public Canvas m_Canvs;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Object manager has two instance");
    }
    // Use this for initialization
	void Start () {
        //. 게임 시작되면 Run 상태로 변경
        GameObject playerObj = ObjectManager.Instance.PlayerObject;
        Player player = playerObj.GetComponent<Player>();
        player.ChangeState(ePlayerState.Run);
	}

    /// <summary>
    ///   장애물 부딪혔을 때 호출
    /// </summary>
    public void OnCollideObstacle()
    {
        //. Todo. Game Over 화면 보여준다.
        SceneManager.LoadScene(R.String.SCENE_LOBBY);
    }
}
