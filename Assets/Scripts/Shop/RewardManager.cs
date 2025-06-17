using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public int baseReward = 100;

    public void GiveReward()
    {
        int finalReward = baseReward;

        if (PlayerInventory.Instance.HasItem(ItemID.ExpertCertificate))
            finalReward = Mathf.RoundToInt(baseReward * 1.3f);

        Debug.Log($"Ganaste: ${finalReward}");
        GameManager.Instance.AddMoney(finalReward);
    }
}
