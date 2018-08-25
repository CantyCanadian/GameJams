using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public enum PossibleAbilities
    {
        None,
        Hover,
        Rock
    }

    public GameObject RockPrefab;

    public GameObject Glider;

    private int m_CurrentAbilityIndex = 0;
    private bool m_LockMouse = false;
    private PossibleAbilities m_CurrentAbility = PossibleAbilities.None;
    private Dictionary<PossibleAbilities, bool> m_Abilities;

	void Start ()
    {
        m_Abilities = new Dictionary<PossibleAbilities, bool>();

        //Debug
        m_Abilities[PossibleAbilities.None] = true;
        m_Abilities[PossibleAbilities.Hover] = false;
        m_Abilities[PossibleAbilities.Rock] = false;
    }
	
	void Update ()
    {
        UpdateMouseWheelInput();
        UpdateMouseClickInput();

        if (Input.GetKeyDown(KeyCode.U))
        {
            m_Abilities[PossibleAbilities.Hover] = true;
        }
	}

    void UpdateMouseWheelInput()
    {
        float mouse = Input.GetAxis("Mouse ScrollWheel");

        if (m_LockMouse == false && mouse > 0.0f)
        {
            m_LockMouse = true;

            while (true)
            {
                m_CurrentAbilityIndex++;
                if (m_CurrentAbilityIndex > 2)
                {
                    m_CurrentAbilityIndex = 0;
                }

                if (UpdateAbility())
                {
                    break;
                }
            }
        }

        if (m_LockMouse == false && mouse < 0.0f)
        {
            m_LockMouse = true;

            while (true)
            {
                m_CurrentAbilityIndex--;
                if (m_CurrentAbilityIndex < 0)
                {
                    m_CurrentAbilityIndex = 2;
                }

                if (UpdateAbility())
                {
                    break;
                }
            }
        }

        if (m_CurrentAbility == PossibleAbilities.Hover)
        {
            Glider.SetActive(true);
        }
        else
        {
            Glider.SetActive(false);
        }

        if (m_LockMouse == true && mouse == 0.0f)
        {
            m_LockMouse = false;
        }
    }

    void UpdateMouseClickInput()
    {
        if (m_CurrentAbility == PossibleAbilities.Rock)
        {
            UpdateRock();
        }
    }

    void UpdateRock()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                Vector3 mousePos = hit.point;
                mousePos.x = transform.position.x;

                Vector3 direction = (mousePos - transform.position).normalized;

                GameObject rock = Instantiate(RockPrefab, transform.position, Quaternion.identity);
                rock.GetComponent<Rock>().Activate(direction);
            }
        }
    }

    bool UpdateAbility()
    {
        switch(m_CurrentAbilityIndex)
        {
            case 0:
                return CheckAbility(PossibleAbilities.None);

            case 1:
                return CheckAbility(PossibleAbilities.Hover);

            case 2:
                return CheckAbility(PossibleAbilities.Rock);

            default:
                Debug.Log("Impossible ability index.");
                return false;
        }
    }

    bool CheckAbility(PossibleAbilities ability)
    {
        if (m_CurrentAbility == ability)
        {
            return true;
        }
        if (m_Abilities[ability] == false)
        {
            return false;
        }
        m_CurrentAbility = ability;
        return true;
    }

    public void EnableAbility(PossibleAbilities ability)
    {
        m_Abilities[ability] = true;
    }

    public PossibleAbilities GetCurrentAbility()
    {
        return m_CurrentAbility;
    }

    public KeyValuePair<string, int> GetAbilityData()
    {
        return new KeyValuePair<string, int>(m_CurrentAbility.ToString(), m_CurrentAbilityIndex);
    }
}
