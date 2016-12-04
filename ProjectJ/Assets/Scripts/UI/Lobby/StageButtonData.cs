using UnityEngine;
using System.Collections;

public class StageButtonData {
	private ushort m_Index;     ///< 스테이지 인덱스
    private bool m_Success;     ///< 성공했냐?

    public ushort GetIndex()
    {
        return m_Index;
    }

    public void SetIndex(int _index)
    {
        if(_index < 0)
        {
            Log.PrintError(eLogFilter.Normal, string.Format("Stage index is invalid (index:{0})", _index));
            return;
        }

        m_Index = (ushort)_index;
    }

    public bool IsSuccess()
    {
        return m_Success;
    }

    public void SetSuccess(bool _success)
    {
        m_Success = _success;
    }    
}
