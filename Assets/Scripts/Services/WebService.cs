using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class WebService : MonoBehaviour
{
    private const string URL = "https://wadahub.manerai.com/api/inventory/status";
    private const string AuthToken = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";

    //Отправка данных на сервер
    public async void SendPostRequest(int id, ActionType actionType)
    {
        // Создание запроса
        using (var request = new UnityWebRequest(URL, "POST"))
        {
            // Устанавливака заголовков
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + AuthToken);

            // Добавление JSON данных в тело запроса
            JSONObject requestData = new JSONObject();
            requestData.Add("Id", id);
            requestData.Add("ActionType", actionType.ToString());
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestData.ToString());
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Отправка запроса и ожидание ответа
            await request.SendWebRequest();

            // Проверка результата
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

    //Тип события присоединения/отсоединения предмета
    public enum ActionType
    {
        Attached,
        Deattached
    }
}