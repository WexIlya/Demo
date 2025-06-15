using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Text;

public class APIChat : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField userInputField;
    public Button sendButton;
    public TMP_Text aiResponseText;
    public TMP_Text statusText;

    [Header("API Settings")]
    public string apiEndpoint = "https://gigachat.devices.sberbank.ru/api/v1/chat/completions";
    [SerializeField] string authToken;
    public string model = "GigaChat:latest";
    string clientId = "";
    string clientSecret = "";
    string scope = "GIGACHAT_API_PERS";
    string authUrl = "https://ngw.devices.sberbank.ru:9443/api/v2/oauth";

    [Header("Debug Settings")]
    public bool showFullJson = true;
    public float requestTimeout = 30f;
    [Header("Role Settings")]
    public string defaultRoleContext = "";

    void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback += MyRemoteCertificateValidationCallback;

        sendButton.onClick.AddListener(SendMessageToAI);
        userInputField.onSubmit.AddListener(delegate { SendMessageToAI(); });

        StartCoroutine(GetToken());
    }

    IEnumerator GetToken()
    {
        string requestBody = $"scope={scope}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);

        UnityWebRequest request = new UnityWebRequest(authUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("RqUID", System.Guid.NewGuid().ToString());
        request.SetRequestHeader("Authorization", $"Basic {GetBase64Auth(clientId, clientSecret)}");
        request.certificateHandler = new BypassCertificateHandler();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);

            var jsonResponse = JsonUtility.FromJson<TokenResponse>(request.downloadHandler.text);
            authToken = jsonResponse.access_token;
            Debug.Log($"Access Token: {authToken}");
        }
        else
        {
            Debug.LogError($" {request.error}");
            Debug.LogError($" {request.downloadHandler.text}");
        }
        yield return new WaitForSeconds(1800f);
    }

    public void SendMessageToAI()
    {
        if (string.IsNullOrEmpty(userInputField.text))
        {
            UpdateStatus("Введите сообщение!");
            Debug.LogWarning("Попытка отправить пустое сообщение");
            return;
        }

        Debug.Log($"Начало отправки запроса: {userInputField.text}");
        StartCoroutine(SendChatRequest(userInputField.text));
    }

    IEnumerator SendChatRequest(string userMessage)
    {
        SetUIInteractable(false);
        UpdateStatus("Подготовка запроса...");
        aiResponseText.text = "Думаю...";

        var requestBody = new RequestBody
        {
            model = this.model,
            messages = new List<Message> {
                new Message {
                role = "system",
                content = defaultRoleContext
                },
                new Message {
                    role = "user",
                    content = userMessage
                }
            },
            temperature = 0.7f,
            max_tokens = 1024
        };

        string jsonBody = JsonUtility.ToJson(requestBody);
        byte[] rawBody = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        if (showFullJson)
        {
            Debug.Log($"{jsonBody}");
        }

        using (UnityWebRequest request = new UnityWebRequest(apiEndpoint, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(rawBody);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {authToken}");
            request.SetRequestHeader("Accept", "application/json");
            request.certificateHandler = new BypassCertificateHandler();

            float startTime = Time.time;
            bool timeoutReached = false;

            var operation = request.SendWebRequest();
            UpdateStatus("Отправка запроса...");

            while (!operation.isDone)
            {
                if (Time.time - startTime > requestTimeout)
                {
                    timeoutReached = true;
                    request.Abort();
                    break;
                }

                UpdateStatus($"Отправка... {Mathf.RoundToInt(operation.progress * 100)}%");
                yield return null;
            }

            if (timeoutReached)
            {
                Debug.LogError($"Таймаут запроса ({requestTimeout} ñåê)");
                UpdateStatus("Таймаут соединения");
                aiResponseText.text = "Сервер не ответил";
                yield break;
            }

            Debug.Log($"Запрос завершен за {Time.time - startTime:F2} сек");
            Debug.Log($"HTTP статус: {request.responseCode}");
            Debug.Log($"Заголовки ответа: {request.GetResponseHeaders()?.ToString() ?? "íåò"}");

            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError($"Ошибка запроса: {request.error}");
                Debug.LogError($"Полный ответ: {request.downloadHandler?.text ?? "íåò äàííûõ"}");
            }
            else if (showFullJson)
            {
                Debug.Log($"Полный ответ: {request.downloadHandler?.text}");
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                ProcessAIResponse(request.downloadHandler.text);
                UpdateStatus("Ответ получен");
            }
            else
            {
                string errorMessage = ParseError(request);
                Debug.LogError($"Ошибка: {errorMessage}");
                aiResponseText.text = errorMessage;
                UpdateStatus("Ошибка запроса");
            }
        }

        SetUIInteractable(true);
    }

    private string ParseError(UnityWebRequest request)
    {
        if (request.responseCode == 401) return "Ошибка 401: Неверный API-ключ";
        if (request.responseCode == 403) return "Ошибка 403: Доступ запрещен";
        if (request.responseCode == 429) return "Ошибка 429: Слишком много запросов";
        if (request.responseCode >= 500) return $"Ошибка {request.responseCode}: Проблемы на сервере";

        try
        {
            var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
            return errorResponse?.error?.message ?? request.error;
        }
        catch
        {
            return request.error ?? "Неизвестная ошибка";
        }
    }

    void ProcessAIResponse(string jsonResponse)
    {
        try
        {
            var response = JsonUtility.FromJson<GigaResponse>(jsonResponse);

            if (response.choices != null && response.choices.Count > 0)
            {
                string aiMessage = response.choices[0].message.content;
                aiResponseText.text = aiMessage;
                Debug.Log("Успешно получен ответ от ИИ");
            }
            else
            {
                aiResponseText.text = "Пустой ответ от ИИ";
                Debug.LogWarning("Получен пустой ответ от API");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка парсинга JSON: {e.Message}\nОтвет сервера: {jsonResponse}");
            aiResponseText.text = "шибка обработки ответа";
        }
    }

    void UpdateStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    void SetUIInteractable(bool interactable)
    {
        sendButton.interactable = interactable;
        userInputField.interactable = interactable;
    }

    private static bool MyRemoteCertificateValidationCallback(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        Debug.Log($"SSL: {certificate?.Subject ?? "нет сертификата"}, ошибка: {sslPolicyErrors}");
        return true;
    }
    private string GetBase64Auth(string id, string secret)
    {
        string authString = $"{id}:{secret}";
        byte[] authBytes = Encoding.UTF8.GetBytes(authString);
        return System.Convert.ToBase64String(authBytes);
    }

    private class BypassCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            Debug.Log("Bypass сертификата");
            return true;
        }
    }

    [System.Serializable]
    private class RequestBody
    {
        public string model;
        public List<Message> messages;
        public float temperature;
        public int max_tokens;
    }

    [System.Serializable]
    public class TokenResponse
    {
        public string access_token;
        public int expires_in;
    }

[System.Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class Choice
    {
        public Message message;
    }

    [System.Serializable]
    private class GigaResponse
    {
        public List<Choice> choices;
    }

    [System.Serializable]
    private class ErrorResponse
    {
        public ErrorInfo error;
    }

    [System.Serializable]
    private class ErrorInfo
    {
        public string message;
        public string type;
    }
}
