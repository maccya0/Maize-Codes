using MazeGame;
using UnityEngine;
public class LightManager : BaseManager<LightManager>
{
    private float LightVal;

    protected override void Awake()
    {
        base.Awake();
        if(Instance != this) return;
    }

    public override void ManagerStart()
    {
        base.ManagerStart();
        Initialize();
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
