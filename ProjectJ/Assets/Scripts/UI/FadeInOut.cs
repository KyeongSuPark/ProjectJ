using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {

    /// <summary>
    ///   Fade 동작
    /// </summary>
    public enum FadeAction
    {
        In,
        Out,
    }

    public FadeAction m_eFadeAction; ///< 페이드 동작
    public float m_Duration = 1.0f;

    private Texture2D m_FadeTexture; 

    private Color m_StartColor;
    private Color m_EndColor;
    private Color m_CurrentColor;

	// Use this for initialization
	void Start () {
        m_FadeTexture = new Texture2D(1, 1);
        m_FadeTexture.SetPixel(0, 0, Color.black);

        if (m_eFadeAction == FadeAction.In)
            FadeIn();
        else if (m_eFadeAction == FadeAction.Out)
            FadeOut();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.depth = -10; //숫자가 낮을수록 나중에 출력되므로 가장 앞에 출력된다
        GUI.color = m_CurrentColor; // 아래의 텍스쳐가 그려질 때 투명도를 조정한다
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_FadeTexture);
    }

    void FixedUpdate()
    {
        // 점차 투명하게
        m_CurrentColor = Color.Lerp(m_StartColor, m_EndColor, Time.time / m_Duration);
    }

    /// <summary>
    ///   점차 밝게
    /// </summary>
    public void FadeIn()
    {
        m_StartColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        m_EndColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Fade();
    }

    /// <summary>
    ///  점차 어둡게
    /// </summary>
    public void FadeOut()
    {
        m_StartColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        m_EndColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Fade();
    }

    void Fade()
    {
        m_CurrentColor = m_StartColor;
        Destroy(gameObject, m_Duration + 1);
    }
}
