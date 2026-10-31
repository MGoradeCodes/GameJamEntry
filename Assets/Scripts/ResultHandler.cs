
using TMPro;
using UnityEngine;

public class ResultHandler : MonoBehaviour
{
    private float score;
    private float percentage;


    [SerializeField] private TextMeshProUGUI resultText;

    void Start()
    {
        resultText = GetComponent<TextMeshProUGUI>();
        score = HandleInteract.shardCount;

        percentage = Mathf.Round((score / 9f) * 100f);

        string memoryMessage = $"You remember about {percentage}% of your memories.\n\n";

        if (percentage <= 33f)
        {
            resultText.text = memoryMessage +
                "\"You searched for answers, but most remained buried.\"\n\n" +
                "Faces fade. Names lose their meaning. " +
                "The past becomes a distant whisper, almost impossible to hear.\n\n" +
                "And somewhere in the silence, a part of you disappears.\n\n" +
                "Some memories are lost forever.";
        }
        else if (percentage <= 66f)
        {
            resultText.text = memoryMessage +
                "\"You remember enough to know that something is missing.\"\n\n" +
                "Fragments return to you. A familiar face. " +
                "A forgotten place. A feeling you cannot explain.\n\n" +
                "But the pieces refuse to fit together. " +
                "The truth remains just beyond your reach.\n\n" +
                "You remember, but you do not understand.";
        }
        else if (percentage <= 83f)
        {
            resultText.text = memoryMessage +
                "\"You came so close to remembering everything.\"\n\n" +
                "The past begins to take shape. The confusion fades, " +
                "and the fragments finally start to make sense.\n\n" +
                "Yet one memory remains buried. " +
                "One unanswered question follows you into the silence.\n\n" +
                "You are almost whole. Almost.";
        }
        else
        {
            resultText.text = memoryMessage +
                "\"You remembered.\"\n\n" +
                "Every fragment returns to its place. " +
                "The faces, the moments, the feelings you thought were gone.\n\n" +
                "For the first time, the past feels whole again.\n\n" +
                "You cannot change what happened. " +
                "But you can finally understand it.\n\n" +
                "You are no longer lost.";
        }
    }


}