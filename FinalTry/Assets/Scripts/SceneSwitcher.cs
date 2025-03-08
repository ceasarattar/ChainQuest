using UnityEngine;
using UnityEngine.SceneManagement;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;

public class SceneSwitcher : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to the OnLogin event when this script is enabled
        Web3.OnLogin += HandleLogin;
    }

    private void OnDisable()
    {
        // Unsubscribe when the script is disabled to avoid memory leaks
        Web3.OnLogin -= HandleLogin;
    }

    private void HandleLogin(Account account)
    {
        // This method is called when a successful login occurs
        Debug.Log($"Successfully logged in with account: {account.PublicKey}");
        
        // Switch to the desired scene
        SceneManager.LoadScene("Game");
    }
}