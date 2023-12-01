using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTextManager : MonoBehaviour
{
    [field: SerializeField] public List<string> DialogueList { get; private set; }

    [field: SerializeField] public List<bool> GrimPanelBoolList { get; private set; }

    [field: SerializeField] public List<bool> BobbyPanelBoolList { get; private set; }

    [field: SerializeField] public GameObject FadeToBlackPanel { get; private set; }

    [field: SerializeField] public GameObject BackgroundPanel { get; private set; }

    [field: SerializeField] public RectTransform BackgroundPanelTweenPos { get; private set; }

    [field: SerializeField] public GameObject GrimPanel { get; private set; }

    [field: SerializeField] public GameObject GrimPanel2 { get; private set; }

    [field: SerializeField] public GameObject BobbyPanel { get; private set; }

    [field: SerializeField] public GameObject BobbyPanel2 { get; private set; }

    [field: SerializeField] public GameObject DialogueTextPanel { get; private set; }

    [field: SerializeField] public RectTransform DialogTextPanelTweenPos { get; private set; }

    [SerializeField] public TextMeshProUGUI DialogueText;

    [field: SerializeField] public Image PressAToAdvance { get; private set; }

    [field: SerializeField] public Image PressXToAdvance { get; private set; }

    [field: SerializeField] public Image ClickToAdvance { get; private set; }
    [field: SerializeField] public RectTransform TutorialPanel { get; private set; }
    [field: SerializeField] public RectTransform TutorialPanel2 { get; private set; }
    [field: SerializeField] public bool ContainsTutorialPanel { get; private set; }
    [field: SerializeField] public bool TutorialPanelShown;
    [field: SerializeField] public bool ContainsTutorialPanel2 { get; private set; }
    [field: SerializeField] public bool TutorialPanel2Shown;
    [field: SerializeField] public RectTransform TutorialPanelButtonTweenPos;

    [field: SerializeField] public RectTransform TutorialPanelButtonPanel;

    [field: SerializeField] public RectTransform TutorialPanelButtonPanel2Pos;

    [field: SerializeField] public Image PressAToAdvanceTut { get; private set; }

    [field: SerializeField] public Image PressXToAdvanceTut { get; private set; }

    [field: SerializeField] public Image ClickToAdvanceTut { get; private set; }






}
