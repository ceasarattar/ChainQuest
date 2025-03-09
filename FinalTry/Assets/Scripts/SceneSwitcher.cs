using UnityEngine;
using UnityEngine.SceneManagement;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;
using System.Collections;

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
        SceneManager.LoadScene("Game");
    }

    private IEnumerator CheckLoginState()
    {
        while (true)
        {
            if (Web3.Wallet != null && Web3.Wallet.Account != null)
            {
                Debug.Log("Detected existing session, switching to Game scene...");
                SceneManager.LoadScene("Game");
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
