using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Script responsável por trocar a cena de login (LoginScene) para a cena de gameplay (GameplayableScene)
// Classe roda GameObject SceneLoader

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreenPrefab;  // Prefab da tela de carregamento
    public GameObject sliderPrefab;  // Prefab da barra de progresso (Slider)

    private GameObject loadingScreenInstance;  // Instância da tela de carregamento
    private Slider progressBar;  // Referência ao componente Slider
    public float minimumLoadingTime = 1.5f;  // Tempo mínimo para mostrar a tela de carregamento

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        // Instanciar a tela de carregamento e o slider
        loadingScreenInstance = Instantiate(loadingScreenPrefab);
        GameObject sliderInstance = Instantiate(sliderPrefab, loadingScreenInstance.transform);

        // Pegar o componente Slider da instância recém-criada
        progressBar = sliderInstance.GetComponent<Slider>();

        // Iniciar o carregamento da cena de forma assíncrona
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;  // Não ativar a cena até o carregamento terminar

        // Registrar o tempo de início
        float startTime = Time.time;

        // Atualizar a barra de progresso enquanto a cena carrega
        while (!operation.isDone)
        {
            // O progresso vai até 0.9, então ajustamos manualmente
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Atualizar o valor da barra de progresso
            if (progressBar != null)
                progressBar.value = progress;

            // Quando o carregamento atingir 90% (0.9), ativar a cena
            if (operation.progress >= 0.9f)
            {
                // Completa o slider visualmente
                progressBar.value = 1f;

                // Aguardar o tempo mínimo antes de ativar a cena
                if (Time.time - startTime >= minimumLoadingTime)
                {
                    operation.allowSceneActivation = true; // Ativar a cena
                }
            }

            yield return null; // Esperar o próximo frame
        }

        // Aguardar um frame para garantir que a nova cena foi ativada
        yield return new WaitForEndOfFrame();

        // Destruir a tela de carregamento após o carregamento da cena
        Destroy(loadingScreenInstance);
    }
}
