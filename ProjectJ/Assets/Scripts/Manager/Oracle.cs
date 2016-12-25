using UnityEngine;
using System.Collections;

/// <summary>
/// 프로그램 실행 되는 동안 살아있는 객체
/// 최소한의 필요한 정보만 담을 것
/// </summary>
public class Oracle : MonoBehaviour {

    private static Oracle m_Instance = null;

    private AccountData m_AccountData;      ///< 계정 데이터

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}

    public void Init()
    {
        m_AccountData = new AccountData();
        m_AccountData.CreateDummyStageData();
    }

    public static Oracle Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameObject("Oracle").AddComponent<Oracle>();
                m_Instance.Init();
            }

            return m_Instance;
        }
    }

    public AccountData GetAccountData() { return m_AccountData; }
}
