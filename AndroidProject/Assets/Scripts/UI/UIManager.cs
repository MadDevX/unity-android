using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Vector2 nickLabelOffset;

    public UIPanel menuPanel;
    public JoinPanel joinPanel;
    public UIPanel gamePanel;
    public UIPanel gameListPanel;
    public LoadingPanelTracker loadingTracker;

    public Vector2 relativeUIOffset(Vector2 offset)
    {
        return new Vector2(offset.x * Screen.width, offset.y * Screen.height);
    }
}
