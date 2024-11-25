using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CritterCollector : MonoBehaviour
{
    public GameObject fadingCollection; // Reference to the Fading_Collection GameObject
    public Image bgImage;               // Reference to BG Image component
    public Image critterImage;          // Reference to Critter Image component

    public Image successImage; // AddToPedia_Success
    public Image errorImage;   // AddToPedia_Error
    public Image skipImage;    // Skip_Message

    private Critter currentCritter; // Store the current critter object
    public float fadeDuration = 1f; // Duration of fade animations

    private CritterCollectingManager collectingManager; // Reference to CritterCollectingManager

    private void Start()
    {
        // Ensure images and UI are inactive initially
        successImage.gameObject.SetActive(false);
        errorImage.gameObject.SetActive(false);
        skipImage.gameObject.SetActive(false);
        fadingCollection.SetActive(false);

        // Find the CritterCollectingManager
        collectingManager = FindObjectOfType<CritterCollectingManager>();
        if (collectingManager == null)
        {
            Debug.LogError("CritterCollectingManager instance not found!");
        }
    }

    public void ShowCritter(Critter critter)
    {
        if (collectingManager == null)
        {
            Debug.LogError("CritterCollectingManager is not assigned!");
            return;
        }

        currentCritter = critter;
        Debug.Log($"Current critter set: {currentCritter?.critterName}");

        if (fadingCollection != null)
        {
            fadingCollection.SetActive(true);
            Debug.Log("Fading_Collection activated.");
        }
        else
        {
            Debug.LogError("Fading_Collection is null!");
            return;
        }

        // Set images for the background and critter
        if (bgImage != null)
            bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0f);

        if (critter.animationSprite != null)
        {
            critterImage.sprite = critter.animationSprite;
            critterImage.color = new Color(critterImage.color.r, critterImage.color.g, critterImage.color.b, 0f);
            Debug.Log($"Critter image set to animation sprite: {critter.animationSprite.name}");
        }
        else
        {
            Debug.LogWarning($"Critter animationSprite is null for: {critter.critterName}");
        }

        StartCoroutine(FadeInImages());

        Animator animator = critterImage.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Spawn");
            Debug.Log("Spawn animation triggered.");
        }
        else
        {
            Debug.LogWarning("Animator not found on Critter GameObject!");
        }
    }

    public void OnAddToPediaButtonClicked()
    {
        Debug.Log($"OnAddToPediaButtonClicked called. Current critter: {currentCritter?.critterName}");

        if (currentCritter == null)
        {
            Debug.LogWarning("No critter selected to add to Pedia!");
            StartCoroutine(FadeOutImages(false, false));
            return;
        }

        Debug.Log($"Attempting to add critter: {currentCritter.critterName}");
        Debug.Log($"Critter instance in CritterCollector: {currentCritter.GetHashCode()}");

        bool success = collectingManager.AddToPedia(currentCritter);

        if (success)
        {
            Debug.Log($"Successfully added critter to Pedia: {currentCritter.critterName}");
            StartCoroutine(FadeOutImages(true, false));
        }
        else
        {
            Debug.LogWarning($"Failed to add critter to Pedia: {currentCritter.critterName}");
            StartCoroutine(FadeOutImages(false, false));
        }
    }

    public void Skip()
    {
        Debug.Log("Skip button clicked.");
        StartCoroutine(FadeOutImages(false, true));
    }

    private IEnumerator FadeInImages()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float normalizedTime = elapsed / fadeDuration;

            if (bgImage != null)
                bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, normalizedTime);

            if (critterImage != null)
                critterImage.color = new Color(critterImage.color.r, critterImage.color.g, critterImage.color.b, normalizedTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (bgImage != null)
            bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 1f);

        if (critterImage != null)
            critterImage.color = new Color(critterImage.color.r, critterImage.color.g, critterImage.color.b, 1f);

        Debug.Log("FadeInImages completed.");
    }

    private IEnumerator FadeOutImages(bool isSuccessful, bool isSkip = false)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float normalizedTime = 1 - (elapsed / fadeDuration);

            if (bgImage != null)
                bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, normalizedTime);

            if (critterImage != null)
                critterImage.color = new Color(critterImage.color.r, critterImage.color.g, critterImage.color.b, normalizedTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        fadingCollection.SetActive(false);

        if (isSkip)
        {
            skipImage.gameObject.SetActive(true);
            StartCoroutine(FadeImage(skipImage));
        }
        else if (isSuccessful)
        {
            successImage.gameObject.SetActive(true);
            StartCoroutine(FadeImage(successImage));
        }
        else
        {
            errorImage.gameObject.SetActive(true);
            StartCoroutine(FadeImage(errorImage));
        }
    }

    private IEnumerator FadeImage(Image image)
    {
        image.gameObject.SetActive(true);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float normalizedTime = elapsed / fadeDuration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, normalizedTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float normalizedTime = elapsed / fadeDuration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - normalizedTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        image.gameObject.SetActive(false);
    }
}
