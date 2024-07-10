using UnityEngine;

public class IngameUpgradableState
{
    string upgradableName;
    int grade;
}

public class InClinicUserProgress
{
    const int BASE_CONTINUOUS_INCOME = 1; // 1 cent
    const int BASE_SUCCESSFUL_HEAL_INCOME = 1000; // 10 dollars
    const int DEFAULT_GRADE = 1;


    // Loadable State variables
    public int totalMoney;
    public int squeezePowerGrade;
    public int healingRateGrade;
    public int incomeGrade;
    public int lastLevelUnlocked;

    // In-Game State variables
    public int continousIncome;
    public int successfulHealIncome;
    
    
    public void Save(int clinicIndex = 0)
    {
        PlayerPrefs.SetInt("totalMoney" + clinicIndex, totalMoney);
        PlayerPrefs.SetInt("squeezePowerGrade" + clinicIndex, squeezePowerGrade);

        PlayerPrefs.SetInt("healingRateGrade" + clinicIndex, healingRateGrade);
        PlayerPrefs.SetInt("incomeGrade" + clinicIndex, incomeGrade);

    }

    public void Load(int clinicIndex = 0, int incomeMultiplierOfClinic = 1)
    {
        totalMoney = PlayerPrefs.GetInt("totalMoney" + clinicIndex,0);
        squeezePowerGrade = PlayerPrefs.GetInt("squeezePowerGrade" + clinicIndex, DEFAULT_GRADE);
        healingRateGrade = PlayerPrefs.GetInt("healingRateGrade" + clinicIndex, DEFAULT_GRADE);
        incomeGrade = PlayerPrefs.GetInt("incomeGrade" + clinicIndex, DEFAULT_GRADE);
        SetInGameIncomeVariables(incomeMultiplierOfClinic);
    }

    private void SetInGameIncomeVariables(int incomeMultiplierOfClinic)
    {
        continousIncome = incomeGrade * BASE_CONTINUOUS_INCOME * incomeMultiplierOfClinic;
        successfulHealIncome = incomeGrade * BASE_SUCCESSFUL_HEAL_INCOME * incomeMultiplierOfClinic;
    }


}
