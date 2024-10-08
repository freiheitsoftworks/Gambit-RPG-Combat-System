using Photon.Pun;
using TMPro;
using UnityEngine;

// Script responsável por trocar a cena de login (LoginScene) para a cena de gameplay (GameplayableScene)
// Classe roda no prefab (deve estar na pasta Resource - por conta do Photon Pun) player no subchild Canvas > Text (TMPRO)
public class UINames : MonoBehaviour
{
    private Transform transformComponent;
    private Vector3 offset = new Vector3(0, 180, 0);

    private TextMeshProUGUI textMeshComponent;
    void Start()
    {
        textMeshComponent = GetComponent<TextMeshProUGUI>();
        //Debug.Log("PhotonNetwork.LocalPlayer.NickName :" + PhotonNetwork.LocalPlayer.NickName);
        textMeshComponent.text = PhotonNetwork.LocalPlayer.NickName;
        transformComponent = GameObject.Find("Camera").GetComponent<Transform>(); 
    }

    void Update()
    {
        transform.LookAt(transformComponent);
        transform.Rotate(offset);
    }
}
