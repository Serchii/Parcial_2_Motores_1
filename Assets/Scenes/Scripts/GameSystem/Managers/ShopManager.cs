using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int watchPrice = 100;
    public int expertCertificatePrice = 200;

    public void BuyWatch()
    {
        if (PlayerMoney.Instance.SpendMoney(watchPrice))
        {
            PlayerInventory.Instance.BuyWatch();
        }
        else
        {
            Debug.Log("Dinero insuficiente para el reloj.");
        }
    }

    public void BuyExpertCertificate()
    {
        if (PlayerMoney.Instance.SpendMoney(expertCertificatePrice))
        {
            PlayerInventory.Instance.BuyExpertCertificate();
        }
        else
        {
            Debug.Log("Dinero insuficiente para el certificado.");
        }
    }
}
