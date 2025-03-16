using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class WebService : MonoBehaviour
{
    private const string URL = "https://wadahub.manerai.com/api/inventory/status";
    private const string AuthToken = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    //�������� ������ �� ������
    public async void SendPostRequest(int id, ActionType actionType)
    {
        // �������� �������
        using (var request = new UnityWebRequest(URL, "POST"))
        {
            // ������������� ����������
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + AuthToken);

            // ���������� JSON ������ � ���� �������
            JSONObject requestData = new JSONObject();
            requestData.Add("Id", id);
            requestData.Add("ActionType", actionType.ToString());
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestData.ToString());
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // �������� ������� � �������� ������
            await request.SendWebRequest();

            // �������� ����������
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    //��� ������� �������������/������������ ��������
    public enum ActionType
    {
        Attached,
        Deattached
    }
}