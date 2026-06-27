using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class HelpMessaageController : MonoBehaviour
{
    public static HelpMessaageController instance;
    [SerializeField] public TMPro.TextMeshProUGUI textMeshPro;
    private Renderer targetRenderer;
    [SerializeField] private float upperOffset = 0.5f;
    [SerializeField] private float forwardOffset = 0.5f;

    public void Init()
    {
        instance = this;
    }

    public void Begin()
    {
        this.gameObject.SetActive(false);

    }

    public void ShowHelp(string message , GameObject target)
    {
        this.gameObject.SetActive( true );
        textMeshPro.text = message;
        targetRenderer = target.GetComponentInChildren<Renderer>();

    }

    public void HideHelp()
    {
        this.gameObject.SetActive( false );
        textMeshPro.text = "";
        targetRenderer = null;
    }

    void LateUpdate()
    {
        if (targetRenderer == null) return;

        // オブジェクトの中心を取得(中心からずれているのを考慮して底＋高さの中心に変更)
        Vector3 centerPos = targetRenderer.bounds.center;

        // 中心からカメラへの方向ベクトルを計算
        Vector3 dirToCamera = (Camera.main.transform.position - centerPos).normalized;

        // 少しずらした位置を計算
        transform.position = centerPos + (dirToCamera * forwardOffset) + (Vector3.up * upperOffset);

        // 常にカメラの方を向かせる（ビルボード）
        transform.rotation = Camera.main.transform.rotation;
    }
}
