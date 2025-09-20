using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DateSelect : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown typeDropdown;
    [SerializeField] private TMP_Dropdown yearDropdown;
    [SerializeField] private TMP_Dropdown monthDropdown;
    [SerializeField] private TMP_Dropdown dayDropdown;

    void Start()
    {
        setYearDropdown();
        setMonthDropdown();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setYearDropdown()
    {
        int currentYear = System.DateTime.Now.Year;
        int diff = typeDropdown.value == 0 ? 1911 : 0;
        yearDropdown.ClearOptions();
        for (int year = currentYear - 50; year <= currentYear; year++)
        {
            yearDropdown.options.Add(new TMP_Dropdown.OptionData((year - diff).ToString()));
        }
        yearDropdown.value = 50;
        yearDropdown.RefreshShownValue();
    }

    public void setMonthDropdown()
    {
        monthDropdown.ClearOptions();
        for (int month = 1; month <= 12; month++)
        {
            monthDropdown.options.Add(new TMP_Dropdown.OptionData(month.ToString()));
        }
        monthDropdown.value = 0;
        monthDropdown.RefreshShownValue();
    }
    public void setDayDropdown()
    {
        int year = System.DateTime.Now.Year - 50 + yearDropdown.value;
        Debug.Log(year);
        int month = monthDropdown.value + 1;
        int daysInMonth = System.DateTime.DaysInMonth(year, month);

        dayDropdown.ClearOptions();
        for (int day = 1; day <= daysInMonth; day++)
        {
            dayDropdown.options.Add(new TMP_Dropdown.OptionData(day.ToString()));
        }
        dayDropdown.value = 0;
        dayDropdown.RefreshShownValue();
    }
}
