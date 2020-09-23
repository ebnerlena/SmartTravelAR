using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapPlayerMarker : MonoBehaviour
{
    public Image image;
    public Text text;
    public int shortNameLength;

    public Vector3 start, target;
    private bool isMoving = false;
    private Sprite originalSprite;
    
    public void Setup(string playerName, Avatar playerAvatar)
    {
        GameManager.Instance.ExecuteOnMain(() => SetupAction(playerName, playerAvatar));
    }

    public void StartMoving(Vector2 start, Vector2 target, float inSeconds, Sprite travelSprite = null)
    {
        this.start = start;
        this.target = target;
        if(!isMoving)
        {
            StartCoroutine(MoveToTarget(inSeconds, travelSprite));
        }
    }

    private IEnumerator MoveToTarget(float inSeconds, Sprite travelSprite)
    {
        isMoving = true;
        ChangeSprite(travelSprite);

        float progress = 0f;
        Vector2 posProg = start;

        while((transform.localPosition - target).magnitude > 0.03)
        {
            posProg.x = Mathf.Lerp(start.x, target.x, progress);
            posProg.y = Mathf.Lerp(start.y, target.y, progress);

            transform.localPosition = posProg;

            yield return new WaitForFixedUpdate();
            progress += Time.fixedDeltaTime / inSeconds;
        }
        FinishMovement();
    }

    private void FinishMovement()
    {
        transform.localPosition = target;

        ChangeSprite(originalSprite);
        isMoving = false;
    }

    private void ChangeSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            GameManager.Instance.ExecuteOnMain(() => image.sprite = sprite);
        }
    }

    private void SetupAction(string playerName, Avatar playerAvatar)
    {
        SetupAction(playerAvatar.Icon, playerName);
    }

    private void SetupAction(Sprite icon, string name)
    {
        if (image != null && icon != null)
        {
            image.sprite = icon;
            originalSprite = icon;
        }
        if (text != null)
            text.text = name.Substring(0, Mathf.Max(1, shortNameLength)).ToUpper();
    }

    private void OnDisable()
    {
        if(isMoving)
            FinishMovement();
    }
}