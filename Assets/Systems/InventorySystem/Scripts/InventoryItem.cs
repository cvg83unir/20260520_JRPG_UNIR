
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [Header("UI Controls")]
    [SerializeField] Image image;
    [SerializeField] Button useButton;
    [SerializeField] Button discardButton;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI remainingUseCountText;

    Inventory inventory;
    InventoryInfo inventoryInfo;

    

    private void OnEnable()
    {
        useButton.onClick.AddListener(OnUse);
        discardButton.onClick.AddListener(OnDiscard);
    }

    private void OnDisable()
    {
        useButton.onClick.RemoveListener(OnUse);
        discardButton.onClick.RemoveListener(OnDiscard);
    }

    private void OnUse()
    {
        this.inventory.NotifyObjectUsed(this.inventoryInfo);
        this.inventoryInfo.remainingUseCount--;
        this.remainingUseCountText.text = this.inventoryInfo.remainingUseCount.ToString();

        if (this.inventoryInfo.remainingUseCount<=0)
        {
            Destroy(gameObject);
        }

    }

    private void OnDiscard()
    {
        Destroy(gameObject);
    }



    public void Initialize(Inventory inv, InventoryInfo invInfo)
    {
        //Instanciamos el Scriptable Object y lo metemos en la misma variable:
        invInfo = Instantiate(invInfo);

        this.inventory = inv; 
        this.inventoryInfo = invInfo;
        
        this.infoText.text = invInfo.infoText;
        this.remainingUseCountText.text = inventoryInfo.remainingUseCount.ToString();
        this.image.sprite = invInfo.spriteImage;

    }

    public void addUsage(int usages)
    {
        this.inventoryInfo.remainingUseCount+=usages;
        this.remainingUseCountText.text = this.inventoryInfo.remainingUseCount.ToString();
    }

    public bool compareItem(InventoryInfo ii)
    {
        return (ii.spriteImage == inventoryInfo.spriteImage) && (ii.infoText == inventoryInfo.infoText);
    }
}
