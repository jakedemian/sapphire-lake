using UnityEngine;

public class RandomSpriteGrid : MonoBehaviour {
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private int spritePixelsPerUnit;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GenerateRandomSpriteGridTexture();
    }

    private void GenerateRandomSpriteGridTexture() {
        int textureWidth = columns * spritePixelsPerUnit;
        int textureHeight = rows * spritePixelsPerUnit;
        Texture2D combinedTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
        combinedTexture.filterMode = FilterMode.Point;

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                int randomSpriteIndex = Random.Range(0, sprites.Length);
                Sprite sprite = sprites[randomSpriteIndex];

                Texture2D spriteTexture = sprite.texture;
                Rect spriteRect = sprite.rect;

                int xPos = j * spritePixelsPerUnit;
                int yPos = i * spritePixelsPerUnit;

                Color[] spritePixels = spriteTexture.GetPixels((int)spriteRect.x, (int)spriteRect.y, spritePixelsPerUnit, spritePixelsPerUnit);
                spritePixels = RotateSpritePixels(spritePixels, spritePixelsPerUnit);

                combinedTexture.SetPixels(xPos, yPos, spritePixelsPerUnit, spritePixelsPerUnit, spritePixels);
            }
        }

        combinedTexture.Apply();
        Rect combinedSpriteRect = new Rect(0, 0, textureWidth, textureHeight);
        spriteRenderer.sprite = Sprite.Create(combinedTexture, combinedSpriteRect, new Vector2(0.5f, 0.5f), spritePixelsPerUnit);
    }

    private Color[] RotateSpritePixels(Color[] pixels, int resolution) {
        int rotationSteps = Random.Range(0, 4);
        Color[] rotatedPixels = new Color[pixels.Length];

        for (int step = 0; step < rotationSteps; step++) {
            for (int x = 0; x < resolution; x++) {
                for (int y = 0; y < resolution; y++) {
                    int rotatedX = resolution - 1 - y;
                    int rotatedY = x;
                    rotatedPixels[rotatedY * resolution + rotatedX] = pixels[y * resolution + x];
                }
            }
            pixels = (Color[])rotatedPixels.Clone();
            System.Array.Clear(rotatedPixels, 0, rotatedPixels.Length);
        }

        return pixels;
    }
}