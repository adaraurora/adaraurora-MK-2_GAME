using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour
{
    [Header("Card Data")]
    public int id;

    [Header("Optional - boleh dikosongkan")]
    public Image icon;

    private GameManager gameManager;
    private Image cardImage;
    private Button button;

    private Sprite backSprite;
    private Sprite frontSprite;

    private bool opened;
    private bool matched;
    private bool busy;

    private void Awake()
    {
        cardImage = GetComponent<Image>();
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OpenCard);
        }

        if (icon != null)
            icon.gameObject.SetActive(false);
    }

    public void Init(GameManager manager, int cardId, Sprite sprite)
    {
        gameManager = manager;
        id = cardId;
        frontSprite = sprite;
        opened = false;
        matched = false;
        busy = false;

        if (cardImage == null) cardImage = GetComponent<Image>();
        if (button == null) button = GetComponent<Button>();

        // Ambil gambar belakang langsung dari Source Image prefab Card.
        // Jadi prefab Card WAJIB pakai gambar back card sebagai Source Image awal.
        if (backSprite == null && cardImage != null)
            backSprite = cardImage.sprite;

        ShowBackInstant();
    }

    public void OnClick()
    {
        OpenCard();
    }

    public void OpenCard()
    {
        if (opened || matched || busy) return;
        if (gameManager != null && gameManager.IsChecking) return;

        opened = true;
        StartCoroutine(FlipToFront());
    }

    public void CloseCard()
    {
        if (matched || busy) return;
        opened = false;
        StartCoroutine(FlipToBack());
    }

    public void SetMatched()
    {
        matched = true;
        opened = true;

        if (button != null)
            button.interactable = false;

        ShowFrontInstant();
    }

    private IEnumerator FlipToFront()
    {
        busy = true;
        yield return ScaleX(1f, 0f, 0.10f);
        ShowFrontInstant();
        yield return ScaleX(0f, 1f, 0.10f);
        busy = false;

        if (gameManager != null)
            gameManager.SelectCard(this);
    }

    private IEnumerator FlipToBack()
    {
        busy = true;
        yield return ScaleX(1f, 0f, 0.10f);
        ShowBackInstant();
        yield return ScaleX(0f, 1f, 0.10f);
        busy = false;
    }

    private IEnumerator ScaleX(float from, float to, float duration)
    {
        float timer = 0f;
        Vector3 start = transform.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float x = Mathf.Lerp(from, to, timer / duration);
            transform.localScale = new Vector3(x, start.y, start.z);
            yield return null;
        }

        transform.localScale = new Vector3(to, start.y, start.z);
    }

    private void ShowFrontInstant()
    {
        if (icon != null)
            icon.gameObject.SetActive(false);

        if (frontSprite == null)
        {
            Debug.LogWarning("Front sprite kosong. Isi Card Sprites di GameManager.");
            return;
        }

        if (cardImage != null)
        {
            cardImage.sprite = frontSprite;
            cardImage.enabled = true;
            cardImage.color = Color.white;
            cardImage.preserveAspect = true;
        }
    }

    private void ShowBackInstant()
    {
        if (icon != null)
            icon.gameObject.SetActive(false);

        if (button != null)
            button.interactable = true;

        if (cardImage != null)
        {
            if (backSprite != null)
                cardImage.sprite = backSprite;

            cardImage.enabled = true;
            cardImage.color = Color.white;
            cardImage.preserveAspect = true;
        }

        transform.localScale = Vector3.one;
    }
}
