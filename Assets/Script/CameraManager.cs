using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public void CAMOFF()
    {
        GameObject UnActiveCAMS = GameObject.Find("UnActiveCAMS");
        if (UnActiveCAMS != null)
        {
            for (int i = 0; i < UnActiveCAMS.transform.childCount; i++)
                UnActiveCAMS.transform.GetChild(i).gameObject.SetActive(false);
        }
        else
            Debug.Log("����Scene�� UnActiveCAMS�� ���� �� �ٸ� ī�޶� �����ϴ�.");
    }

    public void CAMON(string CAMname)
    {
        GameObject UnActiveCAMS = GameObject.Find("UnActiveCAMS");
        if (UnActiveCAMS != null)
        {
            for (int i = 0; i < UnActiveCAMS.transform.childCount; i++)
                if (UnActiveCAMS.transform.GetChild(i).name == CAMname)
                {
                    UnActiveCAMS.transform.GetChild(i).gameObject.SetActive(true);
                    break;
                }
        }
        else
            Debug.Log("����Scene�� UnActiveCAMS�� ���� ī�޶� ��ȯ���� �ʽ��ϴ�.");
    }
}
