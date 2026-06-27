using UnityEngine;
using System;
using MazeGame;
using System.Collections.Generic;
using UnityEngine.Events;

public class ItemInventory : MonoBehaviour
{
    [SerializeField] private List<ItemBase> ItemList = new List<ItemBase>();
    [SerializeField] private int InventorySize = 5;
    [SerializeField] private EmptyItem emptyItem;
    [SerializeField] private SoundData useSound;
    [SerializeField] private SoundData slideSound;
    [SerializeField] private SoundData getSound;
    private int CurrentIndex;


    [Header("-------------- イベント通知 --------------")]
    // UIから導線を引かないようにUnityEventで登録する
    [SerializeField] private UnityEvent<ItemBase> onChangeItem = new UnityEvent<ItemBase>();
    [SerializeField] private UnityEvent<ItemBase> onInitItem = new UnityEvent<ItemBase>();
    public UnityEvent<ItemBase> OnChangeItem => onChangeItem;
    public UnityEvent<ItemBase> OnInitItem => onInitItem; private PlayerController playerController;
    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;
    }
    public void Begin()
    {
        InitInventory();
    }

    private void InitInventory()
    {
        ItemList.Clear();
        ItemList.Add(emptyItem);
        CurrentIndex = 0;
        OnInitItem?.Invoke(emptyItem);
    }

    private void UpdateIndex()
    {
        if(ItemList.Count <= 1)
        {
            // アイテムインベントリの下限の場合は番兵で空のオブジェクトを指定する
            CurrentIndex = 0;
            return;
        }

        // アイテムインベントリの下限でない場合はリングバッファで動作
        if (CurrentIndex >= ItemList.Count)
        {
            CurrentIndex = 1;
        }
        else if (CurrentIndex < 1)
        {
            CurrentIndex = ItemList.Count-1;
        }
    }

    // プレイヤー操作をトリガーに実施
    public void NextItem()
    {
        if (CurrentIndex == 0) return;
        SoundManager.Instance.RequestSe(slideSound, this.transform.position);
        CurrentIndex++;
        UpdateIndex();
        OnChangeItem?.Invoke(ItemList[CurrentIndex]);
    }
    // プレイヤー操作をトリガーに実施
    public void BeforeItem()
    {
        if (CurrentIndex == 0) return;
        SoundManager.Instance.RequestSe(slideSound, this.transform.position,false);
        CurrentIndex--;
        UpdateIndex();
        OnChangeItem?.Invoke(ItemList[CurrentIndex]);
    }

    public void RemoveItem()
    {
        if (CurrentIndex <= 0 || CurrentIndex >= ItemList.Count) return;
        ItemBase itemBase = ItemList[CurrentIndex];
        ItemList.Remove(itemBase);
        UpdateIndex();
        OnChangeItem?.Invoke(ItemList[CurrentIndex]);
    }

    public void UseItem()
    {
        if (CurrentIndex == 0) return;
        ItemList[CurrentIndex].Use(playerController, MessageScrollManager.Instance);
        SoundManager.Instance.RequestSe(useSound, this.transform.position,false);
        RemoveItem();
        UpdateIndex();
        OnChangeItem?.Invoke(ItemList[CurrentIndex]);
    }


    // ItemBoxからのトリガーで実施
    public bool AddItem(ItemBase item)
    {
        if(InventorySize >= ItemList.Count )
        {
            ItemList.Add(item);
            if (ItemList.Count == 2)
            {
                // 番兵から移動追加された場合はIndexも移動する
                CurrentIndex = 1;
                OnChangeItem?.Invoke(ItemList[CurrentIndex]);
            }
            SoundManager.Instance.RequestSe(getSound, this.transform.position, false);
            return true;
        }
        else
        {
            return false;
        }
    }

    // インベントリ拡張
    public void AddItemInventory()
    {
        InventorySize++;
    }

}
