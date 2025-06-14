using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int watchPrice = 100;
    public int expertCertificatePrice = 200;

    public void BuyWatch()
    {
        if (GameManager.Instance.SpendMoney(watchPrice))
        {
            GameManager.Instance.AddItem("Watch");
            UIManager.Instance.ShowClockUI(true); // opcional
        }
        else
        {
            Debug.Log("Dinero insuficiente para el reloj.");
        }
    }

    public void BuyExpertCertificate()
    {
        if (GameManager.Instance.SpendMoney(expertCertificatePrice))
        {
            GameManager.Instance.AddItem("ExpertCertificate");
        }
        else
        {
            Debug.Log("Dinero insuficiente para el certificado.");
        }
    }
}
