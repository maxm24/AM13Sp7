using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.CompareTag("Enemy"))
        {
            game
        }
    }
}
