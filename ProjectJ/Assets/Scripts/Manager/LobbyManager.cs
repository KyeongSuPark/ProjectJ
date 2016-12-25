using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour {
    public static LobbyManager Instance = null;
    public GameObject m_Player;

	// Use this for initialization
	void Start () {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Object manager has two instance");

        Player player = m_Player.GetComponent<Player>();
        player.ChangeState(ePlayerState.Idle);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClicked_StageButton(Stage _stageData)
    {
        string sceneName = string.Format("{0}_Lv_{1}", R.String.SCENE_GAME, _stageData.Id);
        SceneManager.LoadScene(sceneName);
    }
}
