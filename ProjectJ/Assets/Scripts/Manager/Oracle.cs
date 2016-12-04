using UnityEngine;
using System.Collections;

/// <summary>
/// 프로그램 실행 되는 동안 살아있는 객체
/// 최소한의 필요한 정보만 담을 것
/// </summary>
public class Oracle : MonoBehaviour {

    private static Oracle m_Instance = null;
	// Use this for initialization
	void Start () {
        m_Instance = this;
        DontDestroyOnLoad(this);
	}


    public static Oracle Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new GameObject("Oracle").AddComponent<Oracle>();

            return m_Instance;
        }
    }
}
