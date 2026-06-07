using UnityEngine;
using MazeGame;
using System.Collections.Generic;
using static MazeGame.MazeGameConstants;
using UnityEngine.InputSystem;
using DG.Tweening;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private List<ItemBase> ItemList = new List<ItemBase>();
    [SerializeField] private float fadeDuaration = 1.5f;
    private MessageScrollManager messageScrollManager;
    private PlayerController playerController;
    private ItemInventory itemInventory;
    private ItemBase thisItem;
    private bool isGetItem;
    private int totalWeight;

    private void Start()
    {
        CalculateTotalWeight();
        GenerateItem();
        messageScrollManager = FindAnyObjectByType<MessageScrollManager>();
        playerController = FindAnyObjectByType<PlayerController>();
        itemInventory = FindAnyObjectByType<ItemInventory>();
    }

    private void CalculateTotalWeight()
    {
        totalWeight = 0;
        foreach (var item in ItemList)
        {
            totalWeight += item.weight;
        }
    }

    private void GenerateItem()
    {
        // アイテムBoxを開けるアニメーションとSEの再生

        float randomValue = Random.Range(0, totalWeight);

        // 3. どのアイテムの範囲に入ったか判定
        float currentWeight = 0;
        foreach (var item in ItemList)
        {
            currentWeight += item.weight;
            if (randomValue < currentWeight)
            {
                thisItem = item;
                break;
            }
        }
    }

    public void OnGetItem(InputAction.CallbackContext context)
    {
        if (isGetItem)
        {
            messageScrollManager.EnqueueMessage("中身が空だ");
            return;

        }
        bool isget = itemInventory.AddItem(thisItem);
        if (isget)
        {
            isGetItem = true;
            messageScrollManager.EnqueueMessage($"{thisItem.itemName}を入手した");
            // 宝箱を開けるを削除
            playerController.Actions.Player.AllPurpose.performed -= OnGetItem;

        }
        else
        {
            messageScrollManager.EnqueueMessage("上限を超えたので拾えなかった");
        }

        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.DOFade(0f, fadeDuaration).OnComplete(() => Destroy(gameObject));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(PlayerConstants.Tag))
        {
            // 宝箱を開けるを追加
            playerController.Actions.Player.AllPurpose.performed += OnGetItem;
            // ヘルプメッセージを表示
            HelpMessaageController.instance.ShowHelp("宝箱を開ける", this.gameObject);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(PlayerConstants.Tag))
        {
            // 宝箱を開けるを削除
            playerController.Actions.Player.AllPurpose.performed -= OnGetItem;
            // ヘルプメッセージを非表示
            HelpMessaageController.instance.HideHelp();
        }
    }

}
