using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public PlayerMovement player;
    public float maxStamina = 100f;
    private Vector3 targetScale;
    public Image fillImage;

    void Update()
    {

        float barLength = Mathf.Clamp01(player.stamina / maxStamina);


        targetScale = new Vector3(barLength * 4f, 0.1f, 1f);

        
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 10f);

        if (player.isExhausted)
        {
            fillImage.color = Color.red;
        }
        else
        {
            fillImage.color = Color.white;
        }


    }
}
