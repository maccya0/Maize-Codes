using DG.Tweening;
using MazeGame;
using UnityEngine;
using static UIButtonInterFace;

public class RingMenuController : MonoBehaviour
{
    [SerializeField] private GameObject[] ButtonList;
    private CanvasGroup[] buttonGroups;
    [SerializeField] private MenuController MenuController;
    [SerializeField] private SoundData slideSound;

    private SoundManager soundManager;
    private int CurrentIndex = 0;
    private Vector3 CenterPos;
    private Vector3 RightPos;
    private Vector3 LeftPos;
    private const int CenterIndex = 0;
    private const int RightIndex = 1;
    private const int LeftIndex = 2;


    private void Awake()
    {
        //  InputSystem_Actionsを設定するデリゲートを設定する
        buttonGroups = new CanvasGroup[ButtonList.Length];
        for (int i = 0; i < ButtonList.Length; i++)
        {
            buttonGroups[i] = ButtonList[i].GetComponent<CanvasGroup>();
        }

        CenterPos = ButtonList[CenterIndex].transform.position;
        RightPos = ButtonList[RightIndex].transform.position;
        LeftPos = ButtonList[LeftIndex].transform.position;
        Initalize();
    }

    private void Start()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(ButtonList[0]);
        soundManager = SoundManager.Instance;
        // 初期状態の見た目を反映
        UpdateButtonView(true);
        soundManager.CanPlaySound = true;
    }

    private void OnDestroy()
    {
        ButtonList[CurrentIndex].transform.DOKill();

        for (int i = 0; i < ButtonList.Length; i++)
        {
            ButtonList[i].transform.DOKill();
            buttonGroups[i].DOKill();
        }
 
    }

    private void Initalize()
    {
        CurrentIndex = 0;
    }
    private void Update()
    {
        // EventSystemが現在選択しているオブジェクトを取得
        GameObject selected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        // もし選択中のオブジェクトが自分の管理下（ButtonListのどれか）なら
        for (int i = 0; i < ButtonList.Length; i++)
        {
            if (ButtonList[i] == selected && CurrentIndex != i)
            {
                // 選ばれているインデックスに合わせてメニューを回転
                CurrentIndex = i;
                UpdateButtonView();
            }
        }
    }

    private void UpdateButtonView(bool immediate = false)
    {
        float duration = immediate ? 0f : 0.6f;

        soundManager.RequestSe(slideSound, this.transform.position, false);

        // 2. すべてのボタンに対して「お前は今どの席に座るべきか」を命令する
        for (int i = 0; i < ButtonList.Length; i++)
        {
            Vector3 targetPos;
            float targetAlpha = 0.5f;
            float targetScale = 0.8f;
            ButtonState state = ButtonState.NotSelected;

            // ボタン i が今の CurrentIndex に対してどの相対位置にいるか
            if (i == CurrentIndex)
            {
                targetPos = CenterPos;
                targetAlpha = 1.0f;
                targetScale = 1.5f;
                state = ButtonState.Selected;
            }
            else if (i == (CurrentIndex + 1) % ButtonList.Length)
            {
                targetPos = RightPos;
                targetAlpha = 0.5f;
                targetScale = 0.5f;
                state = ButtonState.NotSelected;
            }
            else if (i == (CurrentIndex - 1 + ButtonList.Length) % ButtonList.Length)
            {
                targetPos = LeftPos;
                targetAlpha = 0.5f;
                targetScale = 0.5f;
                state = ButtonState.NotSelected;
            }
            else
            {
                targetPos = CenterPos;
                targetPos.z = 0.0f;
                targetAlpha = 0f;
                state = ButtonState.UnVisible;
            }

            // 3. 決定した座標へ移動・変形
            ButtonList[i].transform.DOMove(targetPos, duration).SetEase(Ease.OutBack);
            buttonGroups[i].DOFade(targetAlpha, duration);
            ButtonList[i].transform.DOScale(Vector3.one * targetScale, duration);

            // インターフェースへの通知
            ButtonList[i].GetComponent<UIButtonInterFace>().SetState(state);
        }
    }

    public GameObject GetCurrentButton()
    {
        return ButtonList[CurrentIndex];
    }

}
