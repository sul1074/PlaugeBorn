using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    private Stat stat;
    private Player player;
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullheart;
    [SerializeField] private Sprite emptyheart;

    void Awake() {
        stat = player.GetComponent<Stat>();
    }

    void Update()
    {
        foreach (Image img in heartImages)
        {
            img.sprite = emptyheart;
        }
        for (int i = 0; i < stat.playerHealth && i < heartImages.Length; i++)
        {
            heartImages[i].sprite = fullheart;
        }
    }
}
