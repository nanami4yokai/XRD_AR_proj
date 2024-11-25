using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SimpleARImageTracking : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager arTrackedImageManager;
    [SerializeField] private bool testingMode = false; // Set to true for testing in Unity
    [SerializeField] private CritterCollectingManager collectingManager; // Reference to the collecting manager

    private void OnEnable()
    {
        // Subscribe to the event
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (testingMode)
        {
            return; // Skip AR functionality when testing in Unity
        }

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            ShowModelForImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                ShowModelForImage(trackedImage);
            }
            else
            {
                HideAllModels(); // Hide all models if tracking is lost
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            HideAllModels(); // Hide all models if the image is removed
        }
    }

    private void ShowModelForImage(ARTrackedImage trackedImage)
    {
        string recognizedCritterName = trackedImage.referenceImage.name;
        Debug.Log($"Tracked image recognized: {recognizedCritterName}");
        IdentifyCritter(recognizedCritterName);
        Debug.Log($"Reference Image Name: {trackedImage.referenceImage.name}");

    }

    private void IdentifyCritter(string imageName)
    {
        if (string.IsNullOrEmpty(imageName))
        {
            Debug.LogWarning("Image name is empty or null.");
            return;
        }

        Critter foundCritter = collectingManager.GetCritterByName(imageName);
        if (foundCritter != null)
        {
            Debug.Log($"Critter found: {foundCritter.critterName}");
            collectingManager.IdentifyCritter(foundCritter);
        }
        else
        {
            Debug.LogWarning($"Critter data not found for image name: {imageName}");
        }
    }

    private void HideAllModels()
    {
        collectingManager.DeactivateAllModels();
        Debug.Log("All models hidden.");
    }
}
