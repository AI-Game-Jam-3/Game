using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public Sprite afterCollectedSprite;

    private SpriteRenderer spriteRenderer;
    public bool triggered = false;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!triggered && other.TryGetComponent<SmallTrigger>(out var comp))
        {
            spriteRenderer.sprite = afterCollectedSprite;
            UIManager.Instance.achievementSystem.SetAchievementStatus("Collect1", true);
            triggered = true;
        }
    }
}
