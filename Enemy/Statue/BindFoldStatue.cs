using MazeGame;
using UnityEngine;
using System.Collections;

public class BindFoldStatue : Statue
{
    [SerializeField] private float maxFogDensity = 0.05f; // –¶‚ÌÅ‘å”Z“x
    [SerializeField] private float fadeDuration = 2.0f;   // –¶‚ª”Z‚­‚È‚é/”–‚­‚È‚é‚Ü‚Å‚ÌŠÔ
    [SerializeField] private float duration = 5.0f;   // ‘±ŠÔ
    [SerializeField] private MessageScrollManager messageScrollManager;

    protected override void ExecuteStatueSkill(GameObject gameObject)
    {
        StartCoroutine(BindFoldExecute());
    }

    private IEnumerator BindFoldExecute()
    {
        // fog‚ğ—LŒø‚É‚µ‚Ä™X‚É”Z‚­‚·‚é
        messageScrollManager.EnqueueMessage("–¶‚ª”Z‚­‚È‚Á‚Ä‚«‚½");
        RenderSettings.fog = true;
        float startDensity = RenderSettings.fogDensity;
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, (startDensity + maxFogDensity), elapsed / fadeDuration);
            yield return null;
        }

        // ˆê’èŠÔŒp‘±
        yield return new WaitForSeconds(duration);


        // ™X‚É–ß‚µ‚Ä‚¢‚­
        messageScrollManager.EnqueueMessage("–¶‚ª”–‚­‚È‚Á‚½");
        elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(startDensity, (startDensity + maxFogDensity), elapsed / fadeDuration);
            yield return null;
        }
    }
}
