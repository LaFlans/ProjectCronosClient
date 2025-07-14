using MasterMemory;
using MessagePack;

/// <summary>
/// ショップで売られているアイテム情報
/// 同じグループIDのアイテムが一覧として売られる想定
/// </summary>
[MemoryTable("m_shop_item"), MessagePackObject(true)]
public class ShopItemData
{
    [PrimaryKey]
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int ItemId { get; set; }

    /// <summary>
    /// 購入制限
    /// 0の場合、無制限に買える
    /// 0出ない場合、設定された数字がゲーム中に購入できる数
    /// </summary>
    public int PurchaseLimit { get; set; }

    public ShopItemData(int Id, int GroupId, int ItemId, int PurchaseLimit)
    {
        this.Id = Id;
        this.GroupId = GroupId;
        this.ItemId = ItemId;
        this.PurchaseLimit = PurchaseLimit;
    }
}
