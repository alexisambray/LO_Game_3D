using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ObjectPool;

public class ImageManager : MonoBehaviour
{
    [System.Serializable]
    public class TestResult
    {
        [Header("Mixture Attributes")]
        [SerializeField] public Appearance appearance;
        [SerializeField] public Uses uses;
        [SerializeField] public WorkTools workTools;
        public Sprite resultImage;
        public string resultText;
    }

    [SerializeField] public List<TestResult> testResults; // Assign this in the inspector



    public Sprite GetImage(Appearance appearance, Uses uses, WorkTools workTools)
{
    TestResult result = testResults.Find(x => 
        x.appearance == appearance && 
        x.uses == uses && 
        x.workTools == workTools);

    return result != null ? result.resultImage : null;
}

public string GetText(Appearance appearance, Uses uses, WorkTools workTools)
{
    TestResult result = testResults.Find(x => 
        x.appearance == appearance && 
        x.uses == uses && 
        x.workTools == workTools);

    return result != null ? result.resultText : "No result found.";
}

    // public void Start()
    // {
    //     PopulateTestResults();
    // }

    // [ContextMenu("Generate Test Results in Inspector")]
    // private void PopulateTestResults()
    // {
    //     //testResults.Clear(); // Clear existing values (optional)
    //     if (testResults == null)
    //     testResults = new List<TestResult>(); // Ensure list is initialized

    // testResults.Clear(); // Clear existing values to avoid duplicates

    //     foreach (Appearance appearance in System.Enum.GetValues(typeof(Appearance)))
    //     {
    //         foreach (Uses uses in System.Enum.GetValues(typeof(Uses)))
    //         {
    //             foreach (WorkTools workTools in System.Enum.GetValues(typeof(WorkTools)))
    //             {
    //                 testResults.Add(new TestResult
    //                 {
    //                     appearance = appearance,
    //                     uses = uses,
    //                     workTools = workTools,
    //                     resultImage = null, // Set in the Inspector later
    //                     resultText = $"Result for {appearance}, {uses}, {workTools}"
    //                 });
    //             }
    //         }
    //     }

    //     Debug.Log($"Generated {testResults.Count} test results.");
    // }
}


