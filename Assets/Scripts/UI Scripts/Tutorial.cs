using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : OrderedMonoBehaviour
{
    public GameObject[] Panele = new GameObject [8];
    private int Page = 0;
    public Button NextPage;
    public Button BackPage;

    protected override int Order => 3;

    public void ChangePage(int Page)
    {
        Panele[0].SetActive(false);
        Panele[1].SetActive(false);
        Panele[2].SetActive(false);
        Panele[3].SetActive(false);
        Panele[4].SetActive(false);
        Panele[5].SetActive(false);
        Panele[6].SetActive(false);
        Panele[7].SetActive(false);
        Panele[Page].SetActive(true);

    }
    public void PageUp()
    {
        Page+=1;
        Debug.Log(Page);
        ChangePage(Page);
        if (Page > 0 && Page < Panele.Length-1)
        {
            BackPage.gameObject.SetActive(true);
        }
        else if (Page == Panele.Length-1 )
        {
            NextPage.gameObject.SetActive(false);
        }
    }
    public void PageDown()
    {
        Page-=1;
        ChangePage(Page);
        if (Page < Panele.Length-1 && Page > 0)
        {
            NextPage.gameObject.SetActive(true);
        }
        else if (Page == 0 )
        {
            BackPage.gameObject.SetActive(false);
        }
    }

    protected override void Initialize()
    {
        if (MainScript.LoadPath != null)
        {
            gameObject.SetActive(false);
        }
    }

    protected override void UpdateAction()
    {
    }
}
