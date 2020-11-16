var media_query = window.matchMedia("(max-width: 767px)")
$(".dropdown-content-intake").css("display", "none")

media_query.addListener(showButtons)

function showButtons() {
    var food_buttons = document.getElementsByClassName("food-buttons")
        
    if (media_query.matches) { 
        //if browser width < 767px
        for (let i = 0; i < food_buttons.length; i++) {
            if (food_buttons[i].classList.contains("show")) {
                food_buttons[i].classList.remove("show")
            }
        }
    } else { 
        //if browser width > 767px
        for (let i = 0; i < food_buttons.length; i++) {
            if (!food_buttons[i].classList.contains("show")) {
                food_buttons[i].classList.add("show")
            }
        }
    }
}

showButtons() //run at load time




$(function () {
    $('#intake-btn').on("click", function (e) {

        intakeBody = document.getElementById("intakeBody");
        foodDetailsBody = document.getElementById("foodDetailsBody");
        foodDetailsBody.style.display = "none"
        intakeBody.style.display == "none"
        

        e.preventDefault();
        //perform the url load  then
        $('#intakeModal').modal({
            keyboard: true
        }, 'show');
        return false;
    })
})


$(function () {
    $('#viewDetailsBtn').on("click", function () {

        intakeBody = document.getElementById("intakeBody");
        foodDetailsBody = document.getElementById("foodDetailsBody");

        if (foodDetailsBody.style.display == "none") {
                intakeBody.style.display = "none";
                foodDetailsBody.style.display = "block";
            }
            else {
                intakeBody.style.display = "block";
                foodDetailsBody.style.display = "none";
            }
        


    })

    $('#viewIntakeBtn').on("click", function () {

        intakeBody = document.getElementById("intakeBody");
        foodDetailsBody = document.getElementById("foodDetailsBody");

        if (intakeBody.style.display == "none") {
            foodDetailsBody.style.display = "none";
            intakeBody.style.display = "block";
        }
        else {
            foodDetailsBody.style.display = "block";
            intakeBody.style.display = "none";
        }



    })
})

