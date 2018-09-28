using System; using UnityEngine; using VeerYeast; public enum ECorner {     LeftBottom,     RightBottom,     LeftTop,     RightTop,     RightMiddle,     LeftMiddle,     TopMiddle,     BottomMiddle,     Center  }    [Serializable ]
public class TargetAnchorConfig :MonoSingleton <TargetAnchorConfig >  {
  
    public    GameObject canvas ;     public    RectTransform originArchor ;     public   RectTransform targetArchor ;
}
