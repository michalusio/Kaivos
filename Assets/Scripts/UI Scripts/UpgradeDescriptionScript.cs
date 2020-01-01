using UnityEngine;
using UnityEngine.UI;

public class UpgradeDescriptionScript : MonoBehaviour
{
    public Text Show;

    public void ShowMineS()
    {
        Show.text = " Szybkość kopania ";
    }

    public void ShowMineA()
    {
        Show.text = " Wielkość kopanego obszaru ";
    }

    public void ShowMachineS()
    {
        Show.text = " Szybkość pracy maszyn ";
    }

    public void ShowJetpack()
    {
        Show.text = " Pojemność jetpacka";
    }

    public void ShowTeleporter()
    {
        Show.text = " Odblokowannie teleportera - Checkpointu";
    }

    public void Clear()
    {
        Show.text = " ";
    }
}
