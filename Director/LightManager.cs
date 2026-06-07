using MazeGame;
using UnityEngine;
public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }
    private float LightVal;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        LightVal = MazeGameConstants.GameConstant.lightMinVal;
    }

    public void AdjustLightVal(float normalized)
    {
        // ê≥ãKâªÇ≥ÇÍÇΩÉfÅ[É^Çïœä∑Ç∑ÇÈ
        float newVal = (MazeGameConstants.GameConstant.lightMaxVal - MazeGameConstants.GameConstant.lightMinVal) * normalized + MazeGameConstants.GameConstant.lightMinVal;
        LightVal = newVal;
    }

    public float GetLightVal()
    {
        return LightVal;
    }

    public float GetNormalizedVal()
    {
        float normalizedVal = (LightVal - MazeGameConstants.GameConstant.lightMinVal) / (MazeGameConstants.GameConstant.lightMaxVal - MazeGameConstants.GameConstant.lightMinVal);
        return normalizedVal;
    }

    public void Initialize()
    {
        LightVal = MazeGameConstants.GameConstant.lightMinVal;
    }

}
