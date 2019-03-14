using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JamButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject SelectArrow;
    public GameObject SelectLock;
    public Text ButtonText;

    private Button m_Button;
    private JamDisplay m_Target;
    private SelectJamLinker.Jam m_Jam;

    private bool m_CanDisplay = false;
    private bool m_IsForced = false;

    public void Initialize(SelectJamLinker.Jam Jam, JamDisplay DisplayTarget)
    {
        m_Button = GetComponent<Button>();

        m_Target = DisplayTarget;
        m_Jam = Jam;

        ButtonText.text = GameManager.Instance.Data.GetGameplayLoc(Jam.NameKey);

        SelectArrow.GetComponent<Bounce>().Initialize();
        SelectArrow.SetActive(false);

        SelectLock.SetActive(!Jam.Unlocked);
        m_CanDisplay = Jam.Unlocked;
        m_Button.interactable = Jam.Unlocked;

        m_Button.onClick.AddListener(() => { m_Target.ShowDisplay(m_Jam, true, this); });
        m_Button.onClick.AddListener(() => { SelectArrow.SetActive(true); });
        m_Button.onClick.AddListener(() => { SelectArrow.GetComponent<Bounce>().Play(); });
        m_Button.onClick.AddListener(() => { m_IsForced = true; });
    }

    public void LostControl()
    {
        SelectArrow.GetComponent<Bounce>().Stop();
        SelectArrow.SetActive(false);
        m_IsForced = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_CanDisplay)
        {
            m_Target.ShowDisplay(m_Jam, false);
            SelectArrow.SetActive(true);
            SelectArrow.GetComponent<Bounce>().Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Target.HideDisplay();

        if (!m_IsForced)
        {
            SelectArrow.SetActive(false);
            SelectArrow.GetComponent<Bounce>().Stop();
        }
    }
}