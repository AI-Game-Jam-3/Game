using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public Sprite afterCollectedSprite;

    private SpriteRenderer spriteRenderer;
    public bool triggered = false;

    public Texture ground_tex;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!triggered && other.TryGetComponent<SmallTrigger>(out var comp))
        {
            // spriteRenderer.sprite = afterCollectedSprite;
            
            Color color = Color.white;
            color.a = 0;
            spriteRenderer.material.SetTexture("_BaseMap", /*afterCollectedSprite.texture*/ground_tex);
            spriteRenderer.material.SetColor("_BaseColor", color);
            //UIManager.Instance.achievementSystem.SetAchievementStatus("Collect1", true);
            triggered = true;
        }
    }
}
