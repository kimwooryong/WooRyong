using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum eItemKeyColumns
{
    ID,
    Name,
    Description,
    CanCount
}
public class ItemManager : MonoBehaviour
{

    #region �̱���
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    private List<Dictionary<string, object>> data;

    private void Start()
    {
        data = CSVReader.Read("ItemDataBase");
    }

    public object ReadItemData(int itemID, eItemKeyColumns dataBaseKey)
    {
        return data[itemID][dataBaseKey.ToString()];
    }

    //�׽�Ʈ��
    [SerializeField]
    public TextMeshProUGUI text;
    private int count = 0;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) 
        {
            Debug.Log(("ID : " + data[count]["ID"] + "\t" +
           "Name : " + data[count]["Name"] + "\t" +
           "Description : " + data[count][eItemKeyColumns.Description.ToString()]));

            text.text = ("ID : " + data[count]["ID"] + "\t" +
           "Name : " + data[count]["Name"] + "\t" +
           "Description : " + data[count]["Description"]);
            count++;
            if (count > 6) count = 0;
        }
    }
}
