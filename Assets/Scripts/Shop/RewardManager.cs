using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private PuzzleGridManager puzzleManager;

    void Start()
    {
        if (puzzleManager != null)
        {
            puzzleManager.OnGiveReward += GiveReward;
        }
    } 

    void OnDestroy()
    {
        if (puzzleManager != null)
        {
            puzzleManager.OnGiveReward -= GiveReward;
        }
    }


    public void GiveReward(int baseReward)
    {
        int finalReward = baseReward;

        if (PlayerInventory.Instance.HasItem(ItemID.ExpertCertificate))
            finalReward = Mathf.RoundToInt(baseReward * 1.3f);

        Debug.Log($"Ganaste: ${finalReward}");
        GameManager.Instance.AddMoney(finalReward);
    }
}
