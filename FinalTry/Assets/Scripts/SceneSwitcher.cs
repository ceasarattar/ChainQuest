using UnityEngine;
using UnityEngine.SceneManagement;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;

public class SceneSwitcher : MonoBehaviour
{
    private void OnEnable()
    {
        Web3.OnLogin += HandleLogin;
        StartCoroutine(CheckLoginState());
    }

    private void OnDisable()
    {
        Web3.OnLogin -= HandleLogin;
        StopCoroutine(CheckLoginState());
    }

    private void HandleLogin(Account account)
    {
        Debug.Log($"Successfully logged in: {account.PublicKey}");
        // Reset map to ensure proper initialization in the new scene
        if (LocationProviderFactory.Instance.mapManager != null)
        {
            LocationProviderFactory.Instance.mapManager.ResetMap();
            Debug.Log("Map reset before scene switch.");
        }
        else
        {
            Debug.LogWarning("MapManager not found. Map may not initialize correctly.");
        }
        SceneManager.LoadScene("Game");
    }

    private IEnumerator CheckLoginState()
    {
        while (true)
        {
            if (Web3.Wallet != null && Web3.Wallet.Account != null)
            {
                Debug.Log("Detected existing session, switching to Game scene...");
                if (LocationProviderFactory.Instance.mapManager != null)
                {
                    LocationProviderFactory.Instance.mapManager.ResetMap();
                    Debug.Log("Map reset before scene switch.");
                }
                else
                {
                    Debug.LogWarning("MapManager not found. Map may not initialize correctly.");
                }
                SceneManager.LoadScene("Game");
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}