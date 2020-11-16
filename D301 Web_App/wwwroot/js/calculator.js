
var currentTdee = document.getElementById("tdee").innerHTML;
var updatedTdee = document.getElementById("adjusted_tdee");

var carbGoal = document.getElementById("carb_goal");
var proteinGoal = document.getElementById("protein_goal");
var fatGoal = document.getElementById("fat_goal");
var sfaGoal = document.getElementById("sfa_goal");
var mufaGoal = document.getElementById("mufa_goal");
var pufaGoal = document.getElementById("pufa_goal");

var goalDropDown = document.getElementById("goalList");

var carbPercentage = 0;
var proteinPercentage = 0;
var fatPercentage = 0;
var sfaPercentage = 0;
var mufaPercentage = 0;
var pufaPercentage = 0;

var calculatedTdee;
var totalPercentage = 0;
var SubFatPercentage = 0;

var perct = document.getElementById("totperc");

var carbInput = document.getElementById("onecarb");
var proteinInput = document.getElementById("oneprotein");
var fatInput = document.getElementById("onefat");
var sfaInput = document.getElementById("onesfa");
var mufaInput = document.getElementById("onemufa");
var pufaInput = document.getElementById("onepufa");

var goalUnitInput = document.getElementById("goalUnit");

var btn_submit = document.getElementById("btnSubmit");

//--------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>


function calculateEnergy(e) {

    var selectedGoalValue = e.options[e.selectedIndex].value;
    var selectedGoalText = e.options[e.selectedIndex].text;

    var intCurrentTdee = parseInt(currentTdee, 10);
    var intSelectedGoalValue;

    if (goalUnitInput.value == "KJ") {
        intSelectedGoalValue = parseInt(selectedGoalValue * 4.184, 10);
    }
    else {
        intSelectedGoalValue = parseInt(selectedGoalValue, 10);
    }

    if (selectedGoalText.includes("Lose")) {
        calculatedTdee = intCurrentTdee - intSelectedGoalValue;
    }
    else if (selectedGoalText.includes("Gain")) {
        calculatedTdee = intCurrentTdee + intSelectedGoalValue;
    }
    else if (selectedGoalText.includes("Maintain")) {
        calculatedTdee = intCurrentTdee;
    }

    updatedTdee.value = calculatedTdee;

    calculateCarbs(carbInput);
    calculateProtein(proteinInput);
    calculateFat(fatInput);
}

function ftesting() {
    calculateCarbs(carbInput);
    calculateProtein(proteinInput);
    calculateFat(fatInput);
}

function setDefaultMacroPercentages() {
    carbInput.value = 55;
    proteinInput.value = 15;
    fatInput.value = 30;
}

//--------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>


function calculateTotalPercentage() {
    totalPercentage = carbPercentage + proteinPercentage + fatPercentage;
    perct.innerHTML = totalPercentage + "%";

    if (totalPercentage == 100) {
        btn_submit.disabled = false;
        perct.style.color = "green";
    }
    else {
        btn_submit.disabled = true;
        perct.style.color = "red";
    }

}

function calculateFatSubGroupPercentage() {
    SubFatPercentage = sfaPercentage + mufaPercentage + pufaPercentage;

}

//--------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>

function calculateCarbs(e) {
    var selectedCarbValue = e.options[e.selectedIndex].value;

    var intUpdatedTdee = parseInt(updatedTdee.value, 10);

    var carbCalories = (selectedCarbValue / 100) * intUpdatedTdee;

    var carbGrams;

    if (goalUnitInput.value == "KJ") {
        carbGrams = parseInt(carbCalories / 16.7, 10);
    }
    else {
        carbGrams = parseInt(carbCalories / 4, 10);
    }


    carbGoal.value = carbGrams;
    carbPercentage = parseInt(selectedCarbValue, 10);

    document.getElementById("lblCarb").innerHTML = "Carbohydrates: " + carbGrams + " g";

    calculateTotalPercentage();
}

//--------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>

function calculateProtein(e) {
    var selectedProteinValue = e.options[e.selectedIndex].value;

    var intUpdatedTdee = parseInt(updatedTdee.value, 10);

    var proteinCalories = (selectedProteinValue / 100) * intUpdatedTdee;

    var proteinGrams;

    if (goalUnitInput.value == "KJ") {
        proteinGrams = parseInt(proteinCalories / 16.7, 10);
    }
    else {
        proteinGrams = parseInt(proteinCalories / 4, 10);
    }

    proteinGoal.value = proteinGrams;
    proteinPercentage = parseInt(selectedProteinValue, 10);
    document.getElementById("lblProtein").innerHTML = "Protein: " + proteinGrams + " g";
    calculateTotalPercentage();
}

//--------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>

function calculateFat(e) {
    var selectedFatValue = e.options[e.selectedIndex].value;

    var intUpdatedTdee = parseInt(updatedTdee.value, 10);

    var fatCalories = (selectedFatValue / 100) * intUpdatedTdee;

    var fatGrams;

    if (goalUnitInput.value == "KJ") {
        fatGrams = parseInt(fatCalories / 37.7, 10);
    }
    else {
        fatGrams = parseInt(fatCalories / 9, 10);
    }

    fatGoal.value = fatGrams;
    fatPercentage = parseInt(selectedFatValue, 10);
    document.getElementById("lblFat").innerHTML = "Fat: " + fatGrams + " g";
    calculateTotalPercentage();
}

//--------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>
// TODO - refactor code
function calculateSfa(e) {
    var selectedSfaValue = e.options[e.selectedIndex].value;

    var fatCalories;

    if (goalUnitInput.value == "KJ") {
        fatCalories = parseInt(fatGoal.value * 37.7, 10);
    }
    else {
        fatCalories = parseInt(fatGoal.value * 9, 10);
    }

    var SfaCalories = (selectedSfaValue / 100) * fatCalories;

    var sfaGrams;

    if (goalUnitInput.value == "KJ") {
        sfaGrams = parseInt(SfaCalories / 37.7, 10);
    }
    else {
        sfaGrams = parseInt(SfaCalories / 9, 10);
    }

    sfaGoal.value = sfaGrams;
    sfaPercentage = parseInt(selectedSfaValue, 10);

    document.getElementById("lblSfa").innerHTML = "Saturated Fatty Acids: " + sfaGrams + " g";
    calculateFatSubGroupPercentage();
}

function calculateMufa(e) {
    var selectedMufaValue = e.options[e.selectedIndex].value;

    var fatCalories;

    if (goalUnitInput.value == "KJ") {
        fatCalories = parseInt(fatGoal.value * 37.7, 10);
    }
    else {
        fatCalories = parseInt(fatGoal.value * 9, 10);
    }

    var MufaCalories = (selectedMufaValue / 100) * fatCalories;

    var mufaGrams;

    if (goalUnitInput.value == "KJ") {
        mufaGrams = parseInt(MufaCalories / 37.7, 10);
    }
    else {
        mufaGrams = parseInt(MufaCalories / 9, 10);
    }

    mufaGoal.value = mufaGrams;
    mufaPercentage = parseInt(selectedMufaValue, 10);

    document.getElementById("lblMufa").innerHTML = "Monounsaturated Fatty Acids: " + mufaGrams + " g";
    calculateFatSubGroupPercentage();
}

function calculatePufa(e) {
    var selectedPufaValue = e.options[e.selectedIndex].value;

    var fatCalories;

    if (goalUnitInput.value == "KJ") {
        fatCalories = parseInt(fatGoal.value * 37.7, 10);
    }
    else {
        fatCalories = parseInt(fatGoal.value * 9, 10);
    }

    var PufaCalories = (selectedPufaValue / 100) * fatCalories;

    var pufaGrams;

    if (goalUnitInput.value == "KJ") {
        pufaGrams = parseInt(PufaCalories / 37.7, 10);
    }
    else {
        pufaGrams = parseInt(PufaCalories / 9, 10);
    }

    pufaGoal.value = pufaGrams;
    pufaPercentage = parseInt(selectedPufaValue, 10);

    document.getElementById("lblPufa").innerHTML = "Polyunsaturated Fatty Acids: " + pufaGrams + " g";
    calculateFatSubGroupPercentage();
}