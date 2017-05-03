using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///   카메라 연출을 하는 클래스
/// </summary>
public class CameraDirection : MonoBehaviour
{
    private iTweenPath m_ColorPath;  ///< Color 모드에서 사용할 Path
    private iTweenPath m_NormalPath; ///< Normal 모드에서 사용할 Path
    public Transform m_LookTarget;   ///< 카메라 타겟

    private bool bFlag = false;
	// Use this for initialization
	void Start () {
        //. Path 초기화
        Camera mainCam = Camera.main;
        List<iTweenPath> paths = new List<iTweenPath>();
        mainCam.GetComponents<iTweenPath>(paths);
        foreach (var path in paths)
        {
            if (path.pathName == R.String.COLOR_MODE_PATH)
                m_ColorPath = path;
            else if (path.pathName == R.String.NORMAL_MODE_PATH)
                m_NormalPath = path;
        }

        //. 첫번째 노드와 카메라 위치 차이만큼 이동
        Vector3 firstNode = m_ColorPath.nodes[0];
        Vector3 dist = Camera.main.transform.position - firstNode;
        for (int i = 0; i < m_ColorPath.nodes.Count; ++i)
        {
            m_ColorPath.nodes[i] += dist;
        }
        
        //. 마지막 노드와 카메라 위치 차이만큼 이동
        Vector3 lastNode = m_NormalPath.nodes[m_NormalPath.nodes.Count - 1];
        dist = Camera.main.transform.position - lastNode;
        for (int i = 0; i < m_NormalPath.nodes.Count; ++i)
        {
            m_NormalPath.nodes[i] += dist;
        }

	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Fire1"))
        {
            if (bFlag)
                CameraDirection.MoveToNoramlMode();
            else
                CameraDirection.MoveToColorMode();

            bFlag = !bFlag;
        }
	}

    public void MoveToPath(string _pathName)
    {
        //. 이름에 맞는 Path 가져 오고
        iTweenPath path = null;
        if (_pathName == R.String.COLOR_MODE_PATH)
        {
            path = m_ColorPath;
        }
        else if (_pathName == R.String.NORMAL_MODE_PATH)
        {
            path = m_NormalPath;
        }

        if (path == null)
            return;

        //. Tween 적용
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(_pathName), 
            "time", 3.5f, 
            "isLocal", true, 
            "easetype", iTween.EaseType.easeInOutSine,
            "moveToPath", false,
            "looktarget", m_LookTarget));
    }

    public static void MoveToColorMode()
    {
        CameraDirection camProduction = Camera.main.GetComponent<CameraDirection>();
        camProduction.MoveToPath(R.String.COLOR_MODE_PATH);
    }

    public static void MoveToNoramlMode()
    {
        CameraDirection camProduction = Camera.main.GetComponent<CameraDirection>();
        camProduction.MoveToPath(R.String.NORMAL_MODE_PATH);
    }
}
