using UnityEngine;
using TMPro;

public class MultiResourceCounter : MonoBehaviour
{
    [Header("Player 1 UI")]
    public TMP_Text player1oreText;
    public TMP_Text player1waterText;
    public TMP_Text player1foodText;
    public TMP_Text player1popText;
    public TMP_Text player1powerText;
    public TMP_Text player1techPointText;
    public TMP_Text player1maxBuildLevelText;
    private int player1ore = 0, player1water = 0, player1food = 0, player1pop = 0, player1power = 0, player1techPoint = 0, player1maxBuildLevel = 0;

    [Header("Player 2 UI")]
    public TMP_Text player2oreText;
    public TMP_Text player2waterText;
    public TMP_Text player2foodText;
    public TMP_Text player2popText;
    public TMP_Text player2powerText;
    public TMP_Text player2techPointText;
    public TMP_Text player2maxBuildLevelText;
    private int player2ore = 0, player2water = 0, player2food = 0, player2pop = 0, player2power = 0, player2techPoint = 0, player2maxBuildLevel = 0;

    void Start()
    {
        UpdateUI();
    }

    // ---------------- Player 1 ----------------
    public void AddoreP1(int amount)   { player1ore += amount; UpdateUI(); }
    public void AddwaterP1(int amount)   { player1water += amount; UpdateUI(); }
    public void AddfoodP1(int amount)  { player1food += amount; UpdateUI(); }
    public void AddpopP1(int amount)   { player1pop += amount; UpdateUI(); }
    public void AddpowerP1(int amount)   { player1power += amount; UpdateUI(); }
    public void AddtechPointP1(int amount)  { player1techPoint += amount; UpdateUI(); }
    public void AddmaxBuildLevelP1(int amount)   { player1maxBuildLevel += amount; UpdateUI(); }
    
    public void SpendoreP1(int amount) { player1ore = Mathf.Max(0, player1ore - amount); UpdateUI(); }
    public void SpendwaterP1(int amount)  { player1water = Mathf.Max(0, player1water - amount); UpdateUI(); }
    public void SpendfoodP1(int amount)  { player1food = Mathf.Max(0, player1food - amount); UpdateUI(); }
    public void SpendpopP1(int amount)  { player1pop = Mathf.Max(0, player1pop - amount); UpdateUI(); }
    public void SpendpowerP1(int amount)  { player1power = Mathf.Max(0, player1power - amount); UpdateUI(); }
    public void SpendtechPointP1(int amount)  { player1techPoint = Mathf.Max(0, player1techPoint - amount); UpdateUI(); }
    public void SpendmaxBuildLevelP1(int amount)  { player1maxBuildLevel = Mathf.Max(0, player1maxBuildLevel - amount); UpdateUI(); }



    // ---------------- Player 2 ----------------
    public void AddoreP2(int amount)   { player2ore += amount; UpdateUI(); }
    public void AddwaterP2(int amount)   { player2water += amount; UpdateUI(); }
    public void AddfoodP2(int amount)  { player2food += amount; UpdateUI(); }
    public void AddpopP2(int amount)   { player2pop += amount; UpdateUI(); }
    public void AddpowerP2(int amount)   { player2power += amount; UpdateUI(); }
    public void AddtechPointP2(int amount)  { player2techPoint += amount; UpdateUI(); }
    public void AddmaxBuildLevelP2(int amount)   { player2maxBuildLevel += amount; UpdateUI(); }
    
    public void SpendoreP2(int amount) { player2ore = Mathf.Max(0, player2ore - amount); UpdateUI(); }
    public void SpendwaterP2(int amount)  { player2water = Mathf.Max(0, player2water - amount); UpdateUI(); }
    public void SpendfoodP2(int amount)  { player2food = Mathf.Max(0, player2food - amount); UpdateUI(); }
    public void SpendpopP2(int amount)  { player2pop = Mathf.Max(0, player2pop - amount); UpdateUI(); }
    public void SpendpowerP2(int amount)  { player2power = Mathf.Max(0, player2power - amount); UpdateUI(); }
    public void SpendtechPointP2(int amount)  { player2techPoint = Mathf.Max(0, player2techPoint - amount); UpdateUI(); }
    public void SpendmaxBuildLevelP2(int amount)  { player2maxBuildLevel = Mathf.Max(0, player2maxBuildLevel - amount); UpdateUI(); }

    // ---------------- Update UI ----------------
    void UpdateUI()
    {
        if (player1oreText) player1oreText.text = "ore: " + player1ore;
        if (player1waterText) player1waterText.text = "water: " + player1water;
        if (player1foodText) player1foodText.text = "food: " + player1food;
        if (player1popText) player1popText.text = "pop: " + player1pop;
        if (player1powerText) player1powerText.text = "power: " + player1power;
        if (player1techPointText) player1techPointText.text = "techPoint: " + player1techPoint;
        if (player1maxBuildLevelText) player1maxBuildLevelText.text = "maxBuildLevel: " + player1maxBuildLevel;
    

        if (player2oreText) player2oreText.text = "ore: " + player2ore;
        if (player2waterText) player2waterText.text = "water: " + player2water;
        if (player2foodText) player2foodText.text = "food: " + player2food;
        if (player2popText) player2popText.text = "pop: " + player2pop;
        if (player2powerText) player2powerText.text = "power: " + player2power;
        if (player2techPointText) player2techPointText.text = "techPoint: " + player2techPoint;
        if (player2maxBuildLevelText) player2maxBuildLevelText.text = "maxBuildLevel: " + player2maxBuildLevel;
    
    }
}
