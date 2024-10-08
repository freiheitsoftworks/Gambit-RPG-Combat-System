using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Photon.Pun;

// Script responsável por logar jogador no servidor photon cloud - guest login funcionando
// usado no GameObject LoginManager

public class LoginManager : MonoBehaviour
{
    public async void LogInAsGuest()
    {
        await SignInAnonymouslyAsync();
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Obter o PlayerID
            string playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"PlayerID: {playerId}");

            // Armazenar o PlayerID no Photon
            PhotonNetwork.LocalPlayer.NickName = playerId;

            if (playerId != null)
            {
                FindObjectOfType<SceneLoader>().LoadScene(1); // Carrega a cena após o login
            }
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
}
