using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandButtonManager : MonoBehaviour
{
    PilotActionDataList  _pilotActions;

    public GameObject buttonPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pilotActions = Tester.Instance.PilotActions;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateButton(int pilotIndex)
    {
        foreach (PilotActionData pilotAction in _pilotActions.pilotActions)
        {
            if (pilotAction.pilot == pilotIndex)
            {
                string id = pilotAction.id;
                Instantiate(buttonPrefab,transform);
                Button button = buttonPrefab.GetComponent<Button>();
                button.onClick.AddListener(()=> ExecuteCommand(pilotIndex,id));
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = id;
            }
        }
    }

    public void ExecuteCommand(int pilotIndex, string commandIndex)
    {
        Tester.Instance.ChangeSelectedPilot(pilotIndex);
        Tester.Instance.TriggerAction(commandIndex);
    }
}
