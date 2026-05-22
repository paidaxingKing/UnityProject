using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

[Serializable]
public class UI_ConnectDetails//只在个别两个类中使用，所以可以不另创文件，防止文件太多
{
    public UI_TreeConnectionHandler childNode;
    public NodeDirectionType direction;
    [Range(100f,350f)] public float length;
    [Range(-45f, 45f)] public float rotation;

}




public class UI_TreeConnectionHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();//rectangle矩形
    [SerializeField] private UI_ConnectDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionImage;
    private Color originalColor;

    private void Awake()
    {
        if (connectionImage != null)
        {
            originalColor = connectionImage.color;
        }
    }


    private void OnValidate()//提供了在不进入游戏时，能调用实例方法的途径
    {
        if (connectionDetails.Length == 0) return;
     
        if (connectionDetails.Length != connections.Length)
        {
            Debug.Log("边的数量和数据的数量不一致");
            return;
        }
        UpdateAllConnections();
    }

    public UI_TreeNode[] GetChildNodes()
    {
        List<UI_TreeNode> childrenToReturn = new List<UI_TreeNode>();

        foreach (var connection in connectionDetails)
        {
            if (connection.childNode != null)
            {
                childrenToReturn.Add(connection.childNode.GetComponent<UI_TreeNode>());
            }
        }

        return childrenToReturn.ToArray();

    }

    private void UpdateConnections()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            
            var connnection = connections[i];
            var detail = connectionDetails[i];
            connnection.DirectConnection(connectionDetails[i].direction, detail.length,detail.rotation);
            Image connectionImage = connnection.GetConnectionLineImage();

            Vector2 targetPosition = connnection.GetConnectionPoint(rect);//获得相对于锚点的坐标

            if (detail.childNode == null) continue;

            detail.childNode.SetPosition(targetPosition);//设置连接节点的位置
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();//图层置顶
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnections();

        foreach(var node in connectionDetails)
        {
            if (node.childNode == null) continue;
            node.childNode.UpdateAllConnections();
        }
    }

    public void UnlockConnectionImage(bool unlock)
    {
        if (connectionImage == null)
        {
            return;
        }

        connectionImage.color = unlock ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image image)
    {
        connectionImage = image;
    }

    public void SetPosition(Vector2 position)
    {
        rect.anchoredPosition = position;
        /*
        相当于锚点位置的坐标，而技能树节点都在同一层级下，即都在同一层，所以技能树节点的相对锚点坐标是相同的
        实际上应该设置localPosition,但是localPosition和锚点坐标重合，所以都可以
        */
    }

}



