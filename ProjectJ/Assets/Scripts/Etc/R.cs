using UnityEngine;
using System.Collections;

/// <summary>
/// Player 상태 값
/// </summary>
public enum ePlayerState
{
    None,           ///< 아무 상태도 아님
    Run,            ///< 달리는 상태
    JumpStart,      ///< 점프 시작 상태
    Jumpping,       ///< 점프 중인 상태
    Land,           ///< 착지 상태
    DoubleJump,     ///< 2단 점프 상태
    LeftJump,       ///< 왼쪽 점프
    RightJump,      ///< 오른쪽 점프
}

/// <summary>
/// 플레이어 길 값
/// </summary>
public enum eLane
{
    Left,           ///< 왼쪽 길
    Middle,         ///< 가운데 길
    Right,          ///< 오른쪽 길
}

/// <summary>
/// Animation Event 코드값
/// </summary>
public enum eAnimationEvent
{
    AnimationEnd,   ///< 애니메이션 종료
    JumpStart,      ///< 점프 시작 이벤트
}

/// <summary>
///  Sns 타입
/// </summary>
public enum eSocialPlatform
{
    Facebook,       ///< 페이스북
}

/// <summary>
/// 전역 상수값들 선언
/// 내부 클래스는 카테고리처럼 사용
/// </summary>
public class R
{
    /// <summary>
    /// 일반 상수 값들
    /// </summary>
    public class Const
    {
        public static int INDEX_NONE = -1;
    }

    public class String
    {
        /// AnimTrigger 
        public static string ANIM_TRIGGER_RUN = "Run";                  ///< Run
        public static string ANIM_TRIGGER_JUMP_START = "JumpStart";     ///< JumpStart
        public static string ANIM_TRIGGER_JUPPING = "Jumpping";         ///< Jupping
        public static string ANIM_TRIGGER_LAND = "Land";                ///< Land
        public static string ANIM_TRIGGER_DOUBLE_JUMP = "DoubleJump";   ///< Double Jump
        public static string ANIM_TRIGGER_LEFT_JUMP = "LeftJump";       ///< Left Jump
        public static string ANIM_TRIGGER_RIGHT_JUMP = "RightJump";     ///< Right Jump

        /// Input 
        public static string INPUT_JUMP = "Jump";               ///< 점프
        public static string INPUT_LEFT_JUMP = "LeftJump";      ///< 왼쪽 점프
        public static string INPUT_RIGHT_JUMP = "RightJump";    ///< 오른쪽 점프

        /// Tag
        public static string TAG_OBSTACLE = "Obstacle";         ///< 장애물

        /// Scene
        public static string SCENE_LOGIN = "Login";             ///< 로그인
        public static string SCENE_LOBBY = "Lobby";             ///< 로비
        public static string SCENE_LV1 = "Lv1";                 ///< Lv1
    }
}
